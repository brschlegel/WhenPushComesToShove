using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballLogic : MinigameLogic
{
    [SerializeField]
    private BatchObjectSpawnerLocations dodgeballSpawner;
    [SerializeField]
    private Transform dodgeballParent;



    private void Update()
    {
        if(gameRunning)
        {
            if(endCondition.TestCondition())
            {
                int winningTeamIndex = ((LastTeamStanding)endCondition).winningTeamNum;
                ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = winningTeamIndex;
                EndGame();
            }
        }

    }

    public override void CleanUp()
    {
        for(int i = dodgeballParent.childCount - 1; i >= 0; i--)
        {
            Destroy(dodgeballParent.GetChild(i).gameObject);
        }
        base.CleanUp();

    }

    public override void StartGame()
    {
        dodgeballSpawner.SpawnWithDelay();
        base.StartGame();
    }

}
