using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inits the level based on player data
public class InitLevel : MonoBehaviour
{
    public Transform[] playerSpawns;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool initOnStart = true;

    [HideInInspector] public bool lockPlayerSpawn = false;

    [SerializeField] private bool spawnPlayerUI = false;

    [SerializeField] private ParticleSystem[] playerSpawnAnim = new ParticleSystem[4];

    public GameObject[] playerUI= new GameObject[4];
    public Color[] playerHitboxColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerConfigManager.Instance.levelInitRef == null)
        {
            PlayerConfigManager.Instance.levelInitRef = this;
        }

        LevelManager.onEndGame += UnlockPlayerSpawn;

    }

    /// <summary>
    /// Used if players are not spawned on start
    /// </summary>
    public void SpawnPlayer(int index)
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        GameObject player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        GameState.players.Add(player.transform);
        player.GetComponentInChildren<PlayerInputHandler>().InitializePlayer(playerConfigs[index]);
        Instantiate(playerSpawnAnim[index], playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);

        if (spawnPlayerUI)
        {
            //Assign and enable UI
            GameObject UI = player.GetComponentInChildren<PlayerInventory>().UIRef = playerUI[index];
            UI.SetActive(true);
        }

        //Assign Hitbox Colors
        Hitbox[] hitboxes = player.GetComponentsInChildren<Hitbox>();

        foreach (Hitbox h in hitboxes)
        {
            h.gameObject.GetComponent<SpriteRenderer>().color = playerHitboxColors[index];
        }

        //Assign Ground UI Colors
        SpriteRenderer[] srs = player.transform.GetChild(9).GetComponentsInChildren<SpriteRenderer>();

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

        if(levelProps.levelType != LevelType.Arena)
        {
            PlayerConfigManager.Instance.RandomizeTeam();
            PlayerConfiguration[] playerConfigs = PlayerConfigManager.Instance.GetPlayerTeams().ToArray();

            PlayerSpawnProps[] spawnProps = new PlayerSpawnProps[4];
            for (int i = 0; i < spawnProps.Length; i++)
            {
                spawnProps[i] = new PlayerSpawnProps();
                spawnProps[i].transform = playerSpawns[i].transform;
                spawnProps[i].teamIndex = playerSpawns[i].transform.GetComponent<PlayerSpawnPoint>().teamIndex;
                Debug.Log(spawnProps[i].teamIndex);
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
                        Debug.Log("Spawn Index: " + spawnProps[j].teamIndex + " Player Index: " + playerConfigs[i].TeamIndex);
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
