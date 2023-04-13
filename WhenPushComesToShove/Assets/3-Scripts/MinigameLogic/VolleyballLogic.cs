using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//Set up launching of bomb on spawn
//Set up ramping variables/controls

struct RampingSideParts
{
    public ObjectSpawner spawner;
    public Transform chalkLineParent;
    public Transform bumperParent;
    public bool spawnerRampApplied;
}
public class VolleyballLogic : MinigameLogic
{
    [Header("Parameters")]
    [SerializeField]
    private float launchMin;
    [SerializeField]
    private float launchMax;
    [SerializeField]
    private float bombTime;
    [Header("Parts")]
    [SerializeField]
    private ObjectSpawner middleSpawner;
    [SerializeField]
    private Transform bombParent;
    [Header("Ramping")]
    [Header("Left")]
    [SerializeField]
    private Transform leftChalkLineParent;
    [SerializeField]
    private Transform leftBumperParent;
    [SerializeField]
    private ObjectSpawner leftSpawner;
    [Header("Left")]
    [SerializeField]
    private Transform rightChalkLineParent;
    [SerializeField]
    private Transform rightBumperParent;
    [SerializeField]
    private ObjectSpawner rightSpawner;

    private RampingSideParts leftParts;
    private RampingSideParts rightParts;
    private List<SpriteRenderer> chalkSprites;
    private uint bombMessageId;
    private uint clusterMessageId;
    bool spawning = false;
    private AddForceOnSpawn forceAttachment;
    
    

    public override void Init()
    {
        chalkSprites = new List<SpriteRenderer>();

        bombMessageId = Messenger.RegisterEvent("BombExploded", OnBombExplode);
        clusterMessageId = Messenger.RegisterEvent("ClusterExploded", OnClusterExplode);

        //Ooooo fancy lambdas :) just tells the minigame that we have actually spawned the object
        middleSpawner.onObjectSpawned += (Transform t) => {spawning = false;};
        forceAttachment = middleSpawner.GetComponent<AddForceOnSpawn>();
        middleSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;
        leftSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;
        rightSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;

        //Ramping struct
        leftParts.spawner = leftSpawner;
        leftParts.chalkLineParent = leftChalkLineParent;

        for(int i = 0; i < leftParts.chalkLineParent.childCount; i++)
        {
            chalkSprites.Add(leftParts.chalkLineParent.GetChild(i).GetComponent<SpriteRenderer>());
        }

        leftParts.bumperParent = leftBumperParent;
        leftParts.bumperParent.gameObject.SetActive(false);
        leftParts.spawnerRampApplied = false;

        rightParts.spawner = rightSpawner;
        rightParts.chalkLineParent = rightChalkLineParent;

        for (int i = 0; i < rightParts.chalkLineParent.childCount; i++)
        {
            chalkSprites.Add(rightParts.chalkLineParent.GetChild(i).GetComponent<SpriteRenderer>());
        }

        rightParts.bumperParent = rightBumperParent;
        rightParts.bumperParent.gameObject.SetActive(false);
        rightParts.spawnerRampApplied = false;

        foreach(SpriteRenderer chalk in chalkSprites)
        {
            chalk.enabled = false;
        }

        base.Init();
    }

    public override void StartGame()
    {
        leftParts.chalkLineParent.gameObject.SetActive(false);       

        rightParts.chalkLineParent.gameObject.SetActive(false);

        foreach (SpriteRenderer chalk in chalkSprites)
        {
            chalk.enabled = true;
        }

        forceAttachment.force = Random.insideUnitCircle.normalized * Random.Range(launchMin, launchMax);
        middleSpawner.SpawnWithoutDelay();
        base.StartGame();
    }

    private void Update()
    {
        //When there are no bombs left, spawn a new one
        //Don't try to spawn a new bomb if there aren't any around but we are just waiting on the spawner
        if (gameRunning && !spawning && bombParent.childCount == 0)
        {
            forceAttachment.force = Random.insideUnitCircle.normalized * Random.Range(launchMin, launchMax);
            middleSpawner.SpawnWithDelay();
            spawning = true;
        }
    }

    private void OnBombExplode(MessageArgs args)
    {
        AddScore(1, (Vector2)args.vectorArg);
    }

    private void OnClusterExplode(MessageArgs args)
    {
        AddScore(.25f, (Vector2)args.vectorArg);
    }

    private void AddScore(float amount, Vector2 position)
    {
        //If on the left, give score to right team
        if (position.x <= 0)
        {
            data.AddScoreForTeam(0, amount);
            RampSide(rightParts, data.scores[0]);
        }
        //if on the right, give score to left team
        else
        {
            data.AddScoreForTeam(1, amount);
            RampSide(leftParts, data.scores[1]);
        }

        if (endCondition.TestCondition() && gameRunning)
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

            EndGame();
        }
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("BombExploded", bombMessageId);
        Messenger.UnregisterEvent("ClusterExploded", clusterMessageId);

        chalkSprites.Clear();

        base.CleanUp();
    }

    private void RampSide(RampingSideParts parts, float score)
    {
        if (score >= 2 && !parts.chalkLineParent.gameObject.activeSelf)
        {
            parts.chalkLineParent.gameObject.SetActive(true);
        }
        if (score >= 4 && !parts.bumperParent.gameObject.activeSelf)
        {
            parts.bumperParent.gameObject.SetActive(true);
        }
        if (score >= 6 && !parts.spawnerRampApplied)
        {
            middleSpawner.onObjectSpawned += (Transform t) => parts.spawner.SpawnWithoutDelay();
            parts.spawnerRampApplied = true;
        }
    }

}
