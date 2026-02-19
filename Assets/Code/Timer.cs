using UnityEngine;

public struct Timer
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
