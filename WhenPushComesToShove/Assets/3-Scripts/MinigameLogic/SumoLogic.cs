using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoLogic : MinigameLogic
{
    [SerializeField]
    private List<SpikeGroup> spikeGroups;
    [SerializeField]
    private float timeToThreaten;
    [SerializeField]
    private float timeToActivateFromThreaten;

    private int nextGroupIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void StartGame()
    {
        nextGroupIndex = 1;
        StartCoroutine(ActivateSpikeGroup());
        base.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(endCondition.TestCondition())
        {
            Transform winner = ((LastManStandingEndCondition)endCondition).winner;
            ((PlayerWinUIDisplay)endingUIDisplay).winnerName = winner.gameObject.name;
            EndGame();
        }
    }

    private IEnumerator ActivateSpikeGroup()
    {
        yield return new WaitForSeconds(timeToThreaten);
        spikeGroups[nextGroupIndex].Threaten();
        yield return new WaitForSeconds(timeToActivateFromThreaten);
        spikeGroups[nextGroupIndex].Activate();
        nextGroupIndex++;
        if(nextGroupIndex < spikeGroups.Count)
        {
            StartCoroutine(ActivateSpikeGroup());
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
