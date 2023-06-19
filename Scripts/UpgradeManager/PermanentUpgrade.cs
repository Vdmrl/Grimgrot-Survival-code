using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgrade : MonoBehaviour
{
    private static UpgradeButton upgradeButton;
    private TextMeshProUGUI currentLevel;
    public int level;

    [Header("Upgrade info")]
    [SerializeField] private int position;
    [SerializeField] public List<int> levelCosts = new List<int> { 100, 500, 1000 }; //������ ��� ������� ������
    [SerializeField] private string title;
    [SerializeField] private string description;

    private void Awake()
    {
        upgradeButton = MenuMananger.instance.upgradeButton;
        currentLevel = gameObject.GetComponentInChildren<TextMeshProUGUI>(); //����� ������ "0/3"
        if (transform.parent.parent.name == "AttackUpgrades") //�� �������� ��� ������� �����
        {
            level = MenuMananger.instance.AttackUpdates[position / 3, position % 3];
        }
        if (transform.parent.parent.name == "DefenceUpgrades")
        {
            level = MenuMananger.instance.DefenceUpdates[position / 3, position % 3];
        }
        if (transform.parent.parent.name == "SpecialUpgrades")
        {
            level = MenuMananger.instance.SpecialUpdates[position / 3, position % 3];
        }

        if (level < levelCosts.Count)
        {
            currentLevel.text = level + "/" + levelCosts.Count;
        }
        else currentLevel.text = "Max";
    }
    public void OnClick() //��� ������ ��������� �������� ������ � UpgradeButton
    {
        Debug.Log("level = " + level + " button " + this);
        if (level < levelCosts.Count) 
        {
            upgradeButton.moneyAmount.SetActive(true);
            upgradeButton.moneyAmount.GetComponent<TextMeshProUGUI>().text = levelCosts[level].ToString();
            upgradeButton.cost = levelCosts[level];

            if (MenuMananger.instance.money >= levelCosts[level]) //������ ������ ���� ��� �����
                upgradeButton.GetComponent<Button>().interactable = true;
            else
                upgradeButton.GetComponent<Button>().interactable = false;
        }
        else //����� ��������� ���������, ��������� ��� �������������� � �������� �� ����
        {
            upgradeButton.moneyAmount.SetActive(false);
            upgradeButton.GetComponent<Button>().interactable = false;
        }

        upgradeButton.descriptionField.SetEntry(description);
        upgradeButton.titleField.SetEntry(title);
        upgradeButton.position = position;
        upgradeButton.upgrade = this;

    }

    public void Upgraded()
    {
        level++;
        if (level < levelCosts.Count)
        {
            currentLevel.text = level + "/" + levelCosts.Count;
        }
        else currentLevel.text = "Max";
    }
}
