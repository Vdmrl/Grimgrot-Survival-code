using UnityEngine;

public class MeleeSword : Weapon, IUpgradeable
{
    [SerializeField] private AudioSource attackSound;

    protected override void Attack()
    {       
        anim.SetTrigger("Swing");
        attackSound.Play();
    }

    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: //Swing a sword in front of you
                damagePoint = 15;
                pushForce = 5f;
                cooldown = 2f;
                size = 1f;
                gameObject.SetActive(true);
                description = "Sword-2-upgradeDescr"; // Increase the size by 25% and damage by 5
                break;
            case 2:
                size += 0.25f;
                damagePoint += 5;
                description = "Sword-3-upgradeDescr"; // Decrease the cooldown by 0.25
                break;
            case 3:
                cooldown -= 0.25f;
                description = "Sword-4-upgradeDescr"; // Increase knockback by 5 and damage by 5
                break;
            case 4:
                pushForce += 5f;
                damagePoint += 5;
                description = "Sword-5-upgradeDescr"; // Increase the size by 25% and damage by 5
                break;
            case 5:
                size += 0.25f;
                damagePoint += 5;
                description = "Sword-6-upgradeDescr"; // Decrease the cooldown by 0.25
                break;
            case 6:
                cooldown -= 0.25f;
                description = "Sword-7-upgradeDescr"; // Increase knockback by 5 and damage by 5
                break;
            case 7:
                pushForce += 5f;
                damagePoint += 5;
                description = "Sword-8-upgradeDescr"; // Increase the size by 25% and damage by 5
                break;
            case 8:
                size += 0.25f;
                damagePoint += 5;
                description = "Sword-9-upgradeDescr"; // Decrease the cooldown by 0.25
                break;
            case 9:
                cooldown -= 0.25f;
                description = "Sword-10-upgradeDescr"; // Increase knockback by 5 and damage by 5
                break;
            case 10:
                pushForce += 5f;
                damagePoint += 5;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;

        }
        transform.localScale = new Vector3(size,size,size);
    }
    
    // attack enemies on weapon trigger
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Entity"))
        {
            if (coll.CompareTag("Player"))
            {
                return;
            }

            Damage dmg = new Damage(damagePoint, transform.position, pushForce);
            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
