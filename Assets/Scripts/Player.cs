using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Rigidbody2D mRigidBody;
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

    private bool facingRight;
    private bool attacking;
    private bool sliding; 
    private bool isGrounded;
    private bool jump;
    private bool jumpAttack;
    [SerializeField]
    private bool airControl;

	void Start () {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();

        facingRight = true;
	}

    void Update() {
        HandleInput();
    }

	void FixedUpdate () {
        float hInput = Input.GetAxis("Horizontal");
        
        isGrounded = IsGrounded();

        HandleMovement(hInput);
        Flip(hInput);
        HandleAttacks();

        HandleLayers();

        ResetFrame();
	}    

    private void HandleMovement(float hInput) {

        if (mRigidBody.velocity.y < 0) {
            mAnimator.SetBool("land", true);
        }

        // Only move if we are not attacking
        if (!mAnimator.GetBool("slide") && !mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && (isGrounded || airControl)) {
            mRigidBody.velocity = new Vector2(hInput * movementSpeed, mRigidBody.velocity.y);
            
        }

        if (isGrounded && jump) {
            isGrounded = false;
            mRigidBody.AddForce(new Vector2(0, jumpForce));
            mAnimator.SetTrigger("jump");
        }

        if (sliding && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("AnimSlide")) {
            mAnimator.SetBool("slide", true);
        }
        else if (!mAnimator.GetCurrentAnimatorStateInfo(0).IsName("AnimSlide")) {
            mAnimator.SetBool("slide", false);
        }

        mAnimator.SetFloat("speed", Mathf.Abs(hInput));
    }

    // Checks inputs
    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attacking = true;
            jumpAttack = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            sliding = true;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
        }
    }

    // Enables attack animation
    private void HandleAttacks() {
        if (attacking && isGrounded && !mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) {
            mAnimator.SetTrigger("attack");
            mRigidBody.velocity = Vector2.zero;
        }

        if (jumpAttack && !isGrounded && !mAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")) {
            mAnimator.SetBool("jumpAttack", true);
        }
        if (!jumpAttack && !mAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")) {
            mAnimator.SetBool("jumpAttack", false);
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

    private void ResetFrame() {
        attacking = false;
        sliding = false;
        jump = false;
        jumpAttack = false;
    }

    private bool IsGrounded() {
        if (mRigidBody.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject) {
                        mAnimator.ResetTrigger("jump");
                        mAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers() {
        if (!isGrounded) {
            mAnimator.SetLayerWeight(1, 1);
        }

        else {
            mAnimator.SetLayerWeight(1, 0);
        }
    }
}
