using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private Vector3[] _waypoints;
    private int _currentWaypointIndex;

    private float _movementSpeed = 2f;

    private void Start()
    {
        this._rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.ActivePathNode == null) return;

        if (GameManager.Instance.justChangedPath && GameManager.Instance.ActivePathNode.Waypoints.Length > 1)
        {
            this._waypoints = GameManager.Instance.ActivePathNode.Waypoints;
            this._currentWaypointIndex = 0;
            float prevDist = ((this._waypoints[0]) - (this.transform.position)).magnitude;
            for (int i = 1; i < this._waypoints.Length; i++)
            {
                float curDist = ((this._waypoints[i]) - (this.transform.position)).magnitude;
                if (curDist < prevDist) { this._currentWaypointIndex = i; }
                prevDist = curDist;
            }
            GameManager.Instance.justChangedPath = false;
        }

        if (this._waypoints == null) return;

        this._rigidbody2D.linearVelocity = (this._waypoints[this._currentWaypointIndex] - this.transform.position).normalized * this._movementSpeed;

        if ((this._waypoints[this._currentWaypointIndex] - this.transform.position).magnitude <= 0.1f)
        {
            this._currentWaypointIndex++;
            if (this._currentWaypointIndex >= this._waypoints.Length) this._currentWaypointIndex = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.insideFishSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.insideFishSight = false;
        }
    }
}
