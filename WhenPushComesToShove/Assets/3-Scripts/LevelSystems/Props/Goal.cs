using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    //[SerializeField] private ObjectSpawner spawner;
    [SerializeField] private MinigameData data;
    [SerializeField] private int teamIndex = 0;

    public UnityAction goalScored;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.TryGetComponent<PointDataIndividual>(out PointDataIndividual pointsInd))
        {
            data.AddScoreForTeam(pointsInd.teamIndex, pointsInd.pointsToGain);

            collision.transform.parent.gameObject.SetActive(false);
            Destroy(collision.transform.parent.gameObject);

            goalScored?.Invoke();
            return;
        }

        if (collision.transform.parent.TryGetComponent<PointData>(out PointData points))
        {
            data.AddScoreForTeam(teamIndex, points.pointsToGain);

            collision.transform.parent.gameObject.SetActive(false);
            Destroy(collision.transform.parent.gameObject);

           goalScored?.Invoke();
        }
    }

}
