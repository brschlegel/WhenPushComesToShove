using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Minigame {All, Dodgeball, Soccer, HotPotato, Sumo, Pinball};

//All minigames flow through these methods and can override to add functionality at any step
//Init -> StartGame -> EndGame -> CleanUp
//Children logic scripts only worry about calling EndGame
public abstract class MinigameLogic : MonoBehaviour
{
    public event emptyDelegate onGameStart;
    public bool pauseMovementAtStart = true;

    [Header("UI Displays")]
    [SerializeField]
    protected UIDisplay startingUIDisplay;
    [SerializeField]
    protected UIDisplay endingUIDisplay;

    //Assigned in inspector
    [SerializeField]
    protected BaseEndCondition endCondition;
    [SerializeField] 
    protected MinigameData data;

    [SerializeField] 
    protected bool canPlayersTakeDamage = true;

    protected bool gameRunning;

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
                    p.GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
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
                p.GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                p.GetComponentInChildren<PositionFreezer>().UnlockPosition();
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

    protected void UpdatePlayerInvulnurability(bool enabled)
    {
        GameState.damageEnabled = enabled;

        foreach (HealthBar b in GameState.playerHealthBars)
        {
            b.gameObject.SetActive(enabled);
        }
    }

    //Enumerators to handle transitions from UIDisplays
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

    
}
