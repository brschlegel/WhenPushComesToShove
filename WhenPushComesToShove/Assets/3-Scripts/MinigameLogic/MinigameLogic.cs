using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigameLogic : MonoBehaviour
{
    private List<BaseEndCondition> endConditions;

    [SerializeField]
    private UIDisplay startingUIDisplay;
    [SerializeField]
    private UIDisplay endingUIDisplay;
    private void OnEnable()
    {
        if(endConditions == null)
        {
           Init();
        }
    } 

    public virtual void Init()
    {
        endConditions = new List<BaseEndCondition>(GetComponentsInChildren<BaseEndCondition>());
        startingUIDisplay.ShowDisplay();
        CoroutineManager.StartGlobalCoroutine(WaitToStartGame());
    }
    
    public abstract void StartGame();
    public virtual void EndGame()
    {
        endingUIDisplay.ShowDisplay();
        CoroutineManager.StartGlobalCoroutine(WaitToCleanUp());
    }
    public virtual void CleanUp()
    {

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
