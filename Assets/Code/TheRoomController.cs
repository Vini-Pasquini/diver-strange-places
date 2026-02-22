using UnityEngine;

public class TheRoomController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(true);
        }
    }
}
