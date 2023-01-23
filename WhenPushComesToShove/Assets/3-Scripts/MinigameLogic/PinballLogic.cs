using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballLogic :MinigameLogic
{
    [SerializeField] private ObjectSpawnerRandomLocations spawner;
    [SerializeField] private float spawnInterval;

    private int numBalls;

    public override void StartGame()
    {
        numBalls = 1;
        spawner.SpawnWithDelay();
        StartCoroutine(SpawnAdditionalBalls());
        base.StartGame();
    }

    private void Update()
    {
        if (gameRunning)
        {
            if (endCondition.TestCondition())
            {
                Transform winner = ((LastManStandingEndCondition)endCondition).winner;
                ((PlayerWinUIDisplay)endingUIDisplay).winnerName = winner.gameObject.name;
                EndGame();
            }
        }
    }

    public override void CleanUp()
    {
        spawner.CleanUpSpawnedObjects();
        base.CleanUp();
    }

    public IEnumerator SpawnAdditionalBalls()
    {
        yield return new WaitForSeconds(spawnInterval);
        spawner.SpawnWithoutDelay();
        numBalls++;
        StartCoroutine(SpawnAdditionalBalls());
    }
}
