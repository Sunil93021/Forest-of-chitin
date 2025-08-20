using UnityEngine;


[RequireComponent (typeof(PlayerInputReader))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader input;
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.2f;

    [Header("child Reference")]
    [SerializeField] private Transform playerFeet;
    [SerializeField] private Transform childTransform;
    private SpriteRenderer childSpriteRenderer;

    private bool isGrounded;
    private bool jumpRequested = false;
    public bool IsFacingRight { get; private set; } = true;

    private StateMachine stateMachine;

 
    private void Awake()
    {
        input = GetComponent<PlayerInputReader>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
        childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //jump
        if (input.JumpPressed && isGrounded)
        {
            jumpRequested = true;
            stateMachine.ChangeState(StateMachine.PlayerState.Jumping);
        }

        //Attack 
        if(input.AttackPressed)
        {
            stateMachine.ChangeState(StateMachine.PlayerState.Attack);

        }
    }
    private void FixedUpdate()
    {
        //Horizontal movement
        HorizontalMovement();
        CheckJumpRequest();

    }

    private void CheckJumpRequest()
    {
        //CheckIsGrounded
        isGrounded = Physics2D.OverlapCircle(playerFeet.position, groundRadius, groundLayer);
        if (jumpRequested)
        {
            //rb.AddForce(Vector2.up  * jumpForce , ForceMode2D.Impulse);
            rb.linearVelocityY = jumpForce;
            jumpRequested = false;
        }
    }

    private void HorizontalMovement()
    {
        Vector2 move = new Vector2(input.MoveInput.x * moveSpeed * Time.fixedDeltaTime, rb.linearVelocityY);
        rb.linearVelocity = move;
        HandleFacingDirection(move.x);
        UpdateMoveState();
        
    }
    private void HandleFacingDirection(float moveX)
    {
        if (moveX > 0 && !IsFacingRight)
            Flip();
        else if (moveX < 0 && IsFacingRight)
            Flip();
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        childSpriteRenderer.flipX = !childSpriteRenderer.flipX;
    }
    private void UpdateMoveState()
    {
        if (input.MoveInput != Vector2.zero)
        {
            if (stateMachine.GetCurrentPlayerState() != StateMachine.PlayerState.Jumping ||
                stateMachine.GetCurrentPlayerState() != StateMachine.PlayerState.Attack)
            {

                stateMachine.ChangeState(StateMachine.PlayerState.Running);
            }
        }
        else
        {

            if (stateMachine.GetCurrentPlayerState() != StateMachine.PlayerState.Attack ||
                stateMachine.GetCurrentPlayerState() != StateMachine.PlayerState.Jumping)
            {
                stateMachine.ChangeState(StateMachine.PlayerState.Idle);

            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerFeet.position, groundRadius);
    }

}
