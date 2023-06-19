

using UnityEngine;

public class EnemyChaser : Enemy
{
    private void FixedUpdate()
    {
        if (!isScared) UpdateMotion((playerTransform.position - transform.position));
        else UpdateMotion((transform.position - playerTransform.position));
    }
}
