using UnityEngine;
using UnityEngine.UI;

public class TheRoomController : MonoBehaviour
{
    [SerializeField] private RawImage fade;
    private Timer _fade = new(2f, false);
    private bool _flag = false;

    private void Update()
    {
        if (this._fade.UpdateTimer()) { GameManager.Instance.GameOver(true); }
        this.fade.color = new Color(0f, 0f, 0f, this._flag ? (this._fade.Delay - this._fade.RemainingSeconds) / this._fade.Delay : 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this._flag = true;
            this._fade.Start();
        }
    }
}
