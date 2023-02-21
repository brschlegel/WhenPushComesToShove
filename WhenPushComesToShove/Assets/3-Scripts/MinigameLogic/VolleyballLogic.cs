using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//Set up launching of bomb on spawn
//Set up ramping variables/controls

public class VolleyballLogic : MinigameLogic
{
   [Header("Parts")]
   [SerializeField]
   private ObjectSpawner middleSpawner;
   [SerializeField]
   private Transform bombParent;

   private uint bombMessageId;
   private uint clusterMessageId;
   bool spawning = false;

    public override void Init()
    {
        bombMessageId = Messenger.RegisterEvent("BombExplode", OnBombExplode);
        clusterMessageId = Messenger.RegisterEvent("ClusterExplode", OnClusterExplode);

        //Ooooo fancy lambdas :) just tells the minigame that we have actually spawned the object
        middleSpawner.onObjectSpawned += (Transform t) => {spawning = false;};
        base.Init();
    }

    public override void StartGame()
    {
        middleSpawner.SpawnWithoutDelay();
        base.StartGame();
    }

    private void Update()
    {
        //When there are no bombs left, spawn a new one
        //Don't try to spawn a new bomb if there aren't any around but we are just waiting on the spawner
        if(gameRunning && !spawning && bombParent.childCount == 0)
        {
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
        }
        //if on the right, give score to left team
        else
        {
            data.AddScoreForTeam(0, amount);
        }
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("BombExplode", bombMessageId);
        Messenger.UnregisterEvent("ClusterExplode", clusterMessageId);
        base.CleanUp();
    }
   
}
