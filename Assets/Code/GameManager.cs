using UnityEngine;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("For Testing Only")]
    [SerializeField] private RawImage airPressureIndicator;
    private HUDController _hudController;

    [Header("Actual GameManager Stuff")] // TODO: remove later
    
    // Player Stuff
    private PlayerAudioController _playerAudioController;

    private float _airPressure = 1f;
    public float AirPressure
    {
        get { return this._airPressure; }

        set
        {
            if (value >= 1f) { this._airPressure = 1f; return; }
            if (value <= 0f) { this._airPressure = 0f; return; }

            this._airPressure = value;
        }
    }

    public bool playerVisible = false;
    public bool fishNearby = false;

    // Fish AI
    private PathNode _activePathNode;
    public PathNode ActivePathNode { get { return this._activePathNode; } }
    public EnemyState currentEnemyState;
    public Vector3 soundOrigin = Vector3.zero;
    public bool justChangedPath;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        this._hudController = GameObject.Find("HUDController").GetComponent<HUDController>();
        this._playerAudioController = GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerAudioController>();
    }

    private void Update()
    {
        this.airPressureIndicator.transform.localScale = new Vector3(this._airPressure, 1f, 1f);

        if ((this.playerVisible && this.fishNearby)) { this.currentEnemyState = EnemyState.Kill; }
    }

    public void SetMessageDisplayActive(bool enable, string msg = "")
    {
        if (enable)
        {
            this._hudController.EnableTooltip(msg);
            this._playerAudioController.PlayMessageSound();
            return;
        }
        this._hudController.DisableTooltip();
    }

    // Fish AI
    public void RegisterActivePathNode(PathNode activePathNode)
    {
        if (activePathNode == this._activePathNode) return;
        this._activePathNode = activePathNode;
        this.justChangedPath = true;
    }

    public void AlertFish(Vector3 targetPosition)
    {
        this.justChangedPath = true;
        this.soundOrigin = targetPosition;
        this.currentEnemyState = EnemyState.Chase;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
