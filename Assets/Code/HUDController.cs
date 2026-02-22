using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
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

    public void OnChangeSceneButtonPress(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
}
