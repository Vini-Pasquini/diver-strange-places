using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _fishSprite;
    [SerializeField] private Transform _lookAtFinalRot;

    private AudioSource _audioSource;
    private GameObject _player;

    private float _interpolationTime = 0f;

    private Rigidbody2D _rigidbody2D;

    private Vector3[] _waypoints;
    private int _currentWaypointIndex;

    private float _patrolSpeed = 2f;
    private float _chaseSpeed = 4f;

    private Timer _investigationTimer = new(5f, false);

    private float _soundDistance = 8f;

    private void Start()
    {
        this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        this._audioSource = this.GetComponent<AudioSource>();
        this._player = GameObject.Find("Player");

        GameManager.Instance.currentEnemyState = EnemyState.Patrol;
    }

    private void Update()
    {
        this._interpolationTime += Time.deltaTime * .1f;
        if (this._interpolationTime >= 1f) this._interpolationTime = 1f;

        switch (GameManager.Instance.currentEnemyState)
        {
            case EnemyState.Patrol: this.PatrolUpdate(); break;
            case EnemyState.Chase: this.ChaseUpdate(); break;
            case EnemyState.Investigate: this.InvestigateUpdate(); break;
            case EnemyState.Kill: this.KillUpdate(); break;
        }

        this._fishSprite.rotation = Quaternion.Lerp(this._fishSprite.rotation, this._lookAtFinalRot.rotation, this._interpolationTime);

        float distance = (this._player.transform.position - this.transform.position).magnitude;
        this._audioSource.volume = distance > this._soundDistance ? 0f : (this._soundDistance - distance) / this._soundDistance;
    }

    private void PatrolUpdate()
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

        this._rigidbody2D.linearVelocity = (this._waypoints[this._currentWaypointIndex] - this.transform.position).normalized * this._patrolSpeed;

        if ((this._waypoints[this._currentWaypointIndex] - this.transform.position).magnitude <= 0.5f)
        {
            this._currentWaypointIndex++;
            if (this._currentWaypointIndex >= this._waypoints.Length) this._currentWaypointIndex = 0;
            this._interpolationTime = 0f;
        }

        this._lookAtFinalRot.LookAt(this._waypoints[this._currentWaypointIndex]);
    }

    private void ChaseUpdate()
    {
        Vector3 soundOrigin = GameManager.Instance.soundOrigin;

        this._rigidbody2D.linearVelocity = (soundOrigin - this.transform.position).normalized * this._chaseSpeed;
        
        if ((soundOrigin - this.transform.position).magnitude <= 0.1f)
        {
            this._rigidbody2D.linearVelocity = Vector2.zero;
            GameManager.Instance.currentEnemyState = EnemyState.Investigate;
            this._investigationTimer.Start();
        }

        Vector3 dir = soundOrigin - this.transform.position;
        Vector3 lookPadding = new Vector3(dir.x / Mathf.Abs(dir.x), 0f, 0f);

        this._lookAtFinalRot.LookAt(soundOrigin + lookPadding);
    }

    private void InvestigateUpdate()
    {
        if (this._investigationTimer.UpdateTimer())
        {
            GameManager.Instance.currentEnemyState = EnemyState.Patrol;
        }
    }

    private void KillUpdate()
    {
        GameManager.Instance.GameOver(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player")) { GameManager.Instance.fishNearby = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { GameManager.Instance.fishNearby = false; }
    }
}
