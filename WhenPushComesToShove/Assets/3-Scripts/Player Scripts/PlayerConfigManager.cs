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
    [SerializeField] private Material[] defaultColors = new Material[4];

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

    /// <summary>
    /// Helper function to set a player's color
    /// </summary>
    /// <param name="index">The player's index</param>
    /// <param name="color">The material to assign to the spriteRenderer</param>
    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    }

    /// <summary>
    /// Helper function to signal that a player is ready to move to the next room
    /// </summary>
    /// <param name="index">The player's index</param>
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        //Check if all players are ready and move to the next level if so
        if (playerConfigs.Count >= minPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("DestinyScene");
        }
    }

    /// <summary>
    /// Helper function to back a player out of being ready to move to the next room
    /// </summary>
    /// <param name="index"> The player's index</param>
    public void UnreadyPlayer( int index )
    {
        playerConfigs[index].IsReady = false;
    }

    /// <summary>
    /// Handles a player joining at the start of the game
    /// </summary>
    /// <param name="input">The input to assign to the player</param>
    public void HandlePlayerJoin(PlayerInput input)
    {
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
            SetPlayerColor(input.playerIndex, defaultColors[input.playerIndex]);

            levelInitRef.SpawnPlayer(input.playerIndex);
        }
    }
}

//Used to hold information specific to each player
public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }

    public PlayerConfiguration(PlayerInput input)
    {
        PlayerIndex = input.playerIndex;
        Input = input;
    }
}
