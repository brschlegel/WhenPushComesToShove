using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //[SerializeField] private ObjectSpawner spawner;
    [SerializeField] private MinigameData data;
    [SerializeField] private int teamIndex = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent<PointData>(out PointData points))
        {
            data.AddScoreForTeam(teamIndex, points.pointsToGain);

            collision.transform.parent.gameObject.SetActive(false);
            Destroy(collision.transform.parent.gameObject);

            //Trigger new ball spawn
            ObjectSpawner spawner = collision.transform.parent.GetComponentInParent<ObjectSpawner>();
            spawner.Spawn();
        }
    }

}
