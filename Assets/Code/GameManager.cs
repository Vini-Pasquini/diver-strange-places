using UnityEngine;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("For Testing Only")]
    [SerializeField] private RawImage playerVisibleIndicator;
    [SerializeField] private RawImage nearbyFishIndicator;
    [SerializeField] private RawImage bubbleNoiseIndicator;

    [Header("Actual GameManager Stuff")]
    private float _bubbleNoise = 0f;
    
    public bool playerVisible = false;
    public bool fishNearby = false;
    
    public float BubbleNoise
    {
        get { return this._bubbleNoise; }

        set
        {
            if (value >= 1f) { this._bubbleNoise = 1f; return; }
            if (value <= 0f) { this._bubbleNoise = 0f; return; }

            this._bubbleNoise = value;
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
        playerVisibleIndicator.color = !playerVisible ? Color.red : Color.green;
        nearbyFishIndicator.color = !fishNearby ? Color.red : Color.green;

        bubbleNoiseIndicator.color = Color.Lerp(Color.green, Color.red, this._bubbleNoise);
        bubbleNoiseIndicator.transform.localScale = new Vector3(this._bubbleNoise, 1f, 1f);

        if ((playerVisible && fishNearby) || this._bubbleNoise >= 1f) Debug.Log("Dead");
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
}
