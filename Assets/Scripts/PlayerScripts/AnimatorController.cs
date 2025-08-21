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
        if (stateMachine != null)
            stateMachine.OnPlayerStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (stateMachine != null)
            stateMachine.OnPlayerStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(StateMachine.PlayerState state)
    {
        switch (state)
        {
            case StateMachine.PlayerState.Idle:
                animator.SetBool("Running", false);
                break;

            case StateMachine.PlayerState.Running:
                animator.SetBool("Running", true);
                break;

            case StateMachine.PlayerState.Attack:
                
                break;
        }
    }

    public void PlayAttackAnimation (){
        animator.SetTrigger("Attack");
    }
}
