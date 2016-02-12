using UnityEngine;
using System.Collections;

public class IdleState : IEnemyState {
    private Enemy enemy;

    private float idleTimer;
    private float idleDuration = 5;

    public void Execute() {
        Idle();
    }

    public void Enter(Enemy enemy) {
        this.enemy = enemy;
    }

    public void Exit() {

    }

    public void OnTriggerEnter(Collider2D other) {

    }

    private void Idle() {
        enemy.mAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration) {
            enemy.ChangeState(new PatrolState());
        }
    }
}
