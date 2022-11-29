using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointData))]
public class ChangePointsOnEnable : MonoBehaviour
{
    [SerializeField] private int chanceForChange = 10;
    [SerializeField] private int newPointValue = 3;
    [SerializeField] private Color newColor;

    private void OnEnable()
    {
        int rand = Random.Range(0, 100);
        if (rand <= chanceForChange)
        {
            ChangePoints();
        }
    }

    private void ChangePoints()
    {
        PointData data = GetComponent<PointData>();

        data.pointsToGain = newPointValue;

        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.color = newColor;
        }
    }
}
