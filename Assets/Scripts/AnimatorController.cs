using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Transform parentTransform; // where StateMachine lives
    private Animator _animator;
    private StateMachine _stateMachine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = parentTransform.GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        if (_stateMachine != null)
            _stateMachine.OnPlayerStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (_stateMachine != null)
            _stateMachine.OnPlayerStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(StateMachine.PlayerState playerState)
    {
        switch (playerState)
        {
            case StateMachine.PlayerState.Idle:
                _animator.SetBool("Running", false);
                break;

            case StateMachine.PlayerState.Running:
                _animator.SetBool("Running", true);
                break;

            case StateMachine.PlayerState.Jumping:
                break;

            case StateMachine.PlayerState.Attack:
                _animator.Play("Attack4");
                break;
        }
    }

    
}
