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
    private uint bombMessageId;
    private uint clusterMessageId;
    bool spawning = false;
    private AddForceOnSpawn forceAttachment;
    

    public override void Init()
    {
        bombMessageId = Messenger.RegisterEvent("BombExploded", OnBombExplode);
        clusterMessageId = Messenger.RegisterEvent("ClusterExplode", OnClusterExplode);

        //Ooooo fancy lambdas :) just tells the minigame that we have actually spawned the object
        middleSpawner.onObjectSpawned += (Transform t) => {spawning = false;};
        forceAttachment = middleSpawner.GetComponent<AddForceOnSpawn>();
        middleSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;
        leftSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;
        rightSpawner.GetComponent<SetExplodeTimeOnSpawn>().time = bombTime;

        //Ramping struct
        leftParts.spawner = leftSpawner;
        leftParts.chalkLineParent = leftChalkLineParent;
        leftParts.bumperParent = leftBumperParent;

        rightParts.spawner = rightSpawner;
        rightParts.chalkLineParent = rightChalkLineParent;
        rightParts.bumperParent = rightBumperParent;
        base.Init();
    }

    public override void StartGame()
    {
        forceAttachment.force = Random.insideUnitCircle.normalized * Random.Range(launchMin, launchMax);
        middleSpawner.SpawnWithoutDelay();
        base.StartGame();
    }

    private void Update()
    {
        //When there are no bombs left, spawn a new one
        //Don't try to spawn a new bomb if there aren't any around but we are just waiting on the spawner
        if(gameRunning && !spawning && bombParent.childCount == 0)
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
        if(position.x <= 0)
        {
            data.AddScoreForTeam(1, amount);
            RampSide(rightParts, data.scores[1]);
        }
        //if on the right, give score to left team
        else
        {
            data.AddScoreForTeam(0, amount);
            RampSide(leftParts, data.scores[0]);
        }

        if(endCondition.TestCondition() && gameRunning)
        {
            EndGame();
        }
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("BombExplode", bombMessageId);
        Messenger.UnregisterEvent("ClusterExplode", clusterMessageId);
        base.CleanUp();
    }

    private void RampSide(RampingSideParts parts, float score)
    {
        switch(score)
        {
            case 2: 
            parts.chalkLineParent.gameObject.SetActive(true);
            break;
            case 4:
            parts.bumperParent.gameObject.SetActive(true);
            break;
            case 6:
            middleSpawner.onObjectSpawned += (Transform t) => parts.spawner.Spawn();
            break;
        }
    }
   
}
