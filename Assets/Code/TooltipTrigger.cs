using UnityEngine;

public class TooltipTrigger : MonoBehaviour
{
    [Header("Display Mode")]
    [SerializeField] private string _message;
    [SerializeField] private bool _automaticFadeOut;
    [Range(2f, 10f)]
    [SerializeField] private float _fadeDelay;

    [Header("Disable Mode")]
    [SerializeField] private bool _disableMode;

    private bool _triggered = false;

    private Timer _autoFadeTimer;

    private void Start()
    {
        this._autoFadeTimer = new Timer(this._fadeDelay, false);
    }

    private void Update()
    {
        if (this._autoFadeTimer.UpdateTimer())
        {
            GameManager.Instance.SetMessageDisplayActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this._triggered) return;

        if (collision.CompareTag("Player"))
        {
            this._triggered = true;
            
            if (this._disableMode)
            {
                GameManager.Instance.SetMessageDisplayActive(false);
                return;
            }

            GameManager.Instance.SetMessageDisplayActive(true, this._message);
            if (this._automaticFadeOut) { this._autoFadeTimer.Start(); }
        }
    }
}
