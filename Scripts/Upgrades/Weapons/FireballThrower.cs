using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballThrower : WeaponThrower, IUpgradeable
{
    [SerializeField] private Fireball projectile;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionKnockBack;

    [SerializeField] private AudioSource attackSound;

    protected override void Attack()
    {
        attackSound.Play();
        Vector3? pos = ClosestEnemyPosition();
        if (pos != null)
        {
            // Spawn projectile
            GameObject proj = Instantiate(projectile.gameObject, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            // Find random target and send target to the projectile
        
            proj.SendMessage("SetDirection", pos); // random to closest
        }
    }
    
    // Update projectile parameters when game starting and when weapon level increase
    public override void UpdateParameters() 
    {
        projectile.damagePoint = damagePoint;
        projectile.pushForce = pushForce;
        projectile.size = size;
        projectile.speed = speed;
        projectile.explosionRadius = explosionRadius;
        projectile.explosionKnockBack = explosionKnockBack;
    }
    
    // Levels
    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: // Launch a fireball that explodes on impact. Deals area damage
                damagePoint = 20;
                pushForce = 3f;
                cooldown = 5f;
                explosionRadius = 1f;
                size = 1f;
                speed = 3;
                gameObject.SetActive(true);
                description = "Fireball-2-upgradeDescr"; // + 10 damage.
                break;
            case 2:
                damagePoint += 10;
                description = "Fireball-3-upgradeDescr"; // + 10 damage. increases explosion radius by 50%
                break;
            case 3:
                damagePoint += 10;
                explosionRadius += 0.5f;
                description = "Fireball-4-upgradeDescr"; // reduce cooldown by 0.5 seconds
                break;
            case 4:
                cooldown -= 0.5f;
                description = "Fireball-5-upgradeDescr"; // + 10 damage. + 7 push force
                break;
            case 5:
                damagePoint += 10;
                pushForce += 7;
                description = "Fireball-6-upgradeDescr"; // + 10 damage. increases explosion radius by 50%
                break;
            case 6:
                damagePoint += 10;
                explosionRadius += 0.5f;
                description = "Fireball-7-upgradeDescr"; // + 10 damage
                break;
            case 7:
                damagePoint += 10;
                description = "Fireball-8-upgradeDescr"; // + 10 damage. increases explosion radiusы by 0.5
                break;
            case 8:
                damagePoint += 10;
                explosionRadius += 0.5f;
                description = "Fireball-9-upgradeDescr"; // reduce cooldown by 0.5 second
                break;
            case 9:
                cooldown -= 0.5f;
                description = "Fireball-10-upgradeDescr"; // + 10 damage. increases explosion radius by 50%
                break;
            case 10:
                damagePoint += 10;
                explosionRadius += 0.5f;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
        }
        UpdateParameters();
    }
}
