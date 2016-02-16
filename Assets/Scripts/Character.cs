using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected float movementSpeed;
   
    [SerializeField]
    protected GameObject knifePrefab;
    [SerializeField]
    protected Transform knifeSpawn;

    protected bool facingRight;

    public Animator mAnimator { get; private set; }
    public bool Attack { get; set; }

    [SerializeField]
    protected int health;
    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }
   
	// Use this for initialization
	public virtual void Start () {
        mAnimator = GetComponent<Animator>();

        facingRight = true;
	}

    public abstract IEnumerator TakeDamage();

    public void ChangeDirection() {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value) {
        if (facingRight) {
            GameObject temp = (GameObject)Instantiate(knifePrefab, knifeSpawn.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            temp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else {
            GameObject temp = (GameObject)Instantiate(knifePrefab, knifeSpawn.position, Quaternion.Euler(new Vector3(0, 0, +90)));
            temp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Knife") {
            StartCoroutine(TakeDamage());
        }
    }
}
