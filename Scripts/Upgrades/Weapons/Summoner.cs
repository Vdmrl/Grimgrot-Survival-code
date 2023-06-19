using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Summoner : WeaponThrower, IUpgradeable
{
    [SerializeField] protected Summon projectile;
    private List<Summon> spawned = new List<Summon>();
    
    // Start is called before the first frame update
    protected override void Start()
    {
        //spawned = 
        base.Start();
        Summon.summoner = this;
        SpawnSummon();
        
        UpgradeSpawned();
    }

    protected override void Update()
    {
        // no upgrade
    }

    // Update projectile parameters when game starting and when weapon level increase
    public override void UpdateParameters() 
    {
        projectile.damagePoint = damagePoint;
        projectile.pushForce = pushForce;
        projectile.speed = speed;
    }

    public void UpgradeSpawned()
    {
        foreach (Summon sum in spawned)
        {
            Debug.Log($"spawned = {spawned.Count}");
            sum.damagePoint = damagePoint;
            sum.pushForce = pushForce;
            sum.transform.localScale = new Vector3(size, size, 1);
            sum.speed = speed;
        }
    }

    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: //  Summons small bat that attack a random enemy. The bat moves with the quarter of player speed
                damagePoint = 1;
                size = 0.15f;
                speed = 0.25f;
                gameObject.SetActive(true);
                // summoned in Start()
                description = "Summon-2-upgradeDescr"; // bat starts fly 2 times faster
                break;
            case 2:
                speed = 0.5f;
                description = "Summon-3-upgradeDescr"; // increase damage by 5 and bat size
                break;
            case 3:
                damagePoint = 5;
                size = 0.25f;
                description = "Summon-4-upgradeDescr"; // summon a second bat
                break;
            case 4:
                SpawnSummon();
                description = "Summon-5-upgradeDescr"; // increase damage by 5 and bat size
                break;
            case 5:
                speed = 0.75f;
                description = "Summon-6-upgradeDescr"; // bat starts fly faster by 50%
                break;
            case 6:
                damagePoint = 15;
                size = 0.4f;
                description = "Summon-7-upgradeDescr"; // summon a third bat
                break;
            case 7:
                SpawnSummon();
                description = "Summon-8-upgradeDescr"; // increase damage by 10 and bat size
                break;
            case 8:
                speed = 1f;
                description = "Summon-9-upgradeDescr"; // bat starts fly with the player speed
                break;
            case 9:
                damagePoint = 30;
                size = 0.6f;
                description = "Summon-10-upgradeDescr"; // summon a fourth bat
                break;
            case 10:
                SpawnSummon();
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
        }
        UpdateParameters();
        UpgradeSpawned();
    }

    public Vector3 NewDirection()
    {
        Vector3? dir = RandomEnemyPosition();
        if (dir != null) return (Vector3)dir;
        else return gameObject.transform.position;
    }

    private void SpawnSummon()
    {
        Vector3? pos = NewDirection();
        // Spawn projectile
        Summon sum = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 0), projectilesFolder);
        // Find random target and send target to the projectile
        sum.SendMessage("SetDirection", pos); // random to closest
        spawned.Add(sum);
    }
}
