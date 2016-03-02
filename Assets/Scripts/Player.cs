using UnityEngine;
using System.Collections;

public delegate void DeadEventHandler();

public class Player : Character {

    private static Player instance;
    public static Player Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    
    public event DeadEventHandler Dead;

    [SerializeField] private Transform[] groundPoints;
    [SerializeField] private float groundRadius;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool airControl;

    public Rigidbody2D MyRigidbody { get; set; }
    private SpriteRenderer mSpriteRenderer { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    [SerializeField] private Vector3 startPos;

    private bool immortal = false;
    [SerializeField] private float immortalDuration;

    [SerializeField] private Stat playerHealth;

    private void Awake() {
        playerHealth.Initialize();
    }

	public override void Start () {
        base.Start();
        MyRigidbody = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Update() {
        if (!TakingDamage && !IsDead) {
            if (transform.position.y <= -14f) {
                Death();
            }
            HandleInput();
        }
    }

	void FixedUpdate () {
        if (!TakingDamage && !IsDead) {
            float hInput = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            HandleMovement(hInput);
            Flip(hInput);

            HandleLayers();
        }
	}

    public void OnDead() {
        if (Dead != null) {
            Dead();
        }
    }

    private void HandleMovement(float hInput) {
        // If we aren't moving up or down, we're on the ground.
        // (Not actually true but lolololYouTubeTutorials
        if (MyRigidbody.velocity.y < 0) {
            mAnimator.SetBool("land", true);
        }

        // If we aren't attacking, or sliding, and are either on the ground or in the air, we move.
        if (!Attack && !Slide && (OnGround || airControl)) {
            MyRigidbody.velocity = new Vector2(hInput * movementSpeed, MyRigidbody.velocity.y);
        }

        // If we want to jump and we aren't currently falling, add jump force
        if (Jump && MyRigidbody.velocity.y == 0) {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        mAnimator.SetFloat("speed", Mathf.Abs(hInput));

    }

    // Checks inputs
    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            mAnimator.SetTrigger("attack");
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            mAnimator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            mAnimator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            mAnimator.SetTrigger("throw");
        }
    }

    private void Flip(float hInput) {
        if (!mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("AnimSlide")) {
            if (hInput > 0 && !facingRight || hInput < 0 && facingRight) {
                ChangeDirection();
            }
        }
    }

    private bool IsGrounded() {
        if (MyRigidbody.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers() {
        if (!OnGround) {
            mAnimator.SetLayerWeight(1, 1);
        }

        else {
            mAnimator.SetLayerWeight(1, 0);
        }
    }

    public void KnifeAnimation(int value) {
        ThrowKnife(value);
    }
    public override void ThrowKnife(int value) {
        if (!OnGround && value == 1 || OnGround && value == 0) {
            base.ThrowKnife(value);
        }

    }

    public override IEnumerator TakeDamage() {
        if (!immortal && !IsDead) {
            if (Random.Range(0, 10) == 0) {
                playerHealth.CurrentValue -= 20;
                CombatTextManager.Instance.CreateText(transform.position, "20", Color.red, true);
            }
            else {
                playerHealth.CurrentValue -= 10;
                CombatTextManager.Instance.CreateText(transform.position, "10", Color.red, false);
            } 

            if (!IsDead) {
                mAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalDuration);

                immortal = false;

            }
            else {
                mAnimator.SetLayerWeight(1, 0);
                mAnimator.SetTrigger("death");
            }
        }
    }

    private IEnumerator IndicateImmortal() {
        while (immortal) {
            mSpriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            mSpriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override bool IsDead {
        get {
            if (playerHealth.CurrentValue <= 0) {
                OnDead();
            }
            return playerHealth.CurrentValue <= 0;
        }
    }

    public override void Death() {
        MyRigidbody.velocity = Vector2.zero;
        mAnimator.SetTrigger("idle");
        playerHealth.CurrentValue = 100;
        transform.position = startPos;
    }
}
