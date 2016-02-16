using UnityEngine;
using System.Collections;

public class MeleeState : IEnemyState {
    private Enemy enemy;

    private float attackTimer;
    private float attackCooldown = 3;
    private bool canAttack = true;

    public void Execute() {
        Attack();

        if (enemy.InThrowRange && !enemy.InMeleeRange) {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null) {
            enemy.ChangeState(new IdleState());
        }

    }

    public void Enter(Enemy enemy) {
        this.enemy = enemy;
    }

    public void Exit() {
    
    }

    public void OnTriggerEnter(Collider2D other) {
        
    }

    private void Attack() {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown) {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack) {
            canAttack = false;
            enemy.mAnimator.SetTrigger("attack");
        }
    }
}
