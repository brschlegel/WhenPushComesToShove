using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEndCondition : BaseEndCondition
{
    private float timer = 0;
    [SerializeField] private float timeForGame = 30;

    protected override void Update()
    {
        timer += Time.deltaTime;

        base.Update();
    }

    protected override void TestCondition()
    {
        if (timer >= timeForGame)
        {
            base.TestCondition();
        }
    }
}
