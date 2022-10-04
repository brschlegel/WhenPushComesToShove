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

    [SerializeField] private int minPlayers = 2;
    [SerializeField] private int maxPlayers = 2;

    public static PlayerConfigManager Instance { get; private set; }

    [HideInInspector] public InitLevel levelInitRef;
    [SerializeField] private RuntimeAnimatorController[] defaultColors = new RuntimeAnimatorController[4];
    [SerializeField] private string[] playerColorNames = new string[4];
    public Material[] playerOutlines = new Material[4];

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
}

//Used to hold information specific to each player
public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
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
