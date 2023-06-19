using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class UpgradeButton : MonoBehaviour
{
    public int upgradeType { get; set; } = 1;
    public int position { get; set; } = -1;
    public int cost { get; set; }

    public LocalizeStringEvent descriptionField;
    public LocalizeStringEvent titleField;
    public GameObject moneyAmount;

    [SerializeField] private TextMeshProUGUI totalMoney;
    [SerializeField] private TextMeshProUGUI upgradeSumText;
    [SerializeField]private int upgradeSum;

    public List<Button> secondRowUpgrades;
    public List<Button> thirdRowUpgrades;

    public PermanentUpgrade upgrade;

    private void OnEnable()
    {
        totalMoney.text = MenuMananger.instance.money.ToString();

        foreach (int i in MenuMananger.instance.AttackUpdates)//сумма всех уровней
        {
            upgradeSum += i;
        }
        foreach (int i in MenuMananger.instance.DefenceUpdates)
        {
            upgradeSum += i;
        }
        foreach (int i in MenuMananger.instance.SpecialUpdates)
        {
            upgradeSum += i;
        }

        upgradeSumText.text = "Lvl " + upgradeSum;

        if (upgradeSum >= 5)
        {
            foreach (Button item in secondRowUpgrades)
            {
                    item.interactable = true;
            }
            if (upgradeSum >= 15)
            {
                foreach (Button item in thirdRowUpgrades)
                {
                        item.interactable = true;
                }
            }
        }
        descriptionField.SetEntry("Default-description");
        titleField.SetEntry("Default-title");
        position = -1;
    }

    public void OnClick()
    {
        Debug.Log($"Type {upgradeType}, pos {position}, cost {cost}, button {upgrade}");
        if (position != -1)
            switch (upgradeType)
            {
                default:
                    break;

                case 1:
                    if (MenuMananger.instance.AttackUpdates[position / 3, position % 3] < upgrade.levelCosts.Count) // прокачка атаки если уровень 2 или меньше
                    {
                        if (cost <= MenuMananger.instance.money)
                        {
                            MenuMananger.instance.money -= cost;
                            MenuMananger.instance.AttackUpdates[position / 3, position % 3]++;
                            upgrade.Upgraded();
                            upgradeSum++;
                            MenuMananger.instance.SaveProgress();
                            MenuMananger.instance.SaveData();
                        }
                    }
                    break;

                case 2:
                    if (MenuMananger.instance.DefenceUpdates[position / 3, position % 3] < upgrade.levelCosts.Count)//защиты
                    {
                        if (cost <= MenuMananger.instance.money)
                        {
                            MenuMananger.instance.money -= cost;
                            MenuMananger.instance.DefenceUpdates[position / 3, position % 3]++;
                            upgrade.Upgraded();
                            upgradeSum++;
                            MenuMananger.instance.SaveProgress();
                            MenuMananger.instance.SaveData();
                        }
                    }
                    break;

                case 3:
                    if (MenuMananger.instance.SpecialUpdates[position / 3, position % 3] < upgrade.levelCosts.Count)//спешл
                    {
                        if (cost <= MenuMananger.instance.money)
                        {
                            MenuMananger.instance.money -= cost;
                            MenuMananger.instance.SpecialUpdates[position / 3, position % 3]++;
                            upgrade.Upgraded();
                            upgradeSum++;
                            MenuMananger.instance.SaveProgress();
                            MenuMananger.instance.SaveData();
                        }
                    }
                    break;
            }

        totalMoney.text = MenuMananger.instance.money.ToString();
        upgrade.OnClick();

        upgradeSumText.text = "Lvl " + upgradeSum;

        if (upgradeSum >= 5)
        {
            foreach ( Button item in secondRowUpgrades ) 
            { 
                item.interactable = true;
            }
            if (upgradeSum >= 15)
            {
                foreach(Button item in thirdRowUpgrades ) 
                {
                        item.interactable = true; 
                }
            }
        }
    }
}
