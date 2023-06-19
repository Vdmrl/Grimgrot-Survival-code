using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponThrower : Weapon
{
    [SerializeField] protected float speed = 1;
    protected Transform projectilesFolder;
    private CircleCollider2D col;
    private List<Collider2D> hits = new List<Collider2D>();
    public ContactFilter2D filter;

    protected override void Start()
    {
        base.Start();
        UpdateParameters();
        col = GetComponent<CircleCollider2D>();
        projectilesFolder = GameManager.instance.projectilesFolder;
    }
    
    // Update projectile parameters when game starting and when weapon level increase
    public virtual void UpdateParameters() 
    {
        Debug.Log("Updating");
    }
    
    protected Vector3? RandomEnemyPosition()
    {
        col.enabled = true;
        col.OverlapCollider(filter, hits);
        col.enabled = false;
        Vector3 currentPosition = transform.position;
        Vector3 enemyPosition;
        if (hits.Count != 0)
        {
            enemyPosition = hits[Random.Range(0, hits.Count)].transform.position;
        }
        else
        {
            return null;
            // random if no enemies
            //enemyPosition = new Vector3(currentPosition.x + Random.Range(-1f,1f),
            //    currentPosition.y + Random.Range(-1f, 1f), 0);
        }
         
        return enemyPosition;
    }
    
    protected Vector3? ClosestEnemyPosition()
    {
        col.enabled = true;
        col.OverlapCollider(filter, hits);
        col.enabled = false;
        Vector3? closestEnemyPosition = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Entity"))
            {
                Vector3 directionToTarget = col.transform.position - currentPosition;
                float distanceSqrToTarget = directionToTarget.sqrMagnitude;
                if (distanceSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToTarget;
                    closestEnemyPosition = col.transform.position;
                }
            }
        }

        return closestEnemyPosition;



        // random if no enemies
        /*if (closestEnemyPosition == new Vector3(1234567,7654321,-100))
        {
            closestEnemyPosition = new Vector3(currentPosition.x + Random.Range(-1f,1f),
                currentPosition.y + Random.Range(-1f, 1f), 0);
        }
        return closestEnemyPosition;*/
    }
}
