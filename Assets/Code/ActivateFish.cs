using UnityEngine;

public class ActivateFish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.ActivateFishAI();
    }
}
