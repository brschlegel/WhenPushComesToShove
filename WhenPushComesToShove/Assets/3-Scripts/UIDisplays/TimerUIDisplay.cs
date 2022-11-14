using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUIDisplay : UIDisplay
{
    [SerializeField] private TextMeshProUGUI timerText;
    [HideInInspector] public float timer;
    [HideInInspector] public bool timeOut = false;
    [HideInInspector] public bool canUpdateTime = false;

    public override void HideDisplay()
    {
        gameObject.SetActive(false);
    }

    public override void ShowDisplay()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
        if (canUpdateTime)
        {
            timer -= Time.deltaTime;

            UpdateTimerText();
        }
    }

    public void UpdateTimerText()
    {
        int min = Mathf.FloorToInt(timer / 60);
        int sec = Mathf.FloorToInt(timer % 60);

        if (timer <= 0)
        {
            timerText.text = "0:00";
            timeOut = true;
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
