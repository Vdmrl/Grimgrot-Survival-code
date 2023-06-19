using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] private GameObject textContainer;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private float randomPosition;
    public static List<FloatingText> floatingTexts;
    private float scale;

    private void Start()
    {
        floatingTexts = new List<FloatingText>();
        scale = Screen.height / 1920f;
    }

    private void Update()
    {
        foreach (FloatingText floatingText in floatingTexts)
        {
            floatingText.UpdateFloatingText();
        }
    }

    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        if (GameManager.instance.damageType == 1)
        {

            FloatingText floatingText = GetFloatingText();

            floatingText.text.text = message;
            floatingText.text.fontSize = fontSize * scale;
            floatingText.text.color = color;
            floatingText.gameObject.transform.position = Camera.main.WorldToScreenPoint(position) + new Vector3(Random.Range(-randomPosition, randomPosition), Random.Range(-randomPosition, randomPosition), Random.Range(-randomPosition, randomPosition));

            floatingText.motion = motion * scale;
            floatingText.duration = duration;

            floatingText.Show();
        }
    }
    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.isActive);

        if (txt == null) 
        {
            txt = new FloatingText();
            txt.gameObject = Instantiate(textPrefab);
            txt.gameObject.transform.SetParent(textContainer.transform);
            txt.text = txt.gameObject.GetComponent<TextMeshProUGUI>();
            
            floatingTexts.Add(txt);
        }

        return txt;
    }
}
