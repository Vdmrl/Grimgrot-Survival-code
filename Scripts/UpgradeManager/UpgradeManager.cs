using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class UpgradeManager : MonoBehaviour
{
    public Transform buttonContainer;
    public LocalizeStringEvent levelLocalized;
    private LocalizedString levelString;
    private IntVariable currentLevel = null;

    private void EvaluateLocale()
    {
        levelString = levelLocalized.StringReference;

        if (!levelString.TryGetValue("currentLevel", out var variable))
        {
            currentLevel = new IntVariable();
            levelString.Add("currentLevel", currentLevel);
        }
        else
        {
            currentLevel = variable as IntVariable;
        }
    }

    private void OnEnable()
    {
        EvaluateLocale();
        currentLevel.Value = GameManager.instance.level;

        int abilityNumber = GameManager.instance.abilities.Count;
        List<int> showed = new List<int>(0); // список всех уже добавленных улучшений

        int updateAmount = 3;
        if (GameModifier.forthUpdateChance > 0 &&
            Random.Range(1, 101) <= GameModifier.forthUpdateChance) updateAmount = 4;
        if (updateAmount == 4) Debug.Log("четыре");
        for (int i = 0; i < updateAmount; i++)
        {
            // find individual index
            int index = Random.Range(0, abilityNumber);

            if (showed.Count != 0 && showed.Contains(index)) // проверка всего списка на наличие повторов. ≈сли нет уникальных, то выводитьс€ доплнительное улучшени денег
            {
                for (int j = index+1; j < abilityNumber; j++)
                {
                    if (!showed.Contains(j))
                    {
                        index = j;
                        break;
                    }
                }

                if (showed.Contains(index))
                {
                    for (int j = index-1; j > -1; j--)
                    {
                        if (!showed.Contains(j))
                        {
                            index = j;
                            break;
                        }
                    }
                }

                if (showed.Contains(index))
                {
                    index = -1;
                }
            }
            showed.Add(index);
            
            IUpgradeable upgradeable;
            if (index == -1 || abilityNumber == 0)
            {
                upgradeable = GameManager.instance.extMoneyUpgrade;
            }
            else
            {
                upgradeable = GameManager.instance.abilities[index];
            }
            GameObject button = Instantiate(upgradeable.UpgradePrefab, buttonContainer);
            
            

            UpgradePrefabHelper links = button.GetComponent<UpgradePrefabHelper>(); //упрощает доступ к объектам, без него пол€ пришлось бы искать в иерархии префаба
            links.description.SetEntry(upgradeable.Description);
            if(upgradeable.Level > 0)
            {
                //links.level.color = new Color(60, 60, 60);
                links.level.text = "LVL " + (upgradeable.Level+1);
            }
            else
            {
                //links.level.colorGradient = new VertexGradient(new Color(30, 144, 255), new Color(255, 69, 0), new Color(0, 250, 154), new Color(218, 165, 32));
                links.level.text = "New";
            }

            links.ability = upgradeable;
        }
    }
}
