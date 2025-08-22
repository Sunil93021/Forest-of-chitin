using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float AttackClipLength = 0.44f;
    private PlayerInputReader input;
    private StateMachine stateMachine;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform visualPlayer;

    [Header("Combo ")]
    [SerializeField] private float ComboActiveTime = 10f;
    [SerializeField] private int maxCombo = 4;
    private int currentCombo = 0;
    private float comboExpireTime = 0f;

    private AnimatorController animatorController;
    private Animator animator;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        input = GetComponent<PlayerInputReader>();
        stateMachine = GetComponent<StateMachine>();
        animatorController = visualPlayer.GetComponent<AnimatorController>();
        animator = visualPlayer.GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentCombo != 0 && Time.time > comboExpireTime)
        {
            currentCombo = 0;
            animator.SetFloat("Combo", 0);
        }

        if (input.AttackPressed && Time.time >= nextAttackTime)
        {
            
            if (currentCombo == 0 || (currentCombo < maxCombo -1 && Time.time < comboExpireTime)) {

                comboExpireTime = Time.time + ComboActiveTime + attackCooldown;
                currentCombo++;
                animator.SetFloat("Combo", currentCombo);

            }
            else
            {
                comboExpireTime = 0f;
                currentCombo = 0;
                animator.SetFloat("Combo", 0);
            }

            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void PerformAttack()
    {
        animatorController.PlayAttackAnimation();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D hit in colliders)
        {
            hit.TryGetComponent<EnemyHealth>(out EnemyHealth enemy);
            if(enemy != null) enemy.TakeDamage(damage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawSphere(attackPoint.position, attackRange);
    }

    
}
