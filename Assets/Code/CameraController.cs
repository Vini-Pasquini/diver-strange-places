using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private void Update()
    {
        Vector3 buffer = Vector3.Lerp(this.transform.position, this._target.transform.position, .1f);
        buffer.z = -10f;
        this.transform.position = buffer;
    }
}
