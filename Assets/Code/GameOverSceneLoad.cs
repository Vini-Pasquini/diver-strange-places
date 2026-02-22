using UnityEngine;
using UnityEngine.UI;

public class GameOverSceneLoad : MonoBehaviour
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _lossScreen;

    [SerializeField] private GameObject _buttonWin;
    [SerializeField] private GameObject _buttonLoss;
    [SerializeField] private Sprite _finalSprite;

    private Timer _zoomTimer = new Timer(4f, false);

    private void Start()
    {
        GameState state = GameManager.Instance.CurrentGameState;
        this._winScreen.SetActive(state == GameState.GameOver_Win);
        this._lossScreen.SetActive(state == GameState.GameOver_Loss);
        GameManager.Instance.SetMessageDisplayActive(true, state == GameState.GameOver_Win ? "I've Brought You a Friend!" : "Should've been more Careful...");
        this._zoomTimer.Start();
    }

    private void Update()
    {
        if (this._zoomTimer.UpdateTimer())
        {
            if (GameManager.Instance.CurrentGameState == GameState.GameOver_Win)
            {
                this._winScreen.GetComponent<SpriteRenderer>().sprite = this._finalSprite;
                this._buttonWin.SetActive(true);
                this._buttonLoss.SetActive(false);
            }

            if (GameManager.Instance.CurrentGameState == GameState.GameOver_Loss)
            {
                this._buttonWin.SetActive(false);
                this._buttonLoss.SetActive(true);
            }

            GameManager.Instance.SetMessageDisplayActive(false);
        }
    }
}
