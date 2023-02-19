using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagLogic : MinigameLogic
{
    private List<PlayerConfiguration> playerConfigs;
    private List<ProjectileHitbox> pHitBoxes;
    private GameObject taggedPlayer;
    private float currentTime;

    [SerializeField] private Sprite tagIcon;
    [SerializeField] private float timeScoreIncrement;


    public override void Init()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
        pHitBoxes = new List<ProjectileHitbox>();

        for(int i = 0; i < playerConfigs.Count; i++)
        {
            pHitBoxes.Add(playerConfigs[i].PlayerObject.GetComponentInChildren<ProjectileHitbox>(true));
        }

        UpdateTaggedPlayer(Random.Range(0, playerConfigs.Count));

        currentTime = 0.0f;

        base.Init();
    }
    // Update is called once per frame
    void Update()
    {

        if (gameRunning)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= timeScoreIncrement)
            {
                currentTime -= timeScoreIncrement;

                for (int i = 0; i < playerConfigs.Count; i++)
                {
                    if (playerConfigs[i].PlayerObject != taggedPlayer)
                        data.AddScoreForTeam(i, 1);
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
                int max = Mathf.Max(data.scores);
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
            }
            else
            {
                sr.sprite = null;
            }
        }
    }
}
