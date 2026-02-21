using UnityEngine;
using UnityEngine.UI;

//public enum KilledBy
//{
//    None,
//    Sight,
//    Sound,
//}

public class GameManager : PersistentSingleton<GameManager>
{
    //private KilledBy _deathMethod = KilledBy.None;
    //public KilledBy DeathMethod { get { return this._deathMethod; } }

    [Header("For Testing Only")]
    [SerializeField] private RawImage playerVisibleIndicator;
    [SerializeField] private RawImage nearbyFishIndicator;
    //[SerializeField] private RawImage bubbleNoiseIndicator;
    [SerializeField] private RawImage airPressureIndicator;

    [Header("Actual GameManager Stuff")]
    
    public bool playerVisible = false;
    public bool fishNearby = false;

    public EnemyState currentEnemyState;

    //private float _bubbleNoise = 0f;
    //public float BubbleNoise
    //{
    //    get { return this._bubbleNoise; }

    //    set
    //    {
    //        if (value >= 1f) { this._bubbleNoise = 1f; return; }
    //        if (value <= 0f) { this._bubbleNoise = 0f; return; }

    //        this._bubbleNoise = value;
    //    }
    //}

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

    private HUDController _hudController;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        this._hudController = GameObject.Find("HUDController").GetComponent<HUDController>();
    }

    private void Update()
    {
        this.playerVisibleIndicator.color = !this.playerVisible ? Color.red : Color.green;
        this.nearbyFishIndicator.color = !this.fishNearby ? Color.red : Color.green;
        this.airPressureIndicator.transform.localScale = new Vector3(this._airPressure, 1f, 1f);

        //this.bubbleNoiseIndicator.color = Color.Lerp(Color.green, Color.red, this._bubbleNoise);
        //this.bubbleNoiseIndicator.transform.localScale = new Vector3(this._bubbleNoise, 1f, 1f);

        //if ((this.playerVisible && this.fishNearby) || this._bubbleNoise >= 1f)
        //{
        //    this._deathMethod = this.playerVisible ? KilledBy.Sight : KilledBy.Sound;
        //}

        if ((this.playerVisible && this.fishNearby)) { this.currentEnemyState = EnemyState.Kill; }
    }

    public void SetMessageDisplayActive(bool enable, string msg = "")
    {
        if (enable)
        {
            this._hudController.EnableTooltip(msg);
            return;
        }
        this._hudController.DisableTooltip();
    }

    private PathNode _activePathNode;
    public PathNode ActivePathNode { get { return this._activePathNode; } }
    public bool justChangedPath;

    public void RegisterActivePathNode(PathNode activePathNode)
    {
        if (activePathNode == this._activePathNode) return;
        this._activePathNode = activePathNode;
        this.justChangedPath = true;
    }

    public Vector3 soundOrigin = Vector3.zero;

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
