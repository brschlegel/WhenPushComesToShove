using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public float spawnDelay = 1.0f;
    public event PlayerEvent onDetectDeath;
    public event PlayerEvent onRespawn;

    private Dictionary<Transform, bool> respawning;
    private float randomOffsetRadius = 3;
    // Start is called before the first frame update
    void Start()
    {
        respawning = new Dictionary<Transform, bool>();
        foreach(Transform t in GameState.players)
        {
            respawning.Add(t, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> players = GameState.players;
        foreach(Transform t in players)
        {
            if(t.GetComponentInChildren<PlayerHealth>().dead && !respawning[t])
            {
                onDetectDeath?.Invoke(t.GetComponentInChildren<PlayerInputHandler>().playerConfig.PlayerIndex);
                StartCoroutine(RespawnPlayer(t));
            }
        }
    }

   private IEnumerator RespawnPlayer(Transform player)
   {
        respawning[player] =true;
        PlayerMovementScript movement = player.GetComponentInChildren<PlayerMovementScript>();
        PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
        PlayerAnimBrain anim = player.GetComponentInChildren<PlayerAnimBrain>();
        PlayerInputHandler input = player.GetComponentInChildren<PlayerInputHandler>();

        //Wait until animation has stopped playing
        yield return new WaitUntil(() => anim.CurrentState.id != "death");
        yield return new WaitForSeconds(spawnDelay);
        player.position = transform.position + (Vector3)Random.insideUnitCircle * randomOffsetRadius;
        health.ResetHealth();
        respawning[player] = false;
        onRespawn?.Invoke(input.playerConfig.PlayerIndex);
   }

}
