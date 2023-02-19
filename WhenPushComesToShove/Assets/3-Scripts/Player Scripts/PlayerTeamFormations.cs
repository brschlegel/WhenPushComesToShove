using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamFormations : MonoBehaviour
{
    public static PlayerTeamFormations instance;

    private List<PlayerConfiguration> playerConfigs;
    public List<PlayerConfiguration> playerTeamOrder;

    public Sprite[] teamSprites = new Sprite[2];

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
            playerTeamOrder = new List<PlayerConfiguration>();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeTeam();
        }
    }

    public List<PlayerConfiguration> GetPlayerTeams()
    {
        return playerTeamOrder;
    }

    public void RandomizeTeam()
    {
        playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();

        playerTeamOrder.Clear();

        for (int i = 0; i < playerConfigs.Count; i++)
        {
            playerTeamOrder.Add(playerConfigs[i]);
        }

        //Shuffle List if Teams
        if (GameState.currentRoomType != LevelType.Arena)
        {
            for (int i = 0; i < playerTeamOrder.Count; i++)
            {
                PlayerConfiguration temp = playerTeamOrder[i];
                int index = Random.Range(i, playerTeamOrder.Count);
                playerTeamOrder[i] = playerTeamOrder[index];
                playerTeamOrder[index] = temp;
            }
        }

        //Assign Teams
        switch (GameState.currentRoomType)
        {
            case LevelType.Dungeon:
                break;
            case LevelType.Arena:
                for (int i = 0; i < playerConfigs.Count; i++)
                {
                    playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = i;
                    playerTeamOrder[i].TeamIndex = i;
                }
                break;
            case LevelType.TwoTwo:
                for (int i = 0; i < playerConfigs.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = 0;
                        playerTeamOrder[i].TeamIndex = 0;
                    }
                    else
                    {
                        playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = 1;
                        playerTeamOrder[i].TeamIndex = 1;
                    }
                }
                break;
            case LevelType.ThreeOne:
                for (int i = 0; i < playerConfigs.Count; i++)
                {
                    if (i != 1)
                    {
                        playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = 0;
                        playerTeamOrder[i].TeamIndex = 0;
                    }
                    else
                    {
                        playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = 1;
                        playerTeamOrder[i].TeamIndex = 1;
                    }
                }
                break;
            default:
                {
                    for (int i = 0; i < playerConfigs.Count; i++)
                    {
                        playerConfigs[playerTeamOrder[i].PlayerIndex].TeamIndex = i;
                        playerTeamOrder[i].TeamIndex = i;
                    }
                }
                break;
        }

        //Assign Outlines
        //if (GameState.currentRoomType != LevelType.Arena)
        //{
        //    foreach (PlayerConfiguration p in playerConfigs)
        //    {
        //        p.Outline.SetColor("_PlayerColor", teamOutlineColors[p.TeamIndex]);
        //        p.PlayerObject.GetComponent<SpriteRenderer>().material = p.Outline;
        //    }
        //}
        //else
        //{
        //    foreach (PlayerConfiguration p in playerConfigs)
        //    {
        //        p.Outline.SetColor("_PlayerColor", playerOutlineOriginalColors[p.PlayerIndex]);
        //        p.PlayerObject.GetComponent<SpriteRenderer>().material = p.Outline;
        //    }
        //}

        //Assign Number Symbols
        if (GameState.currentRoomType != LevelType.Arena && GameState.currentRoomType != LevelType.Lobby && GameState.currentRoomType != LevelType.Modifier)
        {
            foreach (PlayerConfiguration p in playerConfigs)
            {
                PlayerComponentReferences references = p.PlayerObject.GetComponent<PlayerComponentReferences>();
                SpriteRenderer sr = references.teamIcon.GetComponent<SpriteRenderer>();
                sr.sprite = teamSprites[p.TeamIndex];
            }
        }
        else
        {
            foreach (PlayerConfiguration p in playerConfigs)
            {
                PlayerComponentReferences references = p.PlayerObject.GetComponent<PlayerComponentReferences>();
                SpriteRenderer sr = references.teamIcon.GetComponent<SpriteRenderer>();
                sr.sprite = null;
            }
        }

    }
}
