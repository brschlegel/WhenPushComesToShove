using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerEndCondition : BaseEndCondition
{
    [SerializeField] private TimerUIDisplay timerDisplay;
    [SerializeField] private float timeBeforeEnd;

    public override void Init()
    {
        timerDisplay.timeOut = false;
        timerDisplay.timer = timeBeforeEnd;
        timerDisplay.canUpdateTime = true;
    }

    public override bool TestCondition()
    {
        return timerDisplay.timeOut;
    }
}
