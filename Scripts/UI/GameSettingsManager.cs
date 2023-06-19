using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Components;

public class GameSettingsManager : MonoBehaviour
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
        if (GameManager.instance.damageType == 1)
        {
            damageText.SetEntry("DamageShown-button");
        }
        else
        {
            damageText.SetEntry("DamageHidden-button");
        }

        if (GameManager.instance.joysticType == 0) 
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
        mixer.SetFloat("MusicVolume", Mathf.Log10(GameManager.instance.musicVolume * 0.2f + 0.0001f) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(GameManager.instance.sfxVolume * 0.2f + 0.0001f) * 20);
        musicText.text = $"{GameManager.instance.musicVolume * 20}";
        effectText.text = $"{GameManager.instance.sfxVolume * 20}";
        GameManager.instance.SaveSettings();
    }

    public void ToggleDamage()
    {
        if (GameManager.instance.damageType == 1)
        {
            GameManager.instance.damageType = 0;
            damageText.SetEntry("DamageHidden-button");
        }
        else
        {
            GameManager.instance.damageType = 1;
            damageText.SetEntry("DamageShown-button");
        }
        GameManager.instance.SaveSettings();
    }

    public void Togglejoystick()
    {
        if (GameManager.instance.joysticType == 1)
        {
            GameManager.instance.joysticType = 0;
            joystickText.SetEntry("JoystickInvisible-button");
        }
        else
        {
            GameManager.instance.joysticType = 1;
            joystickText.SetEntry("JoystickFloating-button");
        }
        GameManager.instance.SaveSettings();
    }

    public void PlusMusic()
    {
        if (GameManager.instance.musicVolume < 5)
        {
            GameManager.instance.musicVolume++;
        }
        UpdateVolume();
    }

    public void MinusMusic()
    {
        if (GameManager.instance.musicVolume > 0)
        {
            GameManager.instance.musicVolume--;
        }
        UpdateVolume();
    }

    public void PlusSFX()
    {
        if (GameManager.instance.sfxVolume < 5)
        {
            GameManager.instance.sfxVolume++;
        }
        UpdateVolume();
    }

    public void MinusSFX()
    {
        if (GameManager.instance.sfxVolume > 0)
        {
            GameManager.instance.sfxVolume--;
        }
        UpdateVolume();
    }
}
