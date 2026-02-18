using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private struct Timer
    {
        private float _remainingSeconds;
        private float _delay;
        private bool _loop;
        private bool _running;

        public float RemainingSeconds { get { return this._remainingSeconds; } }
        public float Delay { get { return this._delay; } }
        public bool Running { get { return this._running; } }

        public Timer(float delay, bool loop)
        {
            this._remainingSeconds = 0;
            this._delay = delay;
            this._loop = loop;
            this._running = false;
        }

        public bool UpdateTimer()
        {
            if (!this._running) return false;

            this._remainingSeconds -= Time.deltaTime;
            if (this._remainingSeconds <= 0)
            {
                if (this._loop) this.ResetTimer();
                else this.Stop();
                return true;
            }

            return false;
        }

        public void Start() { this.ResetTimer(); this._running = true; }
        public void Stop() { this._running = false; this.ResetTimer(); }
        public void Play() => this._running = true;
        public void Pause() => this._running = false;
        public void ResetTimer() => this._remainingSeconds = this._delay;
    }

    [SerializeField] private TextMeshProUGUI _tooltipText;

    private Vector2 _tooltipAnchoredPosition = Vector2.zero;

    private Timer _tooltipAnimTimer = new(.08f, loop:true);
    private Timer _tooltipFadeTimer = new(2f, loop:false);

    private bool _tooltipFadeIn = false;

    private void Start()
    {
        this._tooltipAnchoredPosition = this._tooltipText.rectTransform.anchoredPosition;
        this._tooltipAnimTimer.Start();
    }

    private void Update()
    {
        if (this._tooltipAnimTimer.UpdateTimer())
        {
            this._tooltipText.rectTransform.anchoredPosition = this._tooltipAnchoredPosition + new Vector2(Random.Range(0f, 7f), Random.Range(0f, 7f));
        }

        if (!this._tooltipFadeTimer.UpdateTimer() && this._tooltipFadeTimer.Running)
        {
            if (this._tooltipFadeIn) { this._tooltipText.alpha = (this._tooltipFadeTimer.Delay - this._tooltipFadeTimer.RemainingSeconds) / this._tooltipFadeTimer.Delay; }
            else { this._tooltipText.alpha = this._tooltipFadeTimer.RemainingSeconds / this._tooltipFadeTimer.Delay; }
        }
        
    }

    public void EnableTooltip(string newText)
    {
        this._tooltipText.text = newText;
        this._tooltipFadeIn = true;
        this._tooltipFadeTimer.Start();
    }

    public void DisableTooltip()
    {
        this._tooltipFadeIn = false;
        this._tooltipFadeTimer.Start();
    }
}
