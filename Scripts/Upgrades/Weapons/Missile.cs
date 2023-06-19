using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    
    public void SetDirection(Vector3 direction) // Message
    {
        transform.up = direction - transform.position;
        transform.Rotate(0,0,-90);
    }
    
    protected override void Move()
    {
        base.Move();
        // move forward
        gameObject.transform.Translate(Vector3.left * (Time.deltaTime * speed));
    }
    
    protected override void TouchEnemy(Collider2D coll)
    {
        base.TouchEnemy(coll);
        GameManager.instance.magicMissiler.hitSound.Play();
        Destroy(gameObject);
    }
}
