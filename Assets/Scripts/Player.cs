using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Rigidbody2D mRigidBody;
    private Animator mAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;
    private bool attacking;
    private bool sliding;

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
        HandleMovement(hInput);
        Flip(hInput);
        HandleAttacks();

        ResetFrame();
	}    

    private void HandleMovement(float hInput) {
        // Only move if we are not attacking
        if (!mAnimator.GetBool("slide") && !mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) {
            mRigidBody.velocity = new Vector2(hInput * movementSpeed, mRigidBody.velocity.y);
            
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
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            sliding = true;
        }
    }

    // Enables attack animation
    private void HandleAttacks() {
        if (attacking && !mAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) {
            mAnimator.SetTrigger("attack");
            mRigidBody.velocity = Vector2.zero;
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
    }
}
