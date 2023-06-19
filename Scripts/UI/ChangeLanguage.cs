using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLanguage : MonoBehaviour
{
    private bool active = false;
    public int id;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            return;
        }

        id = PlayerPrefs.GetInt("Language");
        if (active == true)
            return;
        StartCoroutine(SetLanguage(id));
    }

    public void ToggleLanguage()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            id = 1;
        }
        else
        {
            id = 0;
        }

        if (active == true)
            return;
        StartCoroutine(SetLanguage(id));
        Debug.Log("попытка смены языка");
    }

    IEnumerator SetLanguage(int id)
    {
        Debug.Log("смена языка");
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
        PlayerPrefs.SetInt("Language", id);
        active = false;
    }
}
