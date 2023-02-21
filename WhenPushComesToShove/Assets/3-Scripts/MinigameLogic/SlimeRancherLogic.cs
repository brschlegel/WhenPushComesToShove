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
    [SerializeField] private bool scoreOnlyBiggestSlime = false;
    [SerializeField] private Transform slimesParent;
    private bool isTie = false;

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
                //Find biggest slime
                if (scoreOnlyBiggestSlime)
                {
                    Slime largestSlime = GetLargestSlime();

                    if (isTie || largestSlime.slimeTeamIndex == -1)
                    {
                        ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = -1;
                    }
                    else
                    {
                        data.scores[largestSlime.slimeTeamIndex] += 1;

                        if (data.scores[0] > data.scores[1])
                        {
                            ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = 0;
                        }
                        else if (data.scores[1] > data.scores[0])
                        {
                            ((TeamWinUIDisplay)endingUIDisplay).winningTeamNum = 1;
                        }
                    }
                }
                //Assign points for slimes based on size
                else
                {
                    //Slime[] allSlimes = slimesParent.GetComponentsInChildren<Slime>();
                    //
                    //foreach (Slime s in allSlimes)
                    //{
                    //    if (s.slimeTeamIndex != -1)
                    //    {
                    //        data.AddScoreForTeam(s.slimeTeamIndex, s.pointWorth);
                    //    }
                    //}

                    //Decide Winner
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
                }

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

    public Slime GetLargestSlime()
    {
        Slime[] allSlimes = slimesParent.GetComponentsInChildren<Slime>();
        int largestSlimeIndex = 0;
        isTie = false;

        //Find Winning Team
        for (int i = 1; i < allSlimes.Length; i++)
        {
            if (allSlimes[i].slimeTeamIndex != -1)
            {
                if (allSlimes[i].slimeSize > allSlimes[largestSlimeIndex].slimeSize)
                {
                    largestSlimeIndex = i;
                    isTie = false;
                }
                else if (allSlimes[i].slimeSize == allSlimes[largestSlimeIndex].slimeSize)
                {
                    if (allSlimes[i].slimeTeamIndex != allSlimes[largestSlimeIndex].slimeTeamIndex)
                    {
                        isTie = true;
                    }
                }
            }
        }

        return allSlimes[largestSlimeIndex];
    }

    public void UpdateTeamScore(int teamIndex, int score, bool addScore)
    {
        if (addScore)
        {
            data.AddScoreForTeam(teamIndex, score);
        }
        else
        {
            data.RemoveScoreForTeam(teamIndex, score);
        }
    }
}
