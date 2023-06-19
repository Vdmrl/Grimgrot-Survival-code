using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMoneyUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] private GameObject upgradePrefab;
    private void Start()
    {
        Description = "ExtraMoney-upgradeDescr";
        Level = 0;
    }

    public void LevelUp()
    {
        GameManager.instance.money += 100;
    }

    public string Description { get; set; }
    public int Level { get; set; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }
}
