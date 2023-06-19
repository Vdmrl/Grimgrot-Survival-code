using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon, IUpgradeable
{
    private Transform projectilesFolder;
    
    [SerializeField] private float speed = 1;
    [SerializeField] private int pierce = 1;
    [SerializeField] private Arrow projectile;
    private int attackPattern;

    [SerializeField] private AudioSource attackSound;

    protected override void Start()
    {
        base.Start();
        projectilesFolder = GameManager.instance.projectilesFolder;
    }
    protected override void Attack()
    {
        attackSound.Play();
        Arrow proj;
        if (attackPattern >= 1)
        {
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x,transform.position.y+10000000000, transform.position.z));
        }
        if (attackPattern >= 2)
        {
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x,transform.position.y-10000000000, transform.position.z));
        }
        if (attackPattern >= 3)
        {
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y, transform.position.z));
            
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y, transform.position.z));
        }
        if (attackPattern >= 4 && attackPattern <= 5)
        {
            if (attackPattern >= 4)
            {
                proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
                proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y+10000000000, transform.position.z));
            
                proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
                proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y+10000000000, transform.position.z));
            }
            if (attackPattern >= 5)
            {
                proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
                proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y-10000000000, transform.position.z));
            
                proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
                proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y-10000000000, transform.position.z));
            }
        }
        else if (attackPattern >= 6)
        {
            // up right
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y+(Mathf.Sqrt(3)*10000000000), transform.position.z));
            
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y+(Mathf.Sqrt(3)/3 *10000000000), transform.position.z));
            
            // up left
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y-(Mathf.Sqrt(3)*10000000000), transform.position.z));
            
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x+10000000000,transform.position.y-(Mathf.Sqrt(3)/3 *10000000000), transform.position.z));
            
            // down right
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y+(Mathf.Sqrt(3)*10000000000), transform.position.z));
            
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y+(Mathf.Sqrt(3)/3 *10000000000), transform.position.z));
            
            // down left
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y-(Mathf.Sqrt(3)*10000000000), transform.position.z));
            
            proj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
            proj.SendMessage("SetDirection", new Vector3(transform.position.x-10000000000,transform.position.y-(Mathf.Sqrt(3)/3 *10000000000), transform.position.z));
        }
        
    }
    
    public void UpdateParameters()
    {
        //arrow = projectile.GetComponent<Arrow>();
        projectile.damagePoint = damagePoint;
        projectile.pushForce = pushForce;
        projectile.size = size;
        projectile.speed = speed;
        projectile.pierce = pierce;
    }

    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: // Shoots an arrow up.
                damagePoint = 15;
                pushForce = 3f;
                cooldown = 2f;
                size = 0.8f;
                speed = 5;
                pierce = 2;
                attackPattern = 1;
                gameObject.SetActive(true);
                description = "Bow-2-upgradeDescr";//Also shoots an arrow down
                break;
            case 2:
                attackPattern = 2;
                description = "Bow-3-upgradeDescr";//Increase damage by 10. pierce one enemy
                break;
            case 3:
                damagePoint += 10;
                pierce += 1;
                description = "Bow-4-upgradeDescr"; // Also shoots an arrow to the left and to the right.
                break;
            case 4:
                attackPattern = 3;
                description = "Bow-5-upgradeDescr"; // increase damage by 10. pierce two enemy
                break;
            case 5:
                damagePoint += 10;
                pierce += 1;
                description = "Bow-6-upgradeDescr"; // Also shoot arrows to the top corners
                break;
            case 6:
                attackPattern = 4;
                description = "Bow-7-upgradeDescr"; // increase damage by 15. pierce three enemy
                break;
            case 7:
                damagePoint += 15;
                description = "Bow-8-upgradeDescr"; // Also shoot arrows to the down corners
                break;
            case 8:
                attackPattern = 5;
                description = "Bow-9-upgradeDescr"; // increase damage by 10. pierce all enemies
                break;
            case 9:
                pierce += 99;
                damagePoint += 10;
                description = "Bow-10-upgradeDescr"; // shoot more arrows to the  corners
                break;
            case 10:
                description = "max";
                attackPattern = 6;
                GameManager.instance.abilities.Remove(this);
                break;

        }

        UpdateParameters();
    }
}
