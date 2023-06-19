using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText
{
    public bool isActive;
    public GameObject gameObject;
    public TextMeshProUGUI text;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        isActive = true;
        gameObject.SetActive(isActive);
        lastShown = Time.time;
    }

    public void Hide()
    {
        isActive = false;
        gameObject.SetActive(isActive);
    }

    public void UpdateFloatingText()
    {
        if (!isActive)
        {
            return;
        }

        if (Time.time - lastShown > duration)
        {
            Hide();
        }

        gameObject.transform.position += motion * Time.deltaTime;
    }
}
