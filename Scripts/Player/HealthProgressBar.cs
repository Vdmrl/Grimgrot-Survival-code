using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProgressBar : MonoBehaviour
{
    private float current;
    private float maximum;
    private bool isHide = false;
    private RectTransform parrent;

    private void Start()
    {
        parrent = GetComponentInParent<RectTransform>();
        UpdateBar();
    }

    public void AddValue(float value)
    {
        current += value;
        if (current < 0)
        {
            current = 0;
        }
        else if (current > maximum)
        {
            current = maximum;
        }
        UpdateBar();
    }

    public void SetValue(float value)
    {
        if (value > maximum)
        {
            current = maximum;
        }
        else
        {
            current = value;
        }
        UpdateBar();
    }

    public void ChangeMax(float newMax)
    {
        if (newMax >= maximum)
        {
            current += newMax - maximum;
            maximum = newMax;
        }
        else
        {
            if (current >= newMax)
            {
                current = newMax;
                maximum = newMax;
            }
            else
            {
                maximum = newMax;
            }
        }
        UpdateBar();
    }

    public void ResetBar(float val)
    {
        current = val;
        maximum = val;
        UpdateBar();
    }
    
    private void UpdateBar()
    {
        if (!isHide && Math.Abs(current - maximum) < 0.99f)
        {
            gameObject.transform.parent.gameObject.SetActive(false); 
            isHide = true;
        }
        else
        {
            if (isHide) gameObject.transform.parent.gameObject.SetActive(true);
            var amount = current / (float)maximum;
            gameObject.transform.localScale = new Vector3(amount, 1, 1);
            isHide = false;
        }
    }   
}
