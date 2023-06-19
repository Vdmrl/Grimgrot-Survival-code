using System.Collections;
using TMPro;
using UnityEngine;

public class MagicMissiler : WeaponThrower, IUpgradeable
{
    [SerializeField] private int projectilesAmount = 1;
    private float projectileInterval = 0.1f;
    [SerializeField] protected Missile projectile;

    [SerializeField] private AudioSource attackSound;
    public AudioSource hitSound;

    protected override void Attack()
    {
        StartCoroutine(SeveralAttack());
    }

    IEnumerator SeveralAttack()
    {
        for (int i = 0; i < projectilesAmount; i++)
        {
            Vector3? pos = RandomEnemyPosition();
            if (pos != null)
            {
                attackSound.pitch = (Random.Range(0.6f, 1.4f));
                attackSound.Play();
                // Spawn projectile
                GameObject proj = Instantiate(projectile.gameObject, transform.position, Quaternion.Euler(0, 0, 0),
                    projectilesFolder);
                // Find random target and send target to the projectile

                proj.SendMessage("SetDirection", pos); // random to closest
            }
            yield return new WaitForSeconds(projectileInterval);
        }
    }
    
    // Update projectile parameters when game starting and when weapon level increase
    public override void UpdateParameters() 
    {
        projectile.damagePoint = damagePoint;
        projectile.pushForce = pushForce;
        projectile.size = size;
        projectile.speed = speed;
    }
    
    // Levels
    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: // Attack random enemy with magic missile
                damagePoint = 10;
                pushForce = 3f;
                cooldown = 1.5f;
                size = 0.2f;
                speed = 5;
                projectilesAmount = 1;
                projectileInterval = 0.1f;
                gameObject.SetActive(true);
                description = "MagicMissile-2-upgradeDescr";//fire 1 more missile
                break;
            case 2:
                projectilesAmount += 1;
                description = "MagicMissile-3-upgradeDescr";//Fire 1 more missile. Increase damage by 5
                break;
            case 3:
                projectilesAmount += 1;
                damagePoint += 5;
                description = "MagicMissile-4-upgradeDescr";//Fire 1 more missile. Decrease the cooldown by 0.25
                break;
            case 4:
                projectilesAmount += 1;
                cooldown -= 0.25f;
                description = "MagicMissile-5-upgradeDescr";//Fire 1 more missile
                break;
            case 5:
                projectilesAmount += 1;
                description = "MagicMissile-6-upgradeDescr";//Fire 1 more missile. Increase knockback by 7
                break;
            case 6:
                projectilesAmount += 1;
                pushForce += 7;
                description = "MagicMissile-7-upgradeDescr";//Fire 1 more missile. Increase damage by 5
                break;
            case 7:
                projectilesAmount += 1;
                damagePoint += 5;
                description = "MagicMissile-8-upgradeDescr";//Fire 1 more missile. Increase projectile speed by 5
                break;
            case 8:
                projectilesAmount += 1;
                speed += 5;
                description = "MagicMissile-9-upgradeDescr";//Fire 1 more missile. Increase knockback by 5
                break;
            case 9:
                projectilesAmount += 1;
                speed += 5;
                description = "MagicMissile-10-upgradeDescr";//Fire 1 more missile. Increase damage by 10
                break;
            case 10:
                projectilesAmount += 1;
                damagePoint += 10;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;

        }

        UpdateParameters();
    }

}
