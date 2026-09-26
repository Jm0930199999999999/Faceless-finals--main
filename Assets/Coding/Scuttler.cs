using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scuttler : MonoBehaviour
{
    public float speed = 2f;
    public Transform GroundCheck;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    
    private bool isFacingRight = false;

    public PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * speed, rb.linearVelocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(GroundCheck.position, Vector2.down, 1.2f, groundLayer);

        if (groundInfo.collider == false)
        {
            Flip();
        }

        if (playerHealth.health <= 0)
        {
            speed = 10f;
        }
        else if (playerHealth.health >= 0)
        {
            speed = 2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1f;
        transform.localScale = scaler;

    }

}
