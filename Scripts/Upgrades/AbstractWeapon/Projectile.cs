using UnityEngine;
using UnityEngine.UIElements;

public abstract class Projectile : MonoBehaviour
{
    private float maxRange = 50;
    private float range = 0;
    public int damagePoint = 1;
    public float pushForce = 1f;
    public float size = 1f;
    public float speed = 1;
    
    protected virtual void Start()
    {
        gameObject.transform.localScale *= size;
    }
    
    private void Update()
    {
        Move();
    }

    // attack enemies on weapon trigger
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Entity")
        {
            if (coll.CompareTag("Player"))
            {
                return;
            }
            
            TouchEnemy(coll);
        }
    }
    
    protected virtual void Move()
    {
        // destroy if out of max range
        range += (Time.deltaTime * speed);
        if (range >= maxRange)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void TouchEnemy(Collider2D coll)
    {
        Damage dmg = new Damage(damagePoint, transform.position, pushForce);
        if (coll != null) coll.SendMessage("ReceiveDamage", dmg);
    }
}
