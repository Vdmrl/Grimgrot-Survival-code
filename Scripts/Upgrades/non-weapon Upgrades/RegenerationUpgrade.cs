using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationUpgrade : MonoBehaviour, IUpgradeable
{
    [SerializeField] protected int level = 0;
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] protected string description = "no description";
    public int Level { get=>level; set=>level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }
    
    public void LevelUp()
    {
        Level++;
        GameManager.instance.player.regeneration += 0.1f;
        if (level == 5)
        {
            description = "max";
            GameManager.instance.abilities.Remove(this);
        }
    }
}
