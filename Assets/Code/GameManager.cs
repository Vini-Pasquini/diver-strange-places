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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        playerVisibleIndicator.color = !playerVisible ? Color.red : Color.green;
        insideFishSightIndicator.color = !insideFishSight ? Color.red : Color.green;

        if (playerVisible && insideFishSight) Debug.Log("Dead");
    }
}
