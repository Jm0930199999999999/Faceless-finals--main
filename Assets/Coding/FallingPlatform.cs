using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float FallWait = 2f;
    public float RespawnWait = 5f;

    bool isFalling;
    Rigidbody2D rb;
    Vector2 DefPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DefPos = transform.position;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(FallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(RespawnWait);
        Respawn();
    }

    private void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = DefPos;
        isFalling = false;
    }
}
