using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballLogic :MinigameLogic
{
    [SerializeField] private ObjectSpawnerRandomLocations[] spawners;
    [SerializeField] private float spawnInterval;
    [SerializeField] private Goal dangerZone;

    private int numBalls;

    public override void Init()
    {
        dangerZone.goalScored += OnGoalScored;
        base.Init();
    }
    public override void StartGame()
    {
        numBalls = 1;
        
        foreach(ObjectSpawnerRandomLocations spawner in spawners)
        {
            spawner.SpawnWithDelay();
        }
        //StartCoroutine(SpawnAdditionalBalls());
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
        dangerZone.goalScored -= OnGoalScored;
        foreach (ObjectSpawnerRandomLocations spawner in spawners)
        {
            spawner.CleanUpSpawnedObjects();
        }
        base.CleanUp();
    }

    public IEnumerator SpawnAdditionalBalls()
    {
        yield return new WaitForSeconds(spawnInterval);
        
        foreach (ObjectSpawnerRandomLocations spawner in spawners)
        {
            spawner.SpawnWithoutDelay();
        }
        numBalls++;
        StartCoroutine(SpawnAdditionalBalls());
    }

    public void OnGoalScored()
    {
        foreach(ObjectSpawnerRandomLocations spawner in spawners)
        {
            if (spawner.spawnedObjectParent.childCount <= 0)
                spawner.Spawn();
        }
    }
}
