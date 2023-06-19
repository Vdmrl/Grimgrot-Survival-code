using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMananger : MonoBehaviour
{
    public static MenuMananger instance;
    
    // data
    public int money;
    public int heroType { get; set; }
    public int mapType { get; set; }

    public UpgradeButton upgradeButton;

    // Attack
    public int[,] AttackUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    // Defence
    public int[,] DefenceUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    // Special
    public int[,] SpecialUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    
    // Settings
    public int musicVolume = 5;
    public int sfxVolume = 5;
    public int language;
    public int joysticType = 1;
    public int damageType = 1;
    
    
    private void Awake()
    {
        instance = this;
        LoadData();
        LoadProgress();
        LoadSettings();
    }
    
    /// <summary>
    /// SceneState:
    /// INT hero // 1 - Knight, 2 - mage, 3 - archer, 4 - summoner
    /// |Update|
    /// 1 2 3
    /// 4 5 6
    /// 7 8 9
    /// </summary>
    public void SaveState()
    {
        string s = "";
        s += heroType + "|"; // Hero
        s += mapType + "|"; // Hero
        // Updates
        foreach (int i in AttackUpdates)
        {
            s += i + "|";
        }
        foreach (int i in DefenceUpdates)
        {
            s += i + "|";
        }
        foreach (int i in SpecialUpdates)
        {
            s += i + "|";
        }
        PlayerPrefs.SetString("StateSave", s);
    }
    
    public void SaveData() // Only money
    {
        PlayerPrefs.SetInt("DataSave", money);
    }
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey("DataSave"))
        {
            return;
        }
        money = PlayerPrefs.GetInt("DataSave");
        
        //string[] data = PlayerPrefs.GetString("GameSave").ToString().Split('|');
        //money = int.Parse(data[1]);
        //currentExperience = int.Parse(data[2]);
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("DataSave", 0);
        money = 0;
    }
    
    // Save global updates
    // Used after every update
    public void SaveProgress()
    {
        string s = "";
        foreach (int i in AttackUpdates)
        {
            s += i + "|";
        }
        foreach (int i in DefenceUpdates)
        {
            s += i + "|";
        }
        foreach (int i in SpecialUpdates)
        {
            s += i + "|";
        }
        PlayerPrefs.SetString("ProgressSave", s);
    }
    
    public void LoadProgress()
    {
        if (!PlayerPrefs.HasKey("ProgressSave"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("ProgressSave").ToString().Split('|');
        for (int i = 0; i < 9; i++)
        {
            AttackUpdates[i / 3, i % 3] = int.Parse(data[i]);
        }
        for (int i = 9; i < 18; i++)
        {
            DefenceUpdates[(i-9) / 3, (i-9) % 3] = int.Parse(data[i]);
        }
        for (int i = 18; i < 27; i++)
        {
            SpecialUpdates[(i-18) / 3, (i-18) % 3] = int.Parse(data[i]);
        }
    }

    public void ResetProgress()
    {
        string s = "";
        foreach (int i in AttackUpdates)
        {
            s += 0 + "|";
        }
        foreach (int i in DefenceUpdates)
        {
            s += 0 + "|";
        }
        foreach (int i in SpecialUpdates)
        {
            s += 0 + "|";
        }
        PlayerPrefs.SetString("ProgressSave", s);
        LoadProgress();
    }
    
    // Save settings
    // Used after game entering and settings change 
    public void SaveSettings()
    {
        string s = "";
        s += musicVolume + "|";
        s += sfxVolume + "|";
        s += language + "|";
        s += joysticType + "|";
        s += damageType + "|";
        PlayerPrefs.SetString("SettingsSave", s);
    }
    
    public void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("SettingsSave"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SettingsSave").ToString().Split('|');
        musicVolume = int.Parse(data[0]);
        sfxVolume = int.Parse(data[1]);
        language = int.Parse(data[2]);
        joysticType = int.Parse(data[3]);
        damageType = int.Parse(data[4]);
    }
    
    
}
