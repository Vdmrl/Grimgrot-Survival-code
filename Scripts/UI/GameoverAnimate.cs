using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class GameoverAnimate : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup textFade;
    [SerializeField]
    private CanvasGroup buttonsFade;
    [Header("Links")]
    public LocalizeStringEvent title;
    public LocalizeStringEvent text;

    private LocalizedString textString;//для переменных перевода, я смог заставить их работать но это пиздец
    public StringVariable timeValue = null;
    public IntVariable killsValue = null;
    public IntVariable levelValue = null;
    public IntVariable goldValue = null;

    public void EvaluateLocale() //необходимо для доступа к переменным в переводе, какой ужас
    {
        textString = text.StringReference;

        //time variable
        if (!textString.TryGetValue("time", out var timeVariable))
        {
            timeValue = new StringVariable();
            textString.Add("time", timeValue);
        }
        else
        {
            timeValue = timeVariable as StringVariable;
        }
        //kills variable
        if (!textString.TryGetValue("kills", out var killsVariable))
        {
            killsValue = new IntVariable();
            textString.Add("kills", killsValue);
        }
        else
        {
            killsValue = killsVariable as IntVariable;
        }
        //level variable
        if (!textString.TryGetValue("level", out var levelVariable))
        {
            levelValue = new IntVariable();
            textString.Add("level", levelValue);
        }
        else
        {
            levelValue = levelVariable as IntVariable;
        }
        //gold variable
        if (!textString.TryGetValue("gold", out var goldVariable))
        {
            goldValue = new IntVariable();
            textString.Add("gold", goldValue);
        }
        else
        {
            goldValue = goldVariable as IntVariable;
        }
    }

    void Start()
    {
        LeanTween.alphaCanvas(this.gameObject.GetComponent<CanvasGroup>(), 1, 1).setEaseOutQuart().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(textFade, 1, 1).setDelay(2f).setEaseOutCubic().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(buttonsFade, 1, 1).setDelay(4f).setEaseOutQuint().setIgnoreTimeScale(true);
    }
}
