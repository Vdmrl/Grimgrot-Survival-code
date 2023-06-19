using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromasteryUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] protected int level = 0;
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] protected string description = "no description";
    public int Level { get=>level; set=>level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }
    public static int killCount = 0;
    private static int killGoal = 0;

    public static void AddNecromastery(int kills)
    {
        if (killGoal != 0)
        {
            killCount += kills;
            if (killCount >= killGoal)
            {
                GameModifier.damagePercentageMod += 0.01f;
                killCount = killGoal - killCount;
            }
        }
    }
    
    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: // Damage is increased by 1% for every 200 killed enemies
                killGoal = 200;
                description = "Necromastery-2-upgradeDescr";
                break;
            case 2:
                killGoal = 150;
                description = "Necromastery-3-upgradeDescr";
                break;
            case 3:
                killGoal = 100;
                description = "Necromastery-4-upgradeDescr";
                break;
            case 4:
                killGoal = 75;
                description = "Necromastery-5-upgradeDescr";
                break;
            case 5:
                killGoal = 50;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
        }
    }
}
