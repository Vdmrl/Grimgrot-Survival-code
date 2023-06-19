using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class UpgradePrefabHelper : MonoBehaviour
{
    public LocalizeStringEvent description;
    public TextMeshProUGUI level;

    public IUpgradeable ability = null;
    public void Click()
    {
        Time.timeScale = 1f;
        GameManager.instance.upgradeMenu.gameObject.SetActive(false);
        foreach (Transform child in gameObject.transform.parent)
        {
            Destroy(child.gameObject);
        }
        GameManager.instance.joystick.gameObject.SetActive(true);
        ability.LevelUp();
        GameManager.instance.AddExperience(0);
        GameManager.instance.gameObject.GetComponent<AudioSource>().Play();
    }
}
