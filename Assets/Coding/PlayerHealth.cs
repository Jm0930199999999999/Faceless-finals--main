using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerHeal;

    public float health;
    public float maxHealth = 15f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        health = maxHealth;
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        HealthItem.OnHealthCollect += Heal;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Trap trap = collision.GetComponent<Trap>();
        if (trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
        }
        else if(trap)
        {
            SoundEffectManager.Play("Bounce");
        }
        
    }

    void Heal(int hp)
    {
        health += hp;
        OnPlayerHeal?.Invoke();
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        SoundEffectManager.Play("Hurt");
        StartCoroutine(FlashRed());

        OnPlayerDamaged.Invoke();
        if (health <= 0)
        {
            OnPlayerDeath.Invoke();
        }
        

    }

    private IEnumerator FlashRed()
    {

        spriteRenderer.color = Color.red;
       
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        
    }

}
