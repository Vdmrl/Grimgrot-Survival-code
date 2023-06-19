using System;
using TMPro;
using UnityEngine;


public class Stopwatch : MonoBehaviour
{
    private bool isActive = false;
    public TextMeshProUGUI timeText;
    // Time for all game!
    public TimeSpan time;
    public string timeString; //я поменял чтобы выводить время в конце игры
    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isActive)
        {
            GameManager.instance.currentTime += Time.deltaTime;
            time = TimeSpan.FromSeconds(GameManager.instance.currentTime);
            if (time.Seconds.ToString().Length == 1)
            {
                timeString = time.Minutes + ":0" + time.Seconds;
            }
            else
            {
                timeString = time.Minutes + ":" + time.Seconds;
            }

            timeText.text = timeString;

            if (time.Minutes == 15)
            {
                GameManager.instance.EndGame(true);
            }
        }
    }

    public void StartStopwatch()
    {
        isActive = true;
    }

    public void StopStopwatch()
    {
        isActive = false;
    }

    public void RestartStopwatch()
    {
        GameManager.instance.currentTime = 0;
    }
}
