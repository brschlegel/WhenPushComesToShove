using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    //[SerializeField] private ObjectSpawner spawner;
    [SerializeField] private MinigameData data;
    [SerializeField] private int teamIndex = 0;
    [SerializeField] private GameObject confetti;
    [SerializeField] private List<Transform> confettiTransforms = new List<Transform>();
    [SerializeField] private bool objectsUseTeamIndex = false;

    public UnityAction goalScored;

    private void OnEnable()
    {
        goalScored += SpawnConfetti;
    }

    private void OnDisable()
    {
        goalScored -= SpawnConfetti;
    }

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
            if (objectsUseTeamIndex && points.teamIndex != teamIndex)
            {
                return;
            }

            data.AddScoreForTeam(teamIndex, points.pointsToGain);

            collision.transform.parent.gameObject.SetActive(false);
            Destroy(collision.transform.parent.gameObject);

            StartCoroutine(Delay());
        }
    }

    public void SpawnConfetti()
    {
        foreach (Transform t in confettiTransforms)
        {
            Instantiate(confetti, t);
        }
    }

    private IEnumerator Delay()
    {
        //Only used since destroying objects is not immediate so we must wait for a frame.
        yield return new WaitForEndOfFrame();
        goalScored?.Invoke();
    }

}
