using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdEndCondition : BaseEndCondition
{
    [SerializeField] private MinigameData data;
    public int threshold = 20;

    public override void Init()
    {
    }

    public override bool TestCondition()
    {
        foreach (int score in data.scores)
        {
            if (score >= threshold)
            {
                return true;
            }
        }

        return false;
    }
}
