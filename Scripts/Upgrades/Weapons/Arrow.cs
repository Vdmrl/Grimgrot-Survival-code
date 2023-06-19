using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    public int pierce = 1;
    
    public void SetDirection(Vector3 direction) // Message
    {
        transform.up = direction - transform.position;
    }
    
    protected override void Move()
    {
        base.Move();
        // move forward
        gameObject.transform.Translate(Vector3.up * (Time.deltaTime * speed));
    }
    protected override void TouchEnemy(Collider2D coll)
    {
        base.TouchEnemy(coll);
        pierce--;
        if (pierce <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
