using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//A manager that track general information about the player group and each player's individual configurations
public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    public List<PlayerConfiguration> playerTeamOrder;

    [SerializeField] private int minPlayers = 2;
    [SerializeField] private int maxPlayers = 2;

    public static PlayerConfigManager Instance { get; private set; }

    [HideInInspector] public InitLevel levelInitRef;
    [SerializeField] private RuntimeAnimatorController[] defaultColors = new RuntimeAnimatorController[4];
    [SerializeField] private string[] playerColorNames = new string[4];
    public Material[] playerOutlines = new Material[4];
    public Color[] playerOutlineOriginalColors = new Color[4];
    //public Color[] teamOutlineColors = new Color[2];
    public Sprite[] teamSprites = new Sprite[2];

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Instance != null)
        {
            Debug.Log("An instance of PlayerConfigManager already exists");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
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

    /// <summary>
    /// Helper function to return all the player's configurations
    /// </summary>
    /// <returns></returns>
    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public List<PlayerConfiguration> GetPlayerTeams()
    {
        return playerTeamOrder;
    }

    public int GetMaxPlayers()
    {
        return maxPlayers;
    }

    public int GetMinPlayer()
    {
        return minPlayers;
    }

    /// <summary>
    /// Helper function to set a player's color
    /// </summary>
    /// <param name="index">The player's index</param>
    /// <param name="color">The material to assign to the spriteRenderer</param>
    public void SetPlayerColor(int index, RuntimeAnimatorController color, Material mat, string name)
    {
        playerConfigs[index].PlayerAnimations = color;
        mat.SetColor("_PlayerColor", playerOutlineOriginalColors[index]);
        playerConfigs[index].Outline = mat;
        playerConfigs[index].PlayerColorName = name;
    }

    /// <summary>
    /// Handles a player joining at the start of the game
    /// </summary>
    /// <param name="input">The input to assign to the player</param>
    public void HandlePlayerJoin(PlayerInput input)
    {
        if (levelInitRef.lockPlayerSpawn)
        {
            return;
        }

        Debug.Log("Player joined " + input.playerIndex);

        if (!playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
        {
            input.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(input));
        }

        //Spawn in lobby if possible
        if (levelInitRef != null)
        {
            //Set to a default color
            SetPlayerColor(input.playerIndex, defaultColors[input.playerIndex], playerOutlines[input.playerIndex], playerColorNames[input.playerIndex]);
            //playerOutlineOriginalColors[input.playerIndex] = playerOutlines[input.playerIndex].GetColor("_PlayerColor");

            levelInitRef.SpawnPlayer(input.playerIndex);
        }
    }

    public bool CheckAllPlayerDeath()
    {
        foreach (PlayerConfiguration config in playerConfigs)
        {
            if (!config.IsDead)
            {
                return false;
            }
        }

        return true;
    }

    public void RandomizeTeam()
    {

        Debug.Log(GameState.currentRoomType);
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

            foreach (PlayerConfiguration p in playerTeamOrder)
            {
                //Debug.Log(p.PlayerIndex);
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
                Debug.Log("TwoTwo");
                for (int i = 0; i < playerConfigs.Count; i++)
                {
                    if (i<2)
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
                    if (i < 3)
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
        if (GameState.currentRoomType != LevelType.Arena)
        {
            foreach (PlayerConfiguration p in playerConfigs)
            {
                SpriteRenderer sr = p.PlayerObject.transform.GetChild(12).GetComponent<SpriteRenderer>();
                sr.sprite = teamSprites[p.TeamIndex];
            }
        }
        else
        {
            foreach (PlayerConfiguration p in playerConfigs)
            {
                SpriteRenderer sr = p.PlayerObject.transform.GetChild(12).GetComponent<SpriteRenderer>();
                sr.sprite = null;
            }
        }

    }
}

//Used to hold information specific to each player
public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public int TeamIndex { get; set; }
    public string PlayerColorName { get; set; }
    public bool IsDead { get; set; }
    public RuntimeAnimatorController PlayerAnimations { get; set; }

    public Material Outline { get; set; }

    public GameObject PlayerObject;

    public PlayerConfiguration(PlayerInput input)
    {
        PlayerIndex = input.playerIndex;
        Input = input;
    }
}
