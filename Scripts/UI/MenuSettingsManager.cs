using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Components;

public class MenuSettingsManager : MonoBehaviour
{
    public AudioMixer mixer;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI effectText;
    public LocalizeStringEvent damageText;
    public LocalizeStringEvent joystickText;

    private void Start()
    {
        ApplyAll();
    }

    public void ApplyAll()
    {
        UpdateVolume();
        if (MenuMananger.instance.damageType == 1)
        {
            damageText.SetEntry("DamageShown-button");
        }
        else
        {
            damageText.SetEntry("DamageHidden-button");
        }

        if (MenuMananger.instance.joysticType == 0)
        {
            joystickText.SetEntry("JoystickInvisible-button");
        }
        else
        {
            joystickText.SetEntry("JoystickFloating-button");
        }
    }

    private void UpdateVolume()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(MenuMananger.instance.musicVolume * 0.2f + 0.0001f) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(MenuMananger.instance.sfxVolume * 0.2f + 0.0001f) * 20);
        musicText.text = $"{MenuMananger.instance.musicVolume * 20}";
        effectText.text = $"{MenuMananger.instance.sfxVolume * 20}";
        MenuMananger.instance.SaveSettings();
    }

    public void ToggleDamage()
    {
        if (MenuMananger.instance.damageType == 1)
        {
            MenuMananger.instance.damageType = 0;
            damageText.SetEntry("DamageShown-button");
        }
        else
        {
            MenuMananger.instance.damageType = 1;
            damageText.SetEntry("DamageHidden-button");
        }
        MenuMananger.instance.SaveSettings();
    }

    public void Togglejoystick()
    {
        if (MenuMananger.instance.joysticType == 1)
        {
            MenuMananger.instance.joysticType = 0;
            joystickText.SetEntry("JoystickInvisible-button");
        }
        else
        {
            MenuMananger.instance.joysticType = 1;
            joystickText.SetEntry("JoystickFloating-button");
        }
        MenuMananger.instance.SaveSettings();
    }

    public void PlusMusic()
    {
        if (MenuMananger.instance.musicVolume < 5)
        {
            MenuMananger.instance.musicVolume++;
        }
        UpdateVolume();
    }

    public void MinusMusic()
    {
        if (MenuMananger.instance.musicVolume > 0)
        {
            MenuMananger.instance.musicVolume--;
        }
        UpdateVolume();
    }

    public void PlusSFX()
    {
        if (MenuMananger.instance.sfxVolume < 5)
        {
            MenuMananger.instance.sfxVolume++;
        }
        UpdateVolume();
    }

    public void MinusSFX()
    {
        if (MenuMananger.instance.sfxVolume > 0)
        {
            MenuMananger.instance.sfxVolume--;
        }
        UpdateVolume();
    }
}
