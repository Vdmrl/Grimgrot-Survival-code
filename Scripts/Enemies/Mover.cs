using UnityEngine;

public abstract class Mover : Entity
{
    
    //private BoxCollider2D _boxCollider;
    private Vector3 _moveDelta;
    //protected RaycastHit2D hit;
    public float speed;
    private Rigidbody2D rb;
    protected Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitPoint = maxHitPoint;
    }

    protected void UpdateMotion(Vector3 input)
    {
        _moveDelta = input;
        _moveDelta.Normalize();

        _moveDelta = new Vector3(_moveDelta.x * speed, _moveDelta.y * speed, 0);

        if (input.x > 0)
        {
            transform.localScale = Vector3.one;    
        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        _moveDelta += pushDirection;

        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
        rb.velocity = new Vector2(_moveDelta.x, _moveDelta.y);
    }
}
