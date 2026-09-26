using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    BoxCollider2D playerCollider;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalmovement;

    [Header("Sprint")]
    public float sprintSpeed = 20f;
    public float sprintDuration = 0.2f;
    public float sprintCooldown = 0.1f;
    bool isSprinting;
    bool canSprint = true;
    TrailRenderer trailRenderer;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    int jumpsremaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    
    //For WallJumps
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(0.5f, 10f);



    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;
    void Start()
    {
        
        playerCollider = GetComponent<BoxCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isSprinting)
        {
            return;
        }

        GroundCheck();
        Gravity();
        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalmovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("Magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalmovement = context.ReadValue<Vector2>().x;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed && canSprint)
        {
            StartCoroutine(SprintCoroutine());
        }
    }

    private IEnumerator SprintCoroutine()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);

        canSprint = false;
        isSprinting = true;
        
        trailRenderer.emitting = true;

        float sprintDirection = isFacingRight ? 1f : -1f; 

        rb.linearVelocity = new Vector2(sprintDirection * sprintSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(sprintDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        isSprinting = false;
        trailRenderer.emitting = false;

        Physics2D.IgnoreLayerCollision(6, 7, false);

        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
    }


    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisablePlayerCollider(0.3f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
    }

    private IEnumerator DisablePlayerCollider(float disabletime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disabletime);
        playerCollider.enabled = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsremaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsremaining--;
                animator.SetTrigger("Jump");
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsremaining--;
                animator.SetTrigger("Jump");
            }
        }

        //For Wall Jumping
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;
            
            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                animator.SetTrigger("Jump");
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }

    }

    private void Flip()
    {
        if(isFacingRight && horizontalmovement < 0 || !isFacingRight && horizontalmovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void GroundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer))
        {
            jumpsremaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer);
    }

    private void WallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalmovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));

        }
        else
        {
            isWallSliding = false;
        }
    } 

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = - transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if(wallJumpTime > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;  
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
