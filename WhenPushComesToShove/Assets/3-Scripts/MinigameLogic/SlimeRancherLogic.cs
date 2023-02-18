using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRancherLogic : MinigameLogic
{
    [SerializeField]
    private ObjectSpawnerRandomLocations spawner;
    [Header("RampSettings")]
    [SerializeField]
    private float startSlimeSpawnInterval;
    [SerializeField]
    private float endSlimeSpawnInterval;
    [SerializeField]
    private float slimeSpawnIncrement = -1;

    private int numSlimes;

    public override void StartGame()
    {
        numSlimes = 2;
        spawner.SpawnWithDelay();
        spawner.SpawnWithDelay();
        StartCoroutine(SpawnAdditionalSlime());
        base.StartGame();
    }

    private void Update()
    {
        if (gameRunning)
        {
            if (endCondition.TestCondition())
            {
                //Transform winner = ((LastManStandingEndCondition)endCondition).winner;

                //PlayerConfiguration config = winner.GetComponentInChildren<PlayerInputHandler>().playerConfig;
                //Assign point to let the system know who won
                //data.AddScoreForTeam(config.PlayerIndex, 1);

                //((PlayerWinUIDisplay)endingUIDisplay).winnerName = GameState.playerNames[config.PlayerIndex];
                EndGame();
            }
        }
    }

    public override void CleanUp()
    {
        spawner.CleanUpSpawnedObjects();
        base.CleanUp();
    }

    public IEnumerator SpawnAdditionalSlime()
    {
        yield return new WaitForSeconds(SpawnInterval);
        spawner.SpawnWithoutDelay();
        numSlimes++;
        StartCoroutine(SpawnAdditionalSlime());
    }

    public float SpawnInterval
    {
        get { return Mathf.Clamp((numSlimes - 1) * slimeSpawnIncrement + startSlimeSpawnInterval, endSlimeSpawnInterval, startSlimeSpawnInterval); }
    }
}
