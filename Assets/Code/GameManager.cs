using System;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    None,
    Running,
    GameOver_Win,
    GameOver_Loss,
}

public enum GameScene
{
    Menu,
    Gameplay,
    GameOver,
}

public class GameManager : PersistentSingleton<GameManager>
{
    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState { get { return this._currentGameState; } }

    private HUDController _hudController;
    public HUDController HUDController
    {
        get
        {
            if (this._hudController) return this._hudController;

            this._hudController = GameObject.Find("HUDController").GetComponent<HUDController>();
            if (this._hudController) return this._hudController;

            return this._hudController = new GameObject("HUDController").AddComponent<HUDController>(); ;
        }
    }
    
    // Player Stuff
    private PlayerAudioController _playerAudioController;
    public PlayerAudioController PlayerAudioController
    {
        get
        {
            if (this._playerAudioController) return this._playerAudioController;

            this._playerAudioController = GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerAudioController>();
            return this._playerAudioController;
        }
    }

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

    private void Update()
    {
        //this.airPressureIndicator.transform.localScale = new Vector3(this._airPressure, 1f, 1f);
        if ((this.playerVisible && this.fishNearby)) { this.currentEnemyState = EnemyState.Kill; }
    }

    // Scene Manager
    public void ChangeScene(string sceneName)
    {
        switch (Enum.Parse(typeof(GameScene), sceneName))
        {
            case GameScene.Menu:
                this._currentGameState = GameState.None;
                break;
            case GameScene.Gameplay:
                this._currentGameState = GameState.Running;
                break;
            case GameScene.GameOver:
                break;
            default:
                break;
        }

        StartCoroutine(LoadNewScene(sceneName));
    }

    public IEnumerator LoadNewScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone) { yield return null; }
    }

    // Stuff
    public void SetMessageDisplayActive(bool enable, string msg = "")
    {
        if (enable)
        {
            this.HUDController.EnableTooltip(msg);
            this.PlayerMessageQueue();
            return;
        }
        this.HUDController.DisableTooltip();
    }

    private void PlayerMessageQueue()
    {
        if (this._currentGameState == GameState.Running) this.PlayerAudioController.PlayMessageSound();
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

    public void GameOver(bool victory)
    {
        this._currentGameState = victory ? GameState.GameOver_Win : GameState.GameOver_Loss;
        this.ChangeScene("GameOver");
    }
}
