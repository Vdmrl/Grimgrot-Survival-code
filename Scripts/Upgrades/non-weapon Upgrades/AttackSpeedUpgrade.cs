using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] protected int level = 0;
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] protected string description = "no description";
    public int Level { get=>level; set=>level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }
    private int rageInterval = 0;
    private int rageTime = 0;
    private float rageBust = 0; //в процентах, увеличение скорости стрельбы (записал дл€ себ€, название необъ€снимое)
    private bool isCheckInvulnerable = false;
    
    public void LevelUp() 
    {
        Level++;
        switch (level)
        {
            case 1: // every 20 seconds character gets enraged for 2 seconds. During rage firerate is increased by 50%
                rageInterval = 20;
                rageTime = 2;
                rageBust = 0.5f;
                StartCoroutine("Rage");
                description = "Enrage-2-upgradeDescr"; // Character gets enraged every 15 seconds
                break;
            case 2:
                rageInterval = 15;
                description = "Enrage-3-upgradeDescr"; // During rage firerate is increased by 100%
                break;
            case 3:
                rageBust += 0.5f;
                description = "Enrage-4-upgradeDescr"; // Rage lasts 1 second longer
                break;
            case 4: 
                rageTime += 1;
                description = "Enrage-5-upgradeDescr"; // Character gets enraged every 10 seconds
                break;
            case 5:
                rageInterval = 10;
                description = "Enrage-6-upgradeDescr"; // During rage firerate is increased by 150%
                break;
            case 6:
                rageBust += 0.5f;
                description = "Enrage-7-upgradeDescr"; // The character is invulnerable during rage
                break;
            case 8:
                description = "Enrage-8-upgradeDescr"; // During rage, the character becomes angrier
                break;
            case 7:
                isCheckInvulnerable = true;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
        }
    }

    IEnumerator Rage()
    {
        do
        {
            GameManager.instance.player.isRaged = true;
            yield return new WaitForSeconds(rageInterval-rageTime);
            float tempRage = rageBust;
            GameManager.instance.player.GetComponent<SpriteRenderer>().color = new Color32(255, 99, 71, 255);
            Debug.Log("rage");
            GameModifier.attackSpeedPercentageMod += tempRage;
            if (isCheckInvulnerable) GameManager.instance.player.GetInvulnerable(rageTime);
            yield return new WaitForSeconds(rageTime);
            GameManager.instance.player.isRaged = false;
            GameModifier.attackSpeedPercentageMod -= tempRage;
            GameManager.instance.player.GetComponent<SpriteRenderer>().color = Color.white;
        } while (true);
    }
        
}
