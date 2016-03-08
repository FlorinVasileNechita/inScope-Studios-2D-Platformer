using UnityEngine;
using System.Collections;

public class Enemy : Character {

    private IEnemyState currentState;
    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float throwRange;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    private bool dumb;
    private bool immortal;
    private float immortalDuration = 0.4f;
    private SpriteRenderer mSpriteRenderer;

    [SerializeField]
    private Vector3 startPos;

    public Canvas healthCanvas;

    public bool InMeleeRange {
        get {
            if (Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public bool InThrowRange {
        get {
            if (Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }

            return false;
        }
    }

	// Use this for initialization
	public override void Start () {
        base.Start();
        ChangeDirection();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        immortal = false;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsDead) {
            if (!TakingDamage) {
                currentState.Execute();
            }
            LookAtTarget();
        }

        //////////////////////////////
        // DUMBEST SHIT EVER UNITY PLS
        //////////////////////////////
        if (dumb) {
            transform.Translate(new Vector3(0.0001f, 0, 0));
        }
        else {
            transform.Translate(new Vector3(-0.0001f, 0, 0));
        }
        dumb = !dumb;
	}

    public void ChangeState(IEnemyState newState) {
        if (currentState != null) {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move() {
        if (!Attack) {
            if (GetDirection().x > 0 && transform.position.x < rightEdge.position.x || GetDirection().x < 0 && transform.position.x > leftEdge.position.x) {
                mAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
            }
            else if (currentState is PatrolState) {
                ChangeDirection();
            }
        }
    }

    private void LookAtTarget() {
        if (Target != null) {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight) {
                ChangeDirection();
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


    public void RemoveTarget() {
        Target = null;
        ChangeState(new PatrolState());
    }

    public Vector2 GetDirection() {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }


    public override IEnumerator TakeDamage() {
        if (!healthCanvas.isActiveAndEnabled) {
            healthCanvas.enabled = true;
        }
        if (!immortal && !IsDead) {
            if (Random.Range(0, 10) == 0) {
                playerHealth.CurrentValue -= 20;
                
                CombatTextManager.Instance.CreateText(transform.position, "20", Color.red, true);
            }
            else {
                playerHealth.CurrentValue -= 10;
                CombatTextManager.Instance.CreateText(transform.position, "10", Color.red, false);
            }
        }

    
        if (!IsDead) {
            mAnimator.SetTrigger("damage");
            immortal = true;
            StartCoroutine(IndicateImmortal());
            yield return new WaitForSeconds(immortalDuration);
            immortal = false;
        }
        else {
            mAnimator.SetTrigger("death");
        }
        yield return null;
    }

    public override bool IsDead {
        get {
            return playerHealth.CurrentValue <= 0;
        }
    }

    public override void ChangeDirection() {
        facingRight = !facingRight;
        Vector3 newCanvasT = healthCanvas.transform.position;
        healthCanvas.transform.parent = null;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        healthCanvas.transform.parent = this.transform;
        healthCanvas.transform.position = newCanvasT;
    }

    public override void Death() {
        mAnimator.ResetTrigger("death");
        mAnimator.SetTrigger("idle");

        playerHealth.CurrentValue = playerHealth.MaxValue;
        transform.position = startPos;
        healthCanvas.enabled = false;
    }
}
