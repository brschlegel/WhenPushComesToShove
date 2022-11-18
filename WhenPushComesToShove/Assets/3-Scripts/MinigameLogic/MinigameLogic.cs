using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Minigame {Dodgeball, Soccer, HotPotato, Sumo, All};

//[RequireComponent(typeof(LevelProperties))]
public abstract class MinigameLogic : MonoBehaviour
{
    [SerializeField]
    protected UIDisplay startingUIDisplay;
    [SerializeField]
    protected UIDisplay endingUIDisplay;

    [SerializeField]
    protected BaseEndCondition endCondition;
    protected bool gameRunning;

    public event emptyDelegate onGameStart;


    public virtual void Init()
    {
        startingUIDisplay.ShowDisplay();
        CoroutineManager.StartGlobalCoroutine(WaitToStartGame());
    }
    
    public virtual void StartGame()
    {
        gameRunning = true;
        onGameStart?.Invoke();
        endCondition.Init();
    }
    public virtual void EndGame()
    {
        gameRunning = false;
        endingUIDisplay.ShowDisplay();
        CoroutineManager.StartGlobalCoroutine(WaitToCleanUp());
    }
    public virtual void CleanUp()
    {
        endingUIDisplay.HideDisplay();
        LevelManager.onNewRoom.Invoke();
    }

    protected IEnumerator WaitToStartGame()
    {
        if(startingUIDisplay != null)
        {
            yield return new WaitUntil(() => startingUIDisplay.isDone);
        }
        StartGame();
    }

    protected IEnumerator WaitToCleanUp()
    {
        if(endingUIDisplay != null)
        {
            yield return new WaitUntil(() => endingUIDisplay.isDone);
        }
        CleanUp();
    }
}
