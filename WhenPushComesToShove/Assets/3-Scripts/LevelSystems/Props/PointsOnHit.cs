using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOnHit : MonoBehaviour
{
    //[SerializeField] private ObjectSpawner spawner;
    [SerializeField] private MinigameData data;
    [SerializeField] private int teamIndex = 0;
    [SerializeField] private PointData pointData;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent<PointDataIndividual>(out PointDataIndividual pointsInd))
        {
            data.AddScoreForTeam(pointsInd.teamIndex, pointData.pointsToGain);
            return;
        }

        if (collision.transform.parent.TryGetComponent<PointData>(out PointData points))
        {
            data.AddScoreForTeam(teamIndex, pointData.pointsToGain);
        }
    }
}
