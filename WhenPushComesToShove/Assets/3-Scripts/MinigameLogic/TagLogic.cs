using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagLogic : MinigameLogic
{
    [SerializeField] private Sprite tagIcon;
    [SerializeField] private float timeScoreIncrement;
    [SerializeField] private float gracePeriod;
    [SerializeField] private float speedOfTagged;
    [SerializeField] private float damagePerSecond;
 
    private List<PlayerConfiguration> playerConfigs;
    private List<PlayerConfiguration> deadPlayers;
    private List<ProjectileHitbox> pHitBoxes;
    private List<PlayerMovementScript> pMovement;

    private bool gracePeriodEnded;

    private GameObject taggedPlayer;
    private float initialSpeed;

    private float currentTime;

    


    public override void Init()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
        deadPlayers = new List<PlayerConfiguration>();
        pHitBoxes = new List<ProjectileHitbox>();
        pMovement = new List<PlayerMovementScript>();

        for(int i = 0; i < playerConfigs.Count; i++)
        {
            pHitBoxes.Add(playerConfigs[i].PlayerObject.GetComponentInChildren<ProjectileHitbox>(true));
            pMovement.Add(playerConfigs[i].PlayerObject.GetComponentInChildren<PlayerMovementScript>(true));
        }

        initialSpeed = pMovement[0].maxSpeed;
        //initialAcceleration = pMovement[0].acceleration;

        UpdateTaggedPlayer(Random.Range(0, playerConfigs.Count));

        gracePeriodEnded = false;
        currentTime = 0.0f;

        for (int i = 0; i < playerConfigs.Count; i++)
        {
            //data.AddScoreForTeam(i, 10);
        }
        base.Init();
    }

    public override void StartGame()
    {
        base.StartGame();
        StartCoroutine(StartGracePeriod());
    }

    // Update is called once per frame
    void Update()
    {

        if (gameRunning)
        {
            //Prevents tagged player from taking damage at the very beginning of the game
            if (gracePeriodEnded)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= timeScoreIncrement)
                {
                    currentTime -= timeScoreIncrement;

                    for (int i = 0; i < playerConfigs.Count; i++)
                    {
                        if (playerConfigs[i].PlayerObject == taggedPlayer)
                        {
                            taggedPlayer.GetComponentInChildren<PlayerHealth>().TakeDamage(damagePerSecond, "Tagged");
                            //If the tagged player dies, find a new player
                            if(playerConfigs[i].IsDead)
                                UpdateTaggedPlayer(Random.Range(0, playerConfigs.Count));
                        }
                    }
                }
            }
          
            
            for (int i = 0; i < playerConfigs.Count; i++)
            {
                //Update who's tagged
                foreach (GameObject owner in pHitBoxes[i].OwnersToIgnore)
                {
                    if (owner == taggedPlayer)
                    {
                        UpdateTaggedPlayer(i);
                        break;
                    }
                }
            }


            if (endCondition.TestCondition())
            {
                //Plugging in 4 will ensure the index is out of range and clean up all icons and speed values
                UpdateTaggedPlayer(4);

                Transform winner = ((LastManStandingEndCondition)endCondition).winner;

                PlayerConfiguration config = winner.GetComponentInChildren<PlayerInputHandler>().playerConfig;
                //Assign point to let the system know who won
                data.AddScoreForTeam(config.PlayerIndex, 1);

                ((PlayerWinUIDisplay)endingUIDisplay).winnerName = GameState.playerNames[config.PlayerIndex];
                EndGame();
            }
        }
       
    }

    void UpdateTaggedPlayer(int tagIndex)
    {
        if(tagIndex < playerConfigs.Count)
        {
            //Keeping looping until a non dead player is found
            if(playerConfigs[tagIndex].IsDead)
            {
                UpdateTaggedPlayer(Random.Range(0, playerConfigs.Count));
                return;
            }
        }
        
        //Updated variables for tagged and non-tagged players
        for(int i = 0; i < playerConfigs.Count; i++)
        {
            PlayerComponentReferences references = playerConfigs[i].PlayerObject.GetComponent<PlayerComponentReferences>();
            SpriteRenderer sr = references.teamIcon.GetComponent<SpriteRenderer>();

            if(i == tagIndex)
            {
                taggedPlayer = playerConfigs[i].PlayerObject;
                sr.sprite = tagIcon;
                pMovement[i].maxSpeed = speedOfTagged;
            }
            else
            {
                sr.sprite = null;
                pMovement[i].maxSpeed = initialSpeed;
            }
        }
    }

    private IEnumerator StartGracePeriod()
    {
        yield return new WaitForSeconds(gracePeriod);
        gracePeriodEnded = true;
    }
}
