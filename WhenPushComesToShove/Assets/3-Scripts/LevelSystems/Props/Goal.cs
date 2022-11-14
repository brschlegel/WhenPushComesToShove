using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private ObjectSpawner spawner;
    [SerializeField] private MinigameData data;
    [SerializeField] private int teamIndex = 0;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            Destroy(collision.gameObject);

            data.AddScoreForTeam(teamIndex, 1);

            //Trigger new ball spawn
            spawner.Spawn();
        }
    }
}
