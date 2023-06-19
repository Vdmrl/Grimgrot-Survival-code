using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceProgressBar : MonoBehaviour
{
    private float current;
    private float maximum;

    public void AddValue(float value)
    {
        current += value;
        UpdateBar();
    }
    
    public void ResetBar(float newMaximum)
    {
        current = 0;
        maximum = newMaximum;
        UpdateBar();
    }

    private void UpdateBar()
    {
        var amount = current / (float)maximum;
        gameObject.transform.localScale = new Vector3(amount, 1, 1);
    }
}