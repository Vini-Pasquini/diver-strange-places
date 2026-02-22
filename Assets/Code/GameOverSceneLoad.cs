using UnityEngine;

public class GameOverSceneLoad : MonoBehaviour
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _lossScreen;

    private void Start()
    {
        GameState state = GameManager.Instance.CurrentGameState;
        this._winScreen.SetActive(state == GameState.GameOver_Win);
        this._lossScreen.SetActive(state == GameState.GameOver_Loss);
        GameManager.Instance.SetMessageDisplayActive(true, state == GameState.GameOver_Win ? "I've Brought You a Friend!" : "Should've been more Careful...");
    }
}
