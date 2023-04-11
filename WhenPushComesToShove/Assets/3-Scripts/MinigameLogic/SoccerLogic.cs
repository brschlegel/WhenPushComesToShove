using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerLogic : MinigameLogic
{
    [SerializeField] private List<ObjectSpawner> spawners;
    [SerializeField] private List<Goal> goals;

    private FMOD.Studio.EventInstance sound;

    public override void Init()
    {
        foreach(Goal g in goals)
        {
            g.goalScored += OnGoalScored;
        }

        sound = AudioManager.instance.PlayWithInstance(FMODEvents.instance.generalCrowd);
 
        base.Init();
    }
    private void Update()
    {
        if (gameRunning)
        {
            if (endCondition.TestCondition())
            {
                if (data.scores[0] > data.scores[1])
                {
                    ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = 0;
                }
                else if (data.scores[1] > data.scores[0])
                {
                    ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = 1;
                }
                else
                {
                    ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = -1;
                }
                //int winningTeamIndex = ((LastTeamStanding)endCondition).winningTeamNum;
                //((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = winningTeamIndex;

                sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                EndGame();
            }
        }

    }

    private void OnGoalScored()
    {
        //Find The Spawner Without A Soccer Ball
        foreach  (ObjectSpawner s in spawners)
        {
            if (s.transform.childCount < 2)
            {
                s.Spawn();
            }
        }
        //ObjectSpawner spawner = spawners.GetRandomElement<ObjectSpawner>();
        //spawner.Spawn();
    }

}