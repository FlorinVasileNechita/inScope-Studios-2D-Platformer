using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour {
    [SerializeField]
    private float speed;
    private Vector2 direction;

    private Rigidbody2D mRigidBody;

	// Use this for initialization
	void Start () {
        mRigidBody = GetComponent<Rigidbody2D>();
	
	}

    public void Initialize(Vector2 direction) {
        this.direction = direction;
    }

    void FixedUpdate() {
        mRigidBody.velocity = direction * speed;
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
