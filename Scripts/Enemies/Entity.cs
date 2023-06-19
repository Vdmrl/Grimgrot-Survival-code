using System;
using System.Collections;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    public float maxHitPoint = 1;
    [SerializeField] protected float pushRecoverySpeed = 1f;
    [SerializeField] protected float hitPoint = 1;
    protected float timeBetweenColours = 0.1f;
    protected Vector3 pushDirection;
    protected SpriteRenderer spriteRenderer;
    
    protected bool isFreezed = false;
    [NonSerialized] public bool isRaged = false;
    
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void ReceiveDamage(Damage dmg) // Message
    {
        
        pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
        if (dmg.damageeAmount > 0)
        {
            TakeDamage(dmg);
            StartCoroutine(Blink());
            GameManager.instance.ShowText(dmg.damageeAmount.ToString(), 40, Color.red, transform.position, Vector3.up * 50, 1f);
        }
        
        if (hitPoint <= 0)
        {
            hitPoint = 0;
            Death();
        }
    }

    protected virtual void Death()
    {
        Debug.Log("Death");
    }

    IEnumerator Blink()
    {
        if (!isFreezed || !isRaged)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(timeBetweenColours);
            spriteRenderer.color = Color.white;
        }
        else if (isFreezed)
        {
            spriteRenderer.color = new Color32(25, 25, 112, 255);
            yield return new WaitForSeconds(timeBetweenColours);
            if (isFreezed)
            {
                spriteRenderer.color = new Color32(0, 128, 128, 255);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        else
        {
            spriteRenderer.color = new Color32(178, 34, 34, 255);
            yield return new WaitForSeconds(timeBetweenColours);
            if (isRaged)
            {
                spriteRenderer.color = new Color32(255, 99, 71, 255);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    protected virtual void TakeDamage(Damage dmg)
    {
        hitPoint -= dmg.damageeAmount;
    }
    

    
}
