using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader input;
    private Rigidbody2D rb;
    private StateMachine stateMachine;
    private SpriteRenderer childSpriteRenderer;
    private AnimatorController animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private float jumpDelay = 0.3f;

    [Header("Visual")]
    [SerializeField] private Transform childTransform;


    private bool CanJump;
    public bool IsFacingRight { get; private set; } = true;
    private Coroutine DealJump = null;

    private void Awake()
    {
        input = GetComponent<PlayerInputReader>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
        childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        animator = childTransform.GetComponent<AnimatorController>();
    }

    private void Update()
    {
        // jump (one-shot)
        
        if (input.JumpPressed && CanJump)
        {
            Debug.Log("Jumped");
            rb.linearVelocityY = jumpForce;
            CanJump = false;

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
        float moveX = input.MoveInput.x * moveSpeed;
        rb.linearVelocityX = moveX;

        if (moveX !=0 )
        {
            animator.PlayRun();
        }
        else
        {
            animator.PlayIdle();
        }

            HandleFacing(moveX);
    }

    private void CheckGrounded()
    {
        bool onGround = Physics2D.OverlapCircle(playerFeet.position, groundRadius, groundLayer);
        if(onGround)
        {
            if(rb.linearVelocityY < 0.1f)
                CanJump = true;
            if(DealJump != null)
            {
                StopCoroutine(DealJump);
                DealJump = null;
            }
            
        }
        else
        {
            DealJump = StartCoroutine(JumpDelay());
        }
    }
    IEnumerator JumpDelay()
    {
        yield return new WaitForSecondsRealtime(jumpDelay);
        CanJump = false;
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

   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (playerFeet != null)
            Gizmos.DrawWireSphere(playerFeet.position, groundRadius);
    }
}
