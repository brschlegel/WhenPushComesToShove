using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballLogic : MinigameLogic
{
    [SerializeField]
    private BatchObjectSpawnerLocations dodgeballSpawner;

    [SerializeField]
    private LastTeamStanding endCondition;

    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        
    }

    public override void StartGame()
    {
        dodgeballSpawner.SpawnWithDelay();
    }
    
}
