using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoLogic : MinigameLogic
{
    [SerializeField]
    private List<SpikeGroup> spikeGroups;
    [SerializeField]
    private Respawner respawner;
    [SerializeField]
    private float timeToThreaten;
    [SerializeField]
    private float timeToActivateFromThreaten;
    [SerializeField]
    private AnimationCurve respawnerDelayByTime;

    private int nextGroupIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Init()
    {
        base.Init();

        foreach (HealthBar b in GameState.playerHealthBars)
        {
            b.gameObject.SetActive(false);
        }
    }
    public override void StartGame()
    {
        nextGroupIndex = 1;
        respawner.onDetectDeath += OnDeath;
        StartCoroutine(ActivateSpikeGroup());
        base.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            TimerEndCondition timer = (TimerEndCondition)endCondition;
            respawner.spawnDelay = respawnerDelayByTime.Evaluate(1 - (timer.CurrentTimeLeft / timer.MaxTime));
            if (endCondition.TestCondition())
            {
                int winnerIndex = data.GetHighestScoreIndex();
                ((PlayerWinUIDisplay)endingUIDisplay).winnerName = GameState.playerNames[winnerIndex];
                EndGame();
            }
        }
    }

    private IEnumerator ActivateSpikeGroup()
    {
        yield return new WaitForSeconds(timeToThreaten);
        spikeGroups[nextGroupIndex].Threaten();
        yield return new WaitForSeconds(timeToActivateFromThreaten);
        spikeGroups[nextGroupIndex].Activate();
        nextGroupIndex++;
        if (nextGroupIndex < spikeGroups.Count)
        {
            StartCoroutine(ActivateSpikeGroup());
        }
    }

    public void OnDeath(int index)
    {
        GameObject deadPlayer = PlayerConfigManager.Instance.GetPlayerConfigs()[index].PlayerObject;
        GameObject killer = deadPlayer.GetComponentInChildren<ProjectileMode>().sourceObject;
        if (killer != null)
        {
            int killerIndex = killer.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex;
            data.AddScoreForTeam(killerIndex, 1);
        }
        else
        {
            data.AddScoreForTeam(index, -1);
        }

    }

    public override void CleanUp()
    {
        foreach(SpikeGroup g in spikeGroups)
        {
            g.Deactivate();
        }
        base.CleanUp();
    }
}
