using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] protected int level = 0;
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] protected string description = "no description";
    public int Level { get=>level; set=>level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }
    private int interval;
    private float incomeVal;
    
    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 1: // Character gets 0.5 experience per second
                interval = 1;
                incomeVal = 0.5f;
                StartCoroutine("Income");
                description = "ExperienceRune-2-upgradeDescr";
                break;
            case 2:
                incomeVal = 1f;
                description = "ExperienceRune-3-upgradeDescr";
                break;
            case 3:
                incomeVal = 2f;
                description = "ExperienceRune-4-upgradeDescr";
                break;
            case 4:
                incomeVal = 3.5f;
                description = "ExperienceRune-5-upgradeDescr";
                break;
            case 5:
                incomeVal = 5f;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
        }
    }
    
    IEnumerator Income()
    {
        do
        {
            yield return new WaitForSeconds(interval);
            GameManager.instance.AddExperience(incomeVal*GameModifier.experiencePercentageMod);  
        } while (true);
    }

    
}
