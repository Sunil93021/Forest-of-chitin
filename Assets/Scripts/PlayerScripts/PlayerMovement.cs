using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader input;
    private Rigidbody2D rb;
    private StateMachine stateMachine;
    private SpriteRenderer childSpriteRenderer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private Transform playerFeet;

    [Header("Visual")]
    [SerializeField] private Transform childTransform;

    private bool isGrounded;
    public bool IsFacingRight { get; private set; } = true;

    private void Awake()
    {
        input = GetComponent<PlayerInputReader>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
        childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // jump (one-shot)
        if (input.JumpPressed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
            stateMachine.ChangeState(StateMachine.PlayerState.Jumping);
        }

        // attack handled in PlayerCombat
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    private void HandleMovement()
    {
        Vector2 move = new Vector2(input.MoveInput.x * moveSpeed, rb.linearVelocityY);
        rb.linearVelocity = move;

        HandleFacing(move.x);
        UpdateMoveState();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(playerFeet.position, groundRadius, groundLayer);
    }

    private void HandleFacing(float moveX)
    {
        if (moveX > 0 && !IsFacingRight) Flip();
        else if (moveX < 0 && IsFacingRight) Flip();
    }

    private void Flip()
    {
        if (IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); ;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); ;

        }
        IsFacingRight = !IsFacingRight;
        //childSpriteRenderer.flipX = !childSpriteRenderer.flipX;
    }

    private void UpdateMoveState()
    {
        if (input.MoveInput != Vector2.zero && stateMachine.GetCurrentPlayerState() == StateMachine.PlayerState.Idle)
            stateMachine.ChangeState(StateMachine.PlayerState.Running);

        else if (input.MoveInput == Vector2.zero && stateMachine.GetCurrentPlayerState() == StateMachine.PlayerState.Running)
            stateMachine.ChangeState(StateMachine.PlayerState.Idle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (playerFeet != null)
            Gizmos.DrawWireSphere(playerFeet.position, groundRadius);
    }
}
