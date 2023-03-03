using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagLogic : MinigameLogic
{
    [SerializeField] private Sprite tagIcon;
    [SerializeField] private float startingGracePeriodTime;
    [SerializeField] private float taggedGracePeriodTime;
    [SerializeField] private float speedOfTagged;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float sizeMultiplierOfTaggedHitBox;
 
    private List<PlayerConfiguration> playerConfigs;
    private List<PlayerConfiguration> deadPlayers;
    private List<ProjectileHitbox> pHitBoxes;
    private List<PlayerMovementScript> pMovement;
    private List<GameObject> lightShoveColliders;
    private List<GameObject> heavyShoveColliders;

    private bool initialGracePeriodEnded;
    private bool tagGracePeriodEnded;

    private GameObject taggedPlayer;
    private float initialSpeed;
    private Vector3 initalShoveCollider;

    private float currentTime;

    


    public override void Init()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
        deadPlayers = new List<PlayerConfiguration>();
        pHitBoxes = new List<ProjectileHitbox>();
        pMovement = new List<PlayerMovementScript>();
        lightShoveColliders = new List<GameObject>();
        heavyShoveColliders = new List<GameObject>();

        for(int i = 0; i < playerConfigs.Count; i++)
        {
            pHitBoxes.Add(playerConfigs[i].PlayerObject.GetComponentInChildren<ProjectileHitbox>(true));
            pMovement.Add(playerConfigs[i].PlayerObject.GetComponentInChildren<PlayerMovementScript>(true));
            lightShoveColliders.Add(playerConfigs[i].PlayerObject.transform.Find("AimDirection").Find("LightHitbox").gameObject);
            heavyShoveColliders.Add(playerConfigs[i].PlayerObject.transform.Find("AimDirection").Find("HeavyHitbox").gameObject);
        }

        initialSpeed = pMovement[0].maxSpeed;
        initalShoveCollider = lightShoveColliders[0].transform.localScale;

        UpdateTaggedPlayer(Random.Range(0, playerConfigs.Count));

        initialGracePeriodEnded = false;
        tagGracePeriodEnded = true;
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
            //Only runs the timer when it's needed
            if(initialGracePeriodEnded || !tagGracePeriodEnded)
                currentTime += Time.deltaTime;

            //Prevents tagged player from taking damage at the very beginning of the game
            if (initialGracePeriodEnded)
            {
                
                if (currentTime >= 1.0f && tagGracePeriodEnded)
                {
                    currentTime -= 1.0f;

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

            //Keeps track of the timer for the tagged grace period
            if (currentTime >= taggedGracePeriodTime && tagGracePeriodEnded == false)
            {
                currentTime -= taggedGracePeriodTime;
                tagGracePeriodEnded = true;
            }

            for (int i = 0; i < playerConfigs.Count; i++)
            {
                //Ensures there's no instant tagbacks
                if (tagGracePeriodEnded)
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

        tagGracePeriodEnded = false;

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
                lightShoveColliders[i].transform.localScale *= sizeMultiplierOfTaggedHitBox;
                heavyShoveColliders[i].transform.localScale *= sizeMultiplierOfTaggedHitBox;
                references.fireVFX.gameObject.SetActive(true);
            }
            else
            {
                sr.sprite = null;
                pMovement[i].maxSpeed = initialSpeed;
                lightShoveColliders[i].transform.localScale = initalShoveCollider;
                heavyShoveColliders[i].transform.localScale = initalShoveCollider;
                references.fireVFX.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator StartGracePeriod()
    {
        yield return new WaitForSeconds(startingGracePeriodTime);
        initialGracePeriodEnded = true;
    }
}
