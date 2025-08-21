using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deathDelay = 2f;

    private int currentHealth;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private GroundPatrolEnemy groundPatrolEnemy;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        groundPatrolEnemy = GetComponent<GroundPatrolEnemy>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
        if(groundPatrolEnemy != null)
        {
            groundPatrolEnemy.GotHit();
        }
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);

        if (col != null) col.isTrigger = true;
        if (rb != null) rb.gravityScale = 0;
        rb.linearVelocityX = 0;


        Destroy(gameObject, deathDelay);
    }
}


