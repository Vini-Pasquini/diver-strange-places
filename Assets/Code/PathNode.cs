using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private GameObject _waypointsParent;
    
    private Vector3[] _waypoints;
    public Vector3[] Waypoints { get { return _waypoints; } }

    private void Start()
    {
        this._waypoints = new Vector3[this._waypointsParent.transform.childCount];
        for (int i = 0; i < this._waypoints.Length; i++)
        {
            this._waypoints[i] = this._waypointsParent.transform.GetChild(i).transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RegisterActivePathNode(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 1; i < _waypointsParent.transform.childCount; i++) Gizmos.DrawLine(_waypointsParent.transform.GetChild(i - 1).position, _waypointsParent.transform.GetChild(i).position);
        Gizmos.DrawLine(_waypointsParent.transform.GetChild(_waypointsParent.transform.childCount - 1).position, _waypointsParent.transform.GetChild(0).position);
    }
#endif
}
