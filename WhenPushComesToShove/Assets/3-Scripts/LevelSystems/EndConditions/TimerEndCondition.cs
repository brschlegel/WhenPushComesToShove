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
        if (GameState.currentRoomType != LevelType.Lobby)
            MusicManager.instance.SetDeltaIntensity(1, timeBeforeEnd / 3f);
    }

    public override bool TestCondition()
    {
        return timerDisplay.timeOut;
    }

    public float MaxTime
    {
        get{return timeBeforeEnd;}
    }

    public float CurrentTimeLeft
    {
        get{return timerDisplay.timer;}
    }
}
