using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballLogic : MinigameLogic
{
    [SerializeField]
    private BatchObjectSpawnerLocations dodgeballSpawner;

    private void Update()
    {
        if(gameRunning)
        {
            if(endCondition.TestCondition())
            {
                int winningTeamIndex = ((LastTeamStanding)endCondition).winningTeamNum;

                data.AddScoreForTeam(winningTeamIndex, 1);

                ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = winningTeamIndex;
                EndGame();
            }
        }

    }

    public override void CleanUp()
    {
        dodgeballSpawner.CleanUpSpawnedObjects();
        base.CleanUp();
    }

    public override void StartGame()
    {
        dodgeballSpawner.SpawnWithDelay();
        base.StartGame();
    }

}
