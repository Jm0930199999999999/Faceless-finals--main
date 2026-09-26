using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    public PlayerHealth playerHealth;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float Direction = Mathf.Sign(player.position.x - transform.position.x);

        float PlayerDistance = Vector2.Distance(player.position, transform.position);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);

        Vector2 movement = (Vector2)transform.position - lastPosition;
        lastPosition = transform.position;


        if (isGrounded && PlayerDistance < 10f)
        {
            rb.linearVelocity = new Vector2(Direction * followSpeed, rb.linearVelocity.y);

            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(Direction, 0f), 2f, groundLayer);

            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(Direction, 0f, 0f), Vector2.down, 2f, groundLayer);

            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer); 

            if(!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if(isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }

            if (movement.x > 0)
            {
                spriteRenderer.flipX = true; // Moving right
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = false; // Moving left
            }

        }
    }

    private void FixedUpdate()
    {
        if (shouldJump && isGrounded)
        {
            shouldJump = false;
            Vector2 Direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = Direction * jumpForce;
            
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
            
        }
        
        if(playerHealth.health == 0)
        {
            followSpeed = 5f;
            jumpForce = 10f;
        }
        else if(playerHealth.health >= 0)
        {
            followSpeed = 2f;
            jumpForce = 5f;
        }

    }
   
}
