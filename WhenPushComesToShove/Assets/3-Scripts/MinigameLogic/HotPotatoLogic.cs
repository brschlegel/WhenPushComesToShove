using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotatoLogic : MinigameLogic
{
    [SerializeField]
    private ObjectSpawnerRandomLocations spawner;

    [Header("RampSettings")]
    [SerializeField]
    private float startBombSpawnInterval;
    [SerializeField]
    private float endBombSpawnInterval;
    [SerializeField]
    private float bombSpawnIncrement = -1;

    private int numBombs;

    public override void StartGame()
    {
        numBombs = 2;
        spawner.SpawnWithDelay();
        spawner.SpawnWithDelay();
        StartCoroutine(SpawnAdditionalBomb());
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

    public IEnumerator SpawnAdditionalBomb()
    {
        yield return new WaitForSeconds(SpawnInterval);
        spawner.SpawnWithoutDelay();
        numBombs++;
        StartCoroutine(SpawnAdditionalBomb());
    }

    public float SpawnInterval
    {
        get{return Mathf.Clamp((numBombs - 1) * bombSpawnIncrement + startBombSpawnInterval, endBombSpawnInterval, startBombSpawnInterval );}  
    }

}
