using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private static Player instance;

    public static Player Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance; 
        }
    }

    private Animator mAnimator;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject knifePrefab;
    [SerializeField]
    private Transform knifeSpawn;

    private bool facingRight;
    [SerializeField]
    private bool airControl;

    public Rigidbody2D MyRigidbody { get; set; }
    public bool Attack { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

	void Start () {
        MyRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();

        facingRight = true;
	}

    void Update() {
        HandleInput();

        // Easeier pausing
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Break();
        }
    }

	void FixedUpdate () {
        float hInput = Input.GetAxis("Horizontal");

        OnGround = IsGrounded();

        HandleMovement(hInput);
        Flip(hInput);

        HandleLayers();
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
                facingRight = !facingRight;

                // Invert X scale to flip
                Vector3 mScale = transform.localScale;
                mScale.x *= -1;
                transform.localScale = mScale;
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

    public void ThrowKnife(int value) {
        if (!OnGround && value == 1 || OnGround && value == 0) {
            if (facingRight) {
                GameObject temp = (GameObject)Instantiate(knifePrefab, knifeSpawn.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                temp.GetComponent<Knife>().Initialize(Vector2.right);
            }
            else {
                GameObject temp = (GameObject)Instantiate(knifePrefab, knifeSpawn.position, Quaternion.Euler(new Vector3(0, 0, +90)));
                temp.GetComponent<Knife>().Initialize(Vector2.left);
            }
        }

    }
}
