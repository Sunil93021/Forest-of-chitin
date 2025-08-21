using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundPatrolEnemy : MonoBehaviour
{
    private const float ATTACK_ANIMATION_LENGTH = 0.84f;

    [Header("Enemy Patrolling Config")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Player info")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Enemy Vision")]
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float runSpeed = 1.5f;

    [Header("Attack settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage = 20;
    [SerializeField, Min(ATTACK_ANIMATION_LENGTH)] private float attackDelay = 0.9f;
    private float attackTimer = 0f;
    

    [Header("Slow Effect (Time) ")]
    [SerializeField] private float effectTimer = 1f;
    [SerializeField] private float timeScale = 0.6f;


    private Rigidbody2D rb;
    private Animator animator;

    private bool movingRight = true;
    private Coroutine AttackTask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public enum State
    {
        Patrolling,
        Run,
        Attack
    }

    private State currentState = State.Patrolling;

    private void Update()
    {
        switch (currentState)
        {

            case State.Patrolling:
                HandlePatrolling();
                break;
            case State.Run:
                HandleRun();
                break;
            case State.Attack:
                HandleAttack();
                break;
        }

    }

    // check player while patrolling
    private void HandlePatrolling()
    {
        rb.linearVelocityX = (movingRight ? 1 : -1) * moveSpeed;
        CheckGroundEnd();

        RaycastHit2D playerHit = Physics2D.Raycast(
            transform.position, transform.right, visionRange, playerLayer
            );
        if (playerHit == true)
        {
            currentState = State.Run;
        }
    }


    // run fast towards player if not in vision again start patrolling
    private void HandleRun()
    {
        rb.linearVelocityX = (movingRight ? 1 : -1) * runSpeed;
        CheckGroundEnd();

        RaycastHit2D playerHit = Physics2D.Raycast(
            transform.position, transform.right, visionRange, playerLayer
            );

        if(playerHit == false || playerHit.distance > visionRange)
        {
            currentState = State.Patrolling; 
        }

        if(playerHit.distance < attackRange)
        {
            currentState = State.Attack;
        }

        
    }

    //attack player if in attack range
    private void HandleAttack()
    {
        RaycastHit2D playerHit = Physics2D.Raycast(
            transform.position, transform.right, visionRange, playerLayer
            );

        if (playerHit == false || playerHit.distance > visionRange)
        {
            currentState = State.Patrolling;
        }
        else if (playerHit.distance > attackRange)
        {
            currentState = State.Run;
        }
        else 
        {
            if(Time.time > attackTimer)
            {
                
                if(AttackTask == null)
                {
                    attackTimer = Time.time + attackDelay;
                    AttackTask = StartCoroutine(AttackCoroutine());
                }
                
            }

        }

    }

    //Animate the attack first after completing animation check if player is in attackRange or not
    IEnumerator AttackCoroutine()
    {
        for(int i = 0;i < 1; i += 1)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSecondsRealtime(ATTACK_ANIMATION_LENGTH);
        }

        RaycastHit2D playerHit = Physics2D.Raycast(
            transform.position, transform.right, visionRange, playerLayer
            );

        if (playerHit)
        {
            playerHit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth);
            if (playerHealth != null)
            {

                attackTimer = Time.time + attackDelay;
                playerHealth.TakeDamage(damage);
            }
        }
        StopCoroutine(AttackTask);
        AttackTask = null ;
    }
    //this check if the enemy is end of the platform if yes it will change direction
    private void CheckGroundEnd()
    {
        // Check ground ahead
        RaycastHit2D groundInfo = Physics2D.Raycast(
            groundCheck.position, -groundCheck.up, checkDistance, groundLayer
            );
        if (groundInfo.collider == null)
            Flip();
    }

    private void Flip()
    {
        if (movingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); ;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); ;

        }
        movingRight = !movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, groundCheck.position  -groundCheck.up * checkDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackRange);

    }

    public void GotHit()
    {
        
        
        RaycastHit2D playerHit = Physics2D.Raycast(
            transform.position, transform.right, visionRange, playerLayer
            );
        if(!playerHit) animator.SetTrigger("Alert");
        StartCoroutine( TimeScalar( effectTimer ,timeScale ));
        if (!playerHit)
        {
            Flip();
        }
    }

    

    IEnumerator TimeScalar(float sec, float timescale)
    {
        Time.timeScale = timescale;
        yield return new WaitForSecondsRealtime(sec);
        Time.timeScale = 1f;
    }
}

