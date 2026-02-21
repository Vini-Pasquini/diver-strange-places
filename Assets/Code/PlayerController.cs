using UnityEngine;

public enum PlayerAnimation
{
    Idle,
    Walk,
    StartBoost,
    Boosting,
    StopBoost,
    Falling,
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private float _gravity = 1.7f;
    private float _boostSpeed = 2f;
    private float _walkSpeed = 1f;

    private bool _isGrounded;
    private bool _isBoosting;
    private bool _skipStopBoostAnim;

    private bool _flipAnimDir;

    private float _horizontalVelocity;
    private float _verticalVelocity;

    private Timer _boostDamp = new Timer(1f, false);
    private Timer _fallDamp = new Timer(1f, false);

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody2D>();
        this._spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this._animator = this.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.AirPressure > 0f)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                this._isBoosting = true;
                this._skipStopBoostAnim = false;
                this._boostDamp.Start();
                this.PlayAnim(PlayerAnimation.StartBoost);
                //GameManager.Instance.BubbleNoise += .1f;
                GameManager.Instance.AlertFish(this.transform.position);
            }
            if (Input.GetKeyUp(KeyCode.W)) { this.StopBoosting(); }
        }

        float boostMultiplier = this._boostDamp.Running ? ((this._boostDamp.Delay - this._boostDamp.RemainingSeconds) / this._boostDamp.Delay) : 1f;
        float fallMultiplier = this._fallDamp.Running ? ((this._fallDamp.Delay - this._fallDamp.RemainingSeconds) / this._fallDamp.Delay) : 1f;

        this._horizontalVelocity = Input.GetAxis("Horizontal") * this._walkSpeed;
        this._verticalVelocity = this._isBoosting ? this._boostSpeed * boostMultiplier : (this._isGrounded ? -.1f : -this._gravity * fallMultiplier);
        
        this._boostDamp.UpdateTimer();
        this._fallDamp.UpdateTimer();

        bool isGroundedBuffer = this._isGrounded;

        this._rigidbody.linearVelocity = new Vector2(this._horizontalVelocity, this._verticalVelocity);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, .55f, 1 << 31);
        this._isGrounded = hit.collider;

        if (isGroundedBuffer != this._isGrounded) { this._fallDamp.Start(); }

        this.UpdateAnimation();

        if (this._isBoosting)
        {
            GameManager.Instance.AirPressure -= Time.deltaTime / 3f;
            GameManager.Instance.AlertFish(this.transform.position);
        }
        else if (this._isGrounded) { GameManager.Instance.AirPressure += Time.deltaTime / 5f; }

        if (this._isBoosting && GameManager.Instance.AirPressure <= 0f) { this.StopBoosting(); }

        //if (GameManager.Instance.fishNearby && this._isBoosting) { GameManager.Instance.BubbleNoise += (Time.deltaTime / 3f) * (this._verticalVelocity / this._boostSpeed); }
        //else { GameManager.Instance.BubbleNoise -= Time.deltaTime / 6f; }
    }

    private void PlayAnim(PlayerAnimation playerAnimation)
    {
        this._animator.Play(playerAnimation.ToString() + (this._flipAnimDir ? "_L" : "_R"));
    }

    private void UpdateAnimation()
    {
        this._flipAnimDir = _horizontalVelocity == 0 ? this._flipAnimDir : this._horizontalVelocity < 0;
        this._animator.SetBool("FLIP", this._flipAnimDir);
        
        if (this._isBoosting) return; // boost anim starts on keydown
        
        if (this._isGrounded)
        {
            this._skipStopBoostAnim = true;
            if (this._horizontalVelocity == 0) { this.PlayAnim(PlayerAnimation.Idle); }
            else { this.PlayAnim(PlayerAnimation.Walk); }
            return;
        }

        if (this._skipStopBoostAnim) { this.PlayAnim(PlayerAnimation.Falling); }
    }

    private void StopBoosting()
    {
        this._isBoosting = false;
        this._fallDamp.Start();
        this.PlayAnim(PlayerAnimation.StopBoost);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position - (this.transform.up * .55f));
    }
#endif
}
