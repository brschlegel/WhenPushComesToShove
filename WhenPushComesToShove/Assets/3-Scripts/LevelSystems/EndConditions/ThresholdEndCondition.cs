using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdEndCondition : BaseEndCondition
{
    [SerializeField] private MinigameData data;
    public int threshold = 20;

    private int currentHighScore = 0;

    public override void Init()
    {
        MusicManager.instance.SetDeltaIntensity(3f / threshold);
    }

    public override bool TestCondition()
    {
        foreach (int score in data.scores)
        {
            if (score > currentHighScore)
            {
                currentHighScore = score;
                if(GameState.currentRoomType != LevelType.Lobby)
                    MusicManager.instance.ChangeIntensity();
            }

            if (score >= threshold)
            {
                return true;
            }
        }

        return false;
    }
}
