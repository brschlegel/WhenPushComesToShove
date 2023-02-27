using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct to grab all the parts of the divider
public struct DividerParents
{
    public Transform areaParent;
    public Transform barrelParent;
    public Transform iconParent;
    public DividerParents(Transform areaParent, Transform barrelParent, Transform iconParent)
    {
        this.areaParent = areaParent;
        this.barrelParent = barrelParent;
        this.iconParent = iconParent;
    }
}
//Divides a rectangle into multiple column areas. Used for the selector. Tried to make this more general, probably was a bad call
public class AreaDivider : MonoBehaviour
{
    [Header("Values")]
    public float width;
    public float height;
    private float iconWidthThreshold = 2;
    public int numberOfAreas = 3;

    [HideInInspector]
    public List<Transform> areas;


    [Header("Prefabs")]
    [SerializeField]
    private Transform areaPrefab;
    [SerializeField]
    private Transform iconPrefab;
    [SerializeField]
    private Transform spacerPrefab;
    [SerializeField]
    private Transform dividerPrefab;

    [Header("Parents")]
    [SerializeField]
    private Transform areaParent;
    [SerializeField]
    private Transform iconParent;
    [SerializeField]
    private Transform barrelParent;
    [SerializeField]
    private List<Color> colors;

    //Icon information, set by init
    private List<RuntimeAnimatorController> iconAnimations;
    private List<Sprite> iconSprites;
    //Parts
    private List<Transform> iconObjects;
    private List<Transform> spacers;
    private List<Transform> dividers;
    
    private float barrelYOffset = -.5f;

    public DividerParents Parts
    {
        get {return new DividerParents(areaParent, barrelParent, iconParent);}
    }

    public void Init(List<Sprite> iconSprites, List<RuntimeAnimatorController> iconAnimations)
    {
        this.iconSprites = iconSprites;
        this.iconAnimations = iconAnimations;
        for(int i = 0; i < numberOfAreas; i++)
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
        spacers = new List<Transform>();
        dividers = new List<Transform>();

        //List of points for the spacers and dividers. First and last element is for outside spacers, middle is for the dividers
        List<Vector2> points = BenMath.GetEquidistantPointsOnLine(new Vector2(transform.position.x - (width / 2.0f), transform.position.y + barrelYOffset), new Vector2(transform.position.x + (width/2.0f), transform.position.y + barrelYOffset), numberOfAreas + 1);
        //Spawn Outside Spacers
        Transform left = Instantiate(spacerPrefab, barrelParent);
        left.position = points[0];
        spacers.Add(left);
        Transform right = Instantiate(spacerPrefab,barrelParent);
        right.position = points[points.Count - 1];
        spacers.Add(right);

        //Spawn Inside Dividers
        for(int i = 0; i < numberOfAreas - 1; i++)
        {
            Transform divider = Instantiate(dividerPrefab, barrelParent);
            divider.position = points[i + 1];
            dividers.Add(divider);
        }
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
                //iconObjects[i].gameObject.SetActive(areas[i].localScale.x >= iconWidthThreshold);
                iconObjects[i].position = areas[i].position;
            }

        }




    }

}
