using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerEndCondition : BaseEndCondition
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeBeforeEnd;
    private float timer;

    public override void Init()
    {
        timer = timeBeforeEnd;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        UpdateTimerText();
    }

    public override bool TestCondition()
    {
        if (timer <= 0)
        {
            return true;
        }

        return false;
    }

    public void UpdateTimerText()
    {
        int min = Mathf.FloorToInt(timer / 60);
        int sec = Mathf.FloorToInt(timer % 60);

        if (timer <= 0)
        {
            timerText.text = "0:00";
        }
        else if (sec < 10)
        {
            timerText.text = min + ":0" + sec;
        }
        else
        {
            timerText.text = min + ":" + sec;
        }
    }
}
