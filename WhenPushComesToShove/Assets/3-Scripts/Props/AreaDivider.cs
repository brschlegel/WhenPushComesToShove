using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO move icon to center of each box
public class AreaDivider : MonoBehaviour
{
    [Header("Values")]
    public float width;
    public float height;
    private float iconWidthThreshold = 2;

    [Header("Prefabs")]
    [SerializeField]
    private Transform areaPrefab;
    [SerializeField]
    private Transform iconPrefab;
    [SerializeField]
    private Transform spacerPrefab;

    [Header("Parts")]
    public List<Transform> dividers;
    [SerializeField]
    private Transform areaParent;
    [SerializeField]
    private Transform iconParent;
    [SerializeField]
    private Transform spacerParent;
    [HideInInspector]
    public List<Transform> areas;
    [HideInInspector]
    public List<RuntimeAnimatorController> iconAnimations;
    [HideInInspector]
    public List<Sprite> iconSprites;
    [SerializeField]
    private List<Color> colors;

    private List<Transform> iconObjects;
    private float barrelYOffset = -.5f;

    public void Start()
    {
        if(iconObjects == null)
        {
            Init();
        }
    }

    public void Init()
    {
        for(int i = 0; i < dividers.Count + 1; i++)
        {
            Transform t = Instantiate(areaPrefab, areaParent).transform;
            t.GetComponent<SpriteRenderer>().color = colors[i];
            areas.Add(t);
        }
        iconObjects = new List<Transform>();
        for(int i = 0; i < iconAnimations.Count; i++)
        {
            Transform t = Instantiate(iconPrefab, areas[i].position, Quaternion.identity, iconParent).transform;
            t.GetComponent<Animator>().runtimeAnimatorController = iconAnimations[i];
            t.GetComponent<SpriteRenderer>().sprite = iconSprites[i];
            iconObjects.Add(t);
            
        }

        //Place spacers
        Debug.Log(transform.position);
        Transform left = Instantiate(spacerPrefab, new Vector2(transform.position.x + (width / 2.0f), transform.position.y + barrelYOffset), Quaternion.identity, transform);
        left.position = new Vector2(transform.position.x + (width / 2.0f), transform.position.y + barrelYOffset);
        Transform right = Instantiate(spacerPrefab, new Vector2(transform.position.x - (width / 2.0f), transform.position.y + barrelYOffset), Quaternion.identity, transform);
        right.position = new Vector2(transform.position.x - (width / 2.0f), transform.position.y + barrelYOffset);

        UpdateAreas();
    }

    private void Update()
    {
        UpdateAreas();
        ClampDividers();
    }

    private void ClampDividers()
    {   
        for(int i = 0; i < dividers.Count; i++)
        {
            dividers[i].position = new Vector2(Mathf.Clamp(dividers[i].position.x, transform.position.x - width /2, transform.position.x + width / 2), transform.position.y + barrelYOffset);
        }
    }

    private void UpdateAreas()
    {
        Vector2 prevPoint;
        Vector2 point;

       
        for (int i = 0; i < areas.Count; i++)
        {
             //Move and scale the areas
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

            areas[i].position = new Vector2(BenMath.Midpoint(point, prevPoint).x, -.5f);

            areas[i].localScale = new Vector3(point.x - prevPoint.x, height, areas[i].localScale.z);

            if (iconAnimations.Count == areas.Count)
            {
                iconObjects[i].gameObject.SetActive(areas[i].localScale.x >= iconWidthThreshold);
                iconObjects[i].position = areas[i].position;
            }

        }




    }

}
