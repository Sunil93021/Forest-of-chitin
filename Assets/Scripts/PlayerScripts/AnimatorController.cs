using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;

    private Animator animator;
    private StateMachine stateMachine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateMachine = playerRoot.GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    
    public void PlayAttackAnimation (){
        animator.SetTrigger("Attack");
    }
    public void PlayRun()
    {
        animator.SetBool("Running", true);

    }
    public void PlayIdle()
    {
        animator.SetBool("Running", false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.TryGetComponent<EnemyHealth>(out EnemyHealth enemy);
        if (enemy != null)
        {
            enemy.TakeDamage(30);
        }
    }
}
