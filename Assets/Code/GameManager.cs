using UnityEngine;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("For Testing Only")]
    [SerializeField] private RawImage playerVisibleIndicator;
    [SerializeField] private RawImage insideFishSightIndicator;

    [Header("Actual GameManager Stuff")]
    public bool playerVisible = false;
    public bool insideFishSight = false;

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
        insideFishSightIndicator.color = !insideFishSight ? Color.red : Color.green;

        if (playerVisible && insideFishSight) Debug.Log("Dead");
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
