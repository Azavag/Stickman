using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{ 
    None,
    Before,
    Ready,
    After
}

public class EnemyAttackController : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField] AnimationClip attackClip;
    Animator animator;
    [SerializeField] AttackState attackState;
    bool isPlayerInAttackZone;
    IDamagable damagablePlayer;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        enemyController = GetComponentInParent<EnemyController>();
    }
    void StartAttackAnimation()
    {
        enemyController.NavmeshAgentState(false);
        animator.SetBool("isAttack", true);
    }
    public void StopAttackAnimation()
    {
        if (!isPlayerInAttackZone)
        {
            animator.SetBool("isAttack", false);
            enemyController.NavmeshAgentState(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            SetPlayerInZone(true);
            damagablePlayer = damagable;
            StartAttackAnimation();
        }
    }

    public void TryAttackPlayer(float damage)
    {
        if(isPlayerInAttackZone)
            damagablePlayer.RecieveDamage(damage);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SetPlayerInZone(false);
    }
    public void SetPlayerInZone(bool state)
    {
        isPlayerInAttackZone = state;
    }
}
