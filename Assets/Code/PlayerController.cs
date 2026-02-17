using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _gravity = 1.7f;
    private float _boostSpeed = 3f;
    private float _walkSpeed = 1.5f;

    private bool _isGrounded;
    private bool _isBoosting;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { this._isBoosting = true; }
        if (Input.GetKeyUp(KeyCode.W)) { this._isBoosting = false; }

        float horizontalVelocity = Input.GetAxis("Horizontal") * this._walkSpeed;
        float verticalVelocity = this._isBoosting ? this._boostSpeed : (this._isGrounded ? -.1f : -this._gravity);

        this._rigidbody.linearVelocity = new Vector3(horizontalVelocity, verticalVelocity, 0f);
        this._isGrounded = Physics.Raycast(this.transform.position, Vector3.down, .6f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position - (this.transform.up * .6f));
    }
}
