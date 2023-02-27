using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagLogic : MinigameLogic
{
    [SerializeField] private Sprite tagIcon;
    [SerializeField] private float timeScoreIncrement;
    [SerializeField] private float gracePeriod;
    [SerializeField] private float speeOfTagged;

    private List<PlayerConfiguration> playerConfigs;
    private List<ProjectileHitbox> pHitBoxes;
    private List<PlayerMovementScript> pMovement;

    private bool gracePeriodEnded;

    private GameObject taggedPlayer;
    private float initialSpeed;

    private float currentTime;

    


    public override void Init()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
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
            if (gracePeriodEnded)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= timeScoreIncrement)
                {
                    currentTime -= timeScoreIncrement;

                    for (int i = 0; i < playerConfigs.Count; i++)
                    {
                        if (playerConfigs[i].PlayerObject != taggedPlayer)
                            data.AddScoreForTeam(i, 1);
                    }
                }
            }
          
            //Update who's tagged
            for (int i = 0; i < playerConfigs.Count; i++)
            {
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
                float max = Mathf.Max(data.scores);
                int numOfWinners = 0;
                string winnerName = "";
                PlayerWinUIDisplay winDisplay = ((PlayerWinUIDisplay)endingUIDisplay);

                for (int i = 0; i < data.scores.Length; i++)
                {
                    if (max == data.scores[i])
                    {
                        numOfWinners++;

                        if (numOfWinners == 1)
                        {
                            winnerName = GameState.playerNames[i];
                        }
                        else
                        {
                            winDisplay.tie = true;
                            winnerName += " player and " + GameState.playerNames[i];
                        }

                    }
                }

                //Transform winner = ((TimerEndCondition)endCondition).winner;
                winDisplay.winnerName = winnerName;
                EndGame();
            }

        }
       
    }

    public override void CleanUp()
    {
        //Plugging in 4 will ensure the index is out of range and clean up all icons
        UpdateTaggedPlayer(4);
        base.CleanUp();
    }
    void UpdateTaggedPlayer(int tagIndex)
    {

        for(int i = 0; i < playerConfigs.Count; i++)
        {
            PlayerComponentReferences references = playerConfigs[i].PlayerObject.GetComponent<PlayerComponentReferences>();
            SpriteRenderer sr = references.teamIcon.GetComponent<SpriteRenderer>();

            if(i == tagIndex)
            {
                taggedPlayer = playerConfigs[i].PlayerObject;
                sr.sprite = tagIcon;
                pMovement[i].maxSpeed = speeOfTagged;
                //pMovement[i].acceleration = speeOfTagged * 10;
            }
            else
            {
                sr.sprite = null;
                pMovement[i].maxSpeed = initialSpeed;
                //pMovement[i].acceleration = initialAcceleration;
            }
        }
    }

    private IEnumerator StartGracePeriod()
    {
        yield return new WaitForSeconds(gracePeriod);
        gracePeriodEnded = true;
    }
}
