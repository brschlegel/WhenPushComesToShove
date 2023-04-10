using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerSound : MonoBehaviour
{
    private int currentPoint;
    public GameObject areaDivider;
    private float dividerWidth;
    private List<Vector2> points;
    public int tickPoints = 4;

    // Start is called before the first frame update
    void Start()
    {
        dividerWidth = areaDivider.GetComponent<AreaDivider>().width;
        points = BenMath.GetEquidistantPointsOnLine(new Vector2(areaDivider.transform.position.x - (dividerWidth / 2.0f), 0), new Vector2(areaDivider.transform.position.x + (dividerWidth / 2.0f), 0), tickPoints);

        currentPoint = (points.Count / 2);
    }

    private void FixedUpdate()
    {
        if (points.Count <= 0) return;

        if(gameObject.transform.position.x > points[currentPoint].x)
        {
            if(currentPoint != 0)
                AudioManager.instance.PlayOneShot(FMODEvents.instance.spinner);

            currentPoint++;
        }

        if(currentPoint == tickPoints)
        {
            currentPoint = 0;
        }
    }
}
