using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private AudioSource _jetAudioSource;
    [SerializeField] private AudioSource _bubbleAudioSource;
    [SerializeField] private AudioSource _messageAudioSource;

    public void PlayStepSound()
    {
        this._stepAudioSource.Play();
    }

    public void PlayJetSound()
    {
        this._jetAudioSource.Play();
    }

    public void StartBubbleSound()
    {
        this._bubbleAudioSource.volume = 1.0f;
    }

    public void StopBubbleSound()
    {
        this._bubbleVolumeDown.Start();
    }

    public void PlayMessageSound()
    {
        this._messageAudioSource.Play();
    }

    private Timer _bubbleVolumeDown = new(.7f, false);

    private void Update()
    {
        if (this._bubbleVolumeDown.Running && !this._bubbleVolumeDown.UpdateTimer())
        {
            this._bubbleAudioSource.volume = this._bubbleVolumeDown.RemainingSeconds / this._bubbleVolumeDown.Delay;
        }
    }
}
