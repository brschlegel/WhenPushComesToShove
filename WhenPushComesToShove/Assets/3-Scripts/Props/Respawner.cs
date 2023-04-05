using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public float spawnDelay = 1.0f;
    public event PlayerEvent onDetectDeath;
    public event PlayerEvent onRespawn;
    public float randomOffsetRadius = 3;

    [SerializeField] private ParticleSystem[] playerSpawnAnim = new ParticleSystem[4];

    [HideInInspector]
    public List<int> indicesToIgnore;

    private Dictionary<Transform, bool> respawning;
    // Start is called before the first frame update
    void Start()
    {
        if(respawning == null)
        {
            Init();
        }
    }

    public void Init()
    {
        respawning = new Dictionary<Transform, bool>();
        indicesToIgnore = new List<int>();
        foreach (Transform t in GameState.players)
        {
            respawning.Add(t, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> players = GameState.players;
        foreach (Transform t in players)
        {
            if (t.GetComponentInChildren<PlayerHealth>().dead && !respawning[t])
            {
                int index = t.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex;
                if (!indicesToIgnore.Contains(index))
                {
                    onDetectDeath?.Invoke(index);
                    StartCoroutine(RespawnPlayer(t));
                }
            }
        }
    }

    private IEnumerator RespawnPlayer(Transform player)
    {
        respawning[player] = true;
        PlayerMovementScript movement = player.GetComponentInChildren<PlayerMovementScript>();
        PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
        PlayerAnimBrain anim = player.GetComponentInChildren<PlayerAnimBrain>();
        PlayerInputHandler input = player.GetComponentInChildren<PlayerInputHandler>();

        //Wait until animation has stopped playing
        yield return new WaitUntil(() => anim.CurrentState.id != "death");
        player.position = transform.position + (Vector3)Random.insideUnitCircle * randomOffsetRadius;
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerSpawnAnim[input.playerConfig.PlayerIndex], player.position, player.rotation);
        health.ResetHealth();
        respawning[player] = false;
        onRespawn?.Invoke(input.playerConfig.PlayerIndex);
    }

}
