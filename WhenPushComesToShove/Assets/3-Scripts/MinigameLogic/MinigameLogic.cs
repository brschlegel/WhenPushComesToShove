using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Minigame {All, Dodgeball, Soccer, HotPotato, Sumo, Pinball};

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

    [SerializeField] protected MinigameData data;

    public event emptyDelegate onGameStart;
    [SerializeField] protected bool canPlayersTakeDamage = true;

    public bool pauseMovementAtStart = true;


    public virtual void Init()
    {
        if (startingUIDisplay != null)
        {
            startingUIDisplay.ShowDisplay();

            //Lock Player Movement
            if (pauseMovementAtStart)
            {
                foreach (Transform p in GameState.players)
                {
                    //p.GetComponentInChildren<PlayerMovementScript>().ChangeMoveSpeed(0);
                    p.GetComponentInChildren<PlayerInputHandler>().movementPaused = true;
                }
            }
        }

        UpdatePlayerInvulnurability(canPlayersTakeDamage);

        CoroutineManager.StartGlobalCoroutine(WaitToStartGame());
    }

    public virtual void StartGame()
    {
        //Unlock Player Movement
        if (pauseMovementAtStart)
        {
            foreach (Transform p in GameState.players)
            {
                //p.GetComponentInChildren<PlayerMovementScript>().ResetMoveSpeed();
                p.GetComponentInChildren<PlayerInputHandler>().movementPaused = false;
            }
        }
        GameState.ModifierManager.StartMinigame();
        gameRunning = true;
        onGameStart?.Invoke();
        endCondition.Init();
    }
    public virtual void EndGame()
    {
        gameRunning = false;

        if (endingUIDisplay != null)
        {
            endingUIDisplay.ShowDisplay();
        }

        if (data != null)
        {
            data.OnMinigameEnd();
        }

        CoroutineManager.StartGlobalCoroutine(WaitToCleanUp());
    }
    public virtual void CleanUp()
    {
        if (!canPlayersTakeDamage)
        {
            UpdatePlayerInvulnurability(true);
        }

        if (endingUIDisplay)
        {
            endingUIDisplay.HideDisplay();
        }

        LevelManager.onNewRoom.Invoke();
    }

    protected IEnumerator WaitToStartGame()
    {
        if (startingUIDisplay != null)
        {
            yield return new WaitUntil(() => startingUIDisplay.isDone);
        }
        StartGame();
    }

    protected IEnumerator WaitToCleanUp()
    {
        if (endingUIDisplay != null)
        {
            yield return new WaitUntil(() => endingUIDisplay.isDone);
        }
        CleanUp();
    }

    protected void UpdatePlayerInvulnurability(bool enabled)
    {
        GameState.damageEnabled = enabled;

        foreach (HealthBar b in GameState.playerHealthBars)
        {
            b.gameObject.SetActive(enabled);
        }
    }
}
