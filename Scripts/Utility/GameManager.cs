using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        level = 0;
        gemAmount = 0;
        redGem = null;
        LoadData();
        LoadState();
        LoadSettings();
        stopwatch.StartStopwatch();
        expProgressBar.ResetBar(maxExperience);
        abilities = player.GetComponentsInChildren<IUpgradeable>(true).ToList();
        extMoneyUpgrade = GameObject.Find("ExtraMoneyUpgrade").GetComponent<ExtraMoneyUpgrade>();
        ChangeJoystick();
    }

    // Logic
    [Header("Logic")]
    public int maxGemsOnGround;
    [HideInInspector] public int gemAmount;
    
    [HideInInspector]
    public int kills;
    [Header("Links")]
    // Utility links
    public Player player;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    public Stopwatch stopwatch;
    public ExperienceProgressBar expProgressBar;
    public HealthProgressBar healthProgressBar;
    public GameObject upgradeMenu;
    public VariableJoystick joystick;
    public GameObject gameoverScreen;
    public ExperienceAmount redGemPrefab;
    public ExperienceAmount purpleGemPrefab;
    public GameObject explosionEffect;
    public Object rareMark;
    public Object flask;
    public Object box;
    public Object chest;
    [HideInInspector] public ExperienceAmount redGem;
    public ChangeLanguage changeLanguage;
    // Weapon Links
    public MagicMissiler magicMissiler;
    [SerializeField] private FloatingTextManager floatingTextManager;
    [Header("Folders")] // Links
    public Treadmill treadmill;
    public Transform enemyFolder;
    public Transform experienceFolder;
    public Transform effectsFolder;
    public Transform projectilesFolder;
    public Transform propsFolder;
    // Load Values
    public int heroType;
    public int mapType;
    public int musicVolume;
    public int sfxVolume;
    public int language;
    public int joysticType;
    public int damageType;
    // Local update links
    public List<IUpgradeable> abilities;
    public IUpgradeable extMoneyUpgrade;
    
    public int[,] AttackUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    public int[,] DefenceUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    public int[,] SpecialUpdates = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
    
    [Space(10)] [Header("Parameters")] // Parameters 
    private float currentExperience = 0;
    public float maxExperience = 5; // experience to the next level
    public int level;
    
    public int money;
    public float currentTime = 0f;

    public void ShowText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(message, fontSize, color,  position, motion, duration);
    }

    
    
    // Experience Manager
    public void AddExperience(float exp)
    {
        exp *= GameModifier.experiencePercentageMod;
        currentExperience += exp;
        expProgressBar.AddValue(exp);
        if (currentExperience >= maxExperience)
        {
            currentExperience -= maxExperience;
            level += 1;

            Time.timeScale = 0f;
            upgradeMenu.SetActive(true);// Повышение уровня(вызов меню)
            joystick.gameObject.SetActive(false);

            maxExperience += 8 + (level / 20) * 2; // previous:  maxExperience += 10 + (level / 20) * 3;
            expProgressBar.ResetBar(maxExperience);
            expProgressBar.AddValue(currentExperience);
        }
    }

    // Gold Manager
    public void AddGold(int gold)
    {
        money += gold;
        // вывод денег на экран или писать в паузе
        SaveData();
    }


    /// INT preferedSkin
    /// INT Money
    /// INT experience
    /// INT weaponLevel
    /// <summary>
    /// DataSave:
    /// INT money
    /// SceneState:
    /// INT hero // 1 - Knight, 2 - mage, 3 - archer, 4 - summoner
    /// Улучшения
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("DataSave", money);
        Debug.Log("Saving");
    }
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey("DataSave"))
        {
            return;
        }

        //string[] data = PlayerPrefs.GetString("GameSave").ToString().Split('|');
        //money = int.Parse(data[1]);
        //currentExperience = int.Parse(data[2]);
        money = PlayerPrefs.GetInt("DataSave");
        Debug.Log("Loading");
    }
    
    public void LoadState()
    {
        if (!PlayerPrefs.HasKey("StateSave"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("StateSave").ToString().Split('|');
        heroType = int.Parse(data[0]);
        ChoosePlayer();
        mapType = int.Parse(data[1]);
        for (int i = 2; i < 11; i++)
        {
            AttackUpdates[(i-2) / 3, (i-2) % 3] = int.Parse(data[i]);
        }
        for (int i = 11; i < 20; i++)
        {
            DefenceUpdates[(i-11) / 3, (i-11) % 3] = int.Parse(data[i]);
        }
        for (int i = 20; i < 29; i++)
        {
            SpecialUpdates[(i-20) / 3, (i-20) % 3] = int.Parse(data[i]);
        }
        money = PlayerPrefs.GetInt("DataSave");
        Debug.Log("Loading");
    }

    private void ChoosePlayer()
    {
        switch (heroType)
        {
            case 1: // knight
                player = player1;
                Destroy(player2);
                Destroy(player3);
                Destroy(player4);
                player.GetComponentInChildren<MeleeSword>(true).LevelUp();
                player.gameObject.SetActive(true);
                player.GetComponentInChildren<ShieldUpgrade>(true).LevelUp();
                break;
            case 2: // mage
                Destroy(player1);
                player = player2;
                Destroy(player3);
                Destroy(player4);
                player.GetComponentInChildren<MagicMissiler>(true).LevelUp();
                player.gameObject.SetActive(true);
                player.GetComponentInChildren<MagicShieldUpgrade>(true).LevelUp();
                break;
            case 3: // archer
                Destroy(player1);
                Destroy(player2);
                player = player3;
                Destroy(player4);
                player.GetComponentInChildren<Bow>(true).LevelUp();
                player.gameObject.SetActive(true);
                player.GetComponentInChildren<AttackSpeedUpgrade>(true).LevelUp();
                break;
            case 4: // summoner
                Destroy(player1);
                Destroy(player2);
                Destroy(player3);
                player = player4;
                player.GetComponentInChildren<Summoner>(true).LevelUp();
                player.gameObject.SetActive(true);
                player.GetComponentInChildren<IncomeUpgrade>().LevelUp();
                break;
        }
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
    
    public void ChangeJoystick()
    {
        if (joysticType == 0)
        {
            joystick.SetMode(JoystickType.Dynamic);
            joystick.transform.GetChild(0).GetComponent<Image>().enabled = false;
            joystick.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
        }
        else
        {
            joystick.SetMode(JoystickType.Floating);
            joystick.transform.GetChild(0).GetComponent<Image>().enabled = true;
            joystick.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    public void EndGame(bool isWin)
    {
        GameoverAnimate animator = gameoverScreen.GetComponent<GameoverAnimate>();
        stopwatch.StopStopwatch();
        joystick.gameObject.SetActive(false);
        Time.timeScale = 0;
        if (isWin)
        {
            animator.title.SetEntry("Victory-title");
        }
        else
        {
            animator.title.SetEntry("Dead-title");
        }

        animator.EvaluateLocale();
        int gainedMoney = (int)((stopwatch.time.Seconds + kills + level * 100) * GameModifier.goldPercentageMod);
        animator.timeValue.Value = stopwatch.timeString;
        animator.killsValue.Value = kills;
        animator.levelValue.Value = level;
        animator.goldValue.Value = gainedMoney;

        money += gainedMoney;
        SaveData();
        gameoverScreen.SetActive(true);
    }
}
