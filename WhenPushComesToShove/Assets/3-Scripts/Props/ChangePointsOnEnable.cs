using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PointData))]
public class ChangePointsOnEnable : MonoBehaviour
{
    [SerializeField] private int chanceForChange = 10;
    [SerializeField] private int newPointValue = 3;
    [SerializeField] private Color newColor;
    [SerializeField] private TextMeshPro textToEnable;

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
        textToEnable.gameObject.SetActive(true);

        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.color = newColor;
        }
    }
}
