using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//Add some hazards in
//Get player respawning in 
public class BreakAwayLogic : MinigameLogic
{
    [SerializeField]
    private Vector2 forceOnBarrel;
    [SerializeField]
    private GameObject orangeBarrelPrefab;
    [SerializeField]
    private GameObject purpleBarrelPrefab;
    [Header("Parts")]
    [SerializeField]
    private TriggerEventPasser basket;
    [SerializeField]
    private Transform barrelParent;
    [SerializeField]
    private SpikeGroup spikes;

    [Header("Left")]
    [SerializeField]
    private ObjectSpawner leftBarrelSpawner;
    [SerializeField]
    private ObjectSpawner leftBombSpawner;
    [SerializeField]
    private Respawner leftPlayerRespawner;
    [Header("Right")]
    [SerializeField]
    private ObjectSpawner rightBarrelSpawner;
    [SerializeField]
    private ObjectSpawner rightBombSpawner;
    [SerializeField]
    private Respawner rightPlayerRespawner;

    [Header("Ramping Variables")]
    [SerializeField]
    private float spikeActivationTime;
    [SerializeField]
    private float bombSpawnerActivationTime;
    [SerializeField]
    private float timeInBetweenBombSpawns;
    
    //For flip-floping barrel spawners
    private Dictionary<Team, ObjectSpawner> lastSpawnerUsed;

    public override void Init()
    {
        basket.triggerEnter += ScoreBasket;
        lastSpawnerUsed = new Dictionary<Team, ObjectSpawner>();
        SetConstantForce(leftBarrelSpawner);
        SetConstantForce(rightBarrelSpawner);

        //Don't want the barrels to start falling immediately
        //A bit clunky but whaddagunnado
        leftBarrelSpawner.onObjectSpawned += FreezeBarrel;
        rightBarrelSpawner.onObjectSpawned += FreezeBarrel;

        //Spawn Orange Barrel
        leftBarrelSpawner.ObjectToSpawn = orangeBarrelPrefab;
        leftBarrelSpawner.SpawnWithoutDelay();
        lastSpawnerUsed.Add(Team.Orange, leftBarrelSpawner);
        //Spawn Purple Barrel
        rightBarrelSpawner.ObjectToSpawn = purpleBarrelPrefab;
        rightBarrelSpawner.SpawnWithoutDelay();
        lastSpawnerUsed.Add(Team.Purple, rightBarrelSpawner);

        base.Init();
    }
    public override void StartGame()
    {
        //Ok i guess barrels can move now
        leftBarrelSpawner.onObjectSpawned -= FreezeBarrel;
        rightBarrelSpawner.onObjectSpawned -= FreezeBarrel;
        foreach(Transform barrel in barrelParent)
        {
            barrel.GetComponent<PositionFreezer>().UnlockPosition();
        }

        SetRespawners();
        //Start ramping
        StartCoroutine(ActivateSpikes());
        StartCoroutine(ActivateBombSpawners());

        base.StartGame();
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
                EndGame();
            }
        }

    }

    private void ScoreBasket(GameObject barrel)
    {
        TeamId scorerId = barrel.GetComponent<TeamId>();
        data.AddScoreForTeam(scorerId.TeamIndex,1);
        SpawnBarrel(scorerId.team);
        Destroy(barrel);
    }

    private void SpawnBarrel(Team teamid)
    {
        ObjectSpawner spawner = GetOppositeBarrelSpawner(lastSpawnerUsed[teamid]);
        spawner.ObjectToSpawn = GetPrefab(teamid);
        spawner.SpawnWithDelay();
        lastSpawnerUsed[teamid] = spawner;
    }


    //Little helper method to get the other spawner 
    private ObjectSpawner GetOppositeBarrelSpawner(ObjectSpawner spawner)
    {
        return spawner == leftBarrelSpawner ? rightBarrelSpawner : leftBarrelSpawner;
    }

    //Little hleper method to get the other bomb spawner
    private ObjectSpawner GetOppositeBombSpawner(ObjectSpawner spawner)
    {
        return spawner == leftBombSpawner ? rightBombSpawner : leftBombSpawner;
    }

    //Little helper method to get the prefab by teamid
    private GameObject GetPrefab(Team teamid)
    {
        return teamid == Team.Orange ? orangeBarrelPrefab : purpleBarrelPrefab;
    }

    //Little helper method to set up the spawner attachments
    private void SetConstantForce(ObjectSpawner spawner)
    {
        SpawnWithVelocity velocity = spawner.transform.GetComponent<SpawnWithVelocity>();
        velocity.initialForce = forceOnBarrel.magnitude;
        velocity.velocityDirection = forceOnBarrel.normalized;
        velocity.Init();
    }

    private void FreezeBarrel(Transform t)
    {
        t.GetComponent<PositionFreezer>().LockPosition(t.position);
    }
    
    private void SetRespawners()
    {
        //This is a bit jank, but want to keep players on their side
        foreach(Transform t in GameState.players)
        {
            int index = t.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex;
            if(t.position.x < 0)
            {
                rightPlayerRespawner.indicesToIgnore.Add(index);
            }
            else
            {
                leftPlayerRespawner.indicesToIgnore.Add(index);
            }
        }
    }

    //#region Ramping Coroutines!
    private IEnumerator ActivateSpikes()
    {
        yield return new WaitForSeconds(spikeActivationTime);
        spikes.Activate();
    }

    private IEnumerator ActivateBombSpawners()
    {
        yield return new WaitForSeconds(bombSpawnerActivationTime);
        //Pick a random bomb spawner to start with
        ObjectSpawner randomSide = Random.value > .5f ? leftBombSpawner : rightBombSpawner;
        StartCoroutine(SpawnBombs(randomSide));
    }

    private IEnumerator SpawnBombs(ObjectSpawner bombSpawner)
    {
        bombSpawner.SpawnWithoutDelay();
        yield return new WaitForSeconds(timeInBetweenBombSpawns);
        StartCoroutine(SpawnBombs(GetOppositeBombSpawner(bombSpawner)));
    }
}
