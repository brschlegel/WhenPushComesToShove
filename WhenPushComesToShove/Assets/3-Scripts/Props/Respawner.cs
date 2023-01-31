using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    public float spawnDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> players = GameState.players;
        foreach(Transform t in players)
        {
            if(t.GetComponentInChildren<PlayerHealth>().dead)
            {
                StartCoroutine(RespawnPlayer(t));
            }
        }
    }

   private IEnumerator RespawnPlayer(Transform player)
   {
        PlayerMovementScript movement = player.GetComponentInChildren<PlayerMovementScript>();
        PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
        PlayerAnimBrain anim = player.GetComponentInChildren<PlayerAnimBrain>();
        yield return new WaitUntil(() => anim.CurrentState.id != "death");
        player.position = transform.position;
        movement.ForceLockMovement();
        yield return new WaitForSeconds(spawnDelay);
        health.ResetHealth();
        movement.ForceUnlockMovement();
   }

}
