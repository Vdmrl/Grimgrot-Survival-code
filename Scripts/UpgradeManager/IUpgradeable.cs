using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable
{
    public void LevelUp();

    public string Description {  get; set; }

    public int Level { get; set; }

    public GameObject UpgradePrefab { get; set; }
}
