using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    public float explosionRadius = 1;
    public float explosionKnockBack = 1;
    private Transform effectsFolder;

    protected override void Start()
    {
        base.Start();
        effectsFolder = GameManager.instance.effectsFolder;
    }

    public void SetDirection(Vector3 direction) // Message
    {
        transform.up = direction - transform.position; 
        transform.Rotate(0,0,90);
    }
    
    protected override void Move()
    {
        base.Move();
        // move forward
        gameObject.transform.Translate(Vector3.right * (Time.deltaTime * speed));
    }
    
    protected override void TouchEnemy(Collider2D coll)
    {
        GameObject expl = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.Euler(0, 0, 0), effectsFolder);
        expl.transform.localScale *= explosionRadius*2;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Entity"))
            {
                float magnitude = (col.transform.position - transform.position).magnitude;
                if (magnitude > 0)
                {
                    int explosionDamage;
                    if (magnitude > 1)
                    {
                        explosionDamage = Mathf.RoundToInt(damagePoint / magnitude);
                    }
                    else
                    {
                        explosionDamage = damagePoint;
                    }
                    if (explosionDamage > 0)
                    {
                        float explosionForce;
                        if (magnitude > 1)
                        {
                            explosionForce = explosionKnockBack / magnitude;
                        }
                        else
                        {
                            explosionForce = explosionKnockBack;
                        }
                        Damage dmg = new Damage(explosionDamage, transform.position, explosionForce);
                        col.SendMessage("ReceiveDamage", dmg);
                    }
                    
                }
            }
        }
        Destroy(gameObject);
    }
}
