using UnityEngine;
using System.Collections;

public class Enemy : Character {

    private IEnemyState currentState;

	// Use this for initialization
	public override void Start () {
        base.Start();
        
        ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update () {
        currentState.Execute();
	}

    public void ChangeState(IEnemyState newState) {
        if (currentState != null) {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move() {
        mAnimator.SetFloat("speed", 1);

        transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
    }

    public Vector2 GetDirection() {
        return facingRight ? Vector2.right : Vector2.left;
    }
}
