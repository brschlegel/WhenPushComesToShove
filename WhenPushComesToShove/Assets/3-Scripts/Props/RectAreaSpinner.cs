using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO move icon to center of each box
public class RectAreaSpinner : MonoBehaviour
{
    public float width;
    public float height;
    public List<Transform> dividers;
    [SerializeField]
    private Transform areaParent;
    [SerializeField]
    private Transform areaPrefab;
    [SerializeField]
    private List<Transform> areas;
    [SerializeField]
    private List<Color> colors;

    public void Start()
    {
        for(int i = 0; i < dividers.Count + 1; i++)
        {
            Transform t = Instantiate(areaPrefab, areaParent).transform;
            t.GetComponent<SpriteRenderer>().color = colors[i];
            areas.Add(t);
        }
        UpdateSpinner();
    }

    private void Update()
    {
        UpdateSpinner();
        ClampDividers();
    }

    private void ClampDividers()
    {   
        for(int i = 0; i < dividers.Count; i++)
        {
            dividers[i].position = new Vector2(Mathf.Clamp(dividers[i].position.x, transform.position.x - width /2, transform.position.x + width / 2), dividers[i].position.y);
        }
    }

    private void UpdateSpinner()
    {
        Vector2 prevPoint;
        Vector2 point;

        for (int i = 0; i < areas.Count; i++)
        {
            if (i == 0)
            {
                prevPoint = (Vector2)transform.position - new Vector2(width / 2, 0);
            }
            else
            {
                prevPoint = dividers[i - 1].position;
            }
            if (i == areas.Count - 1)
            {
                point = (Vector2)transform.position + new Vector2(width / 2, 0);
            }
            else
            {
                point = dividers[i].position;
            }

            areas[i].position = new Vector2(BenMath.Midpoint(point, prevPoint).x, 0);
            
            areas[i].localScale = new Vector3(point.x - prevPoint.x,height, areas[i].localScale.z);

        }
        foreach(Transform t in dividers)
        {
            Vector2 pointPos = t.position;
        }
    }

}
