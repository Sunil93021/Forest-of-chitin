using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInputReader input;
    private StateMachine stateMachine;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform visualPlayer;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        input = GetComponent<PlayerInputReader>();
        stateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if (input.AttackPressed && Time.time >= nextAttackTime)
        {
            PerformAttack();
            stateMachine.ChangeState(StateMachine.PlayerState.Attack);
            
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void PerformAttack()
    {
        visualPlayer.GetComponent<AnimatorController>().PlayAttackAnimation();
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPoint.position, attackPoint.right, attackRange, enemyLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<EnemyHealth>(out var enemyHealth))
                enemyHealth.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.right * attackRange);
    }
}
