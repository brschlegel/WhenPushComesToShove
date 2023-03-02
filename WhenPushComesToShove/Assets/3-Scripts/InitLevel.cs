using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inits the level based on player data
public class InitLevel : MonoBehaviour
{
    public Transform[] playerSpawns;
    public GameObject[] playerUI = new GameObject[4];
    public Color[] playerHitboxColors = new Color[4];

    [HideInInspector] public bool lockPlayerSpawn = false;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool initOnStart = true;
    [SerializeField] private bool spawnPlayerUI = false;
    [SerializeField] private ParticleSystem[] playerSpawnAnim = new ParticleSystem[4];
    //[SerializeField] private Color[] playerCircleVFXColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerConfigManager.Instance.levelInitRef == null)
        {
            PlayerConfigManager.Instance.levelInitRef = this;
        }

        LevelManager.onEndGame += UnlockPlayerSpawn;

    }

    private void Update()
    {
        //Keep Cursor disabled
        Cursor.visible = false;
    }

    /// <summary>
    /// Used if players are not spawned on start
    /// </summary>
    public void SpawnPlayer(int index)
    {
        //Create Player
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        GameObject player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        GameState.players.Add(player.transform);

        //Find health bar and add it to GameState
        HealthBar bar = player.GetComponentInChildren<HealthBar>();
        GameState.playerHealthBars.Add(bar);
        bar.gameObject.SetActive(false);

        PlayerComponentReferences references = player.GetComponent<PlayerComponentReferences>();
        PlayerInputHandler handler = player.GetComponentInChildren<PlayerInputHandler>();

        //Intialize Player and Circle VFX
        handler.InitializePlayer(playerConfigs[index]);
        //var main = references.circleVFX.main;
        //main.startColor = playerCircleVFXColors[index];
        Instantiate(playerSpawnAnim[index], playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);

        //Assign Ground UI Colors
        SpriteRenderer[] srs = references.GroundUIRef.GetComponentsInChildren<SpriteRenderer>();

        //Assign Confetti      
        references.confettiVFX = PlayerConfigManager.Instance.playerConfettiPrefabs[index];

        foreach (SpriteRenderer sr in srs)
        {
            sr.color = playerHitboxColors[index];
        }

        player.GetComponentInChildren<PlayerMovementScript>().LockMovementForTime(.8f);

        playerConfigs[index].PlayerObject = player;
    }

    public void SpawnPlayersInLevel(LevelProperties levelProps)
    {
        //Spawn the players in their set locations for the level.

        //Handle team formation based on room type
        PlayerTeamFormations.instance.RandomizeTeam();

        //Spawn Players for team games
        if (levelProps.levelType != LevelType.Arena && levelProps.levelType != LevelType.Lobby)
        {
            PlayerConfiguration[] playerConfigs = PlayerTeamFormations.instance.GetPlayerTeams().ToArray();

            PlayerSpawnProps[] spawnProps = new PlayerSpawnProps[4];
            for (int i = 0; i < spawnProps.Length; i++)
            {
                spawnProps[i] = new PlayerSpawnProps();
                spawnProps[i].transform = playerSpawns[i].transform;
                spawnProps[i].teamIndex = playerSpawns[i].transform.GetComponent<PlayerSpawnPoint>().teamIndex;
                spawnProps[i].filled = false;
            }

            for (int i = 0; i < playerConfigs.Length; i++)
            {

                if (playerConfigs[i].IsDead)
                {
                    playerConfigs[i].PlayerObject.GetComponentInChildren<PlayerHealth>().ResetHealth();
                    playerConfigs[i].IsDead = false;
                }

                //If a team level, will arrange the players to spawn in the correct team spawn points without spawning on top of each other
                if (levelProps.teamLevel)
                {
                    for (int j = 0; j < spawnProps.Length; j++)
                    {

                        if (spawnProps[j].filled == false && spawnProps[j].teamIndex == playerConfigs[i].TeamIndex)
                        {
                            spawnProps[j].filled = true;
                            playerConfigs[i].PlayerObject.transform.position = spawnProps[j].transform.position;
                            break;
                        }
                    }

                }
                else
                    playerConfigs[i].PlayerObject.transform.position = playerSpawns[i].transform.position;
            }

            spawnProps = null;
        }
        //Spawn players for Arena games
        else
        {
            PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
            for(int i = 0; i < playerConfigs.Length; i++)
            {
                playerConfigs[i].PlayerObject.transform.position = playerSpawns[i].transform.position;
            }
        }
       
    }

    /// <summary>
    /// Helper Function to Lock Player Spawn 
    /// </summary>
    public void LockPlayerSpawn()
    {
        lockPlayerSpawn = true;
    }

    /// <summary>
    /// Helper Function to Unlock Player Spawn If Possible
    /// </summary>
    public void UnlockPlayerSpawn()
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

        if (playerConfigs.Length < PlayerConfigManager.Instance.GetMaxPlayers())
        {
            lockPlayerSpawn = false;
        }
    }

}
