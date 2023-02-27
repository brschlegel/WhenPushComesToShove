using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RetrieveIndex(int index);
public class AreaSelector : MonoBehaviour
{
    public event RetrieveIndex onSelection;
    [SerializeField]
    private Rigidbody2D picker;
    [SerializeField]
    private float forceMin;
    [SerializeField]
    private float forceMax;
    [SerializeField]
    private float stopThreshold;

    private int runningFrames;
    private AreaDivider areaDivider;
    private DividerParents dividerParents;

    private float elementIntroDelay = 1;

    public void Init(List<Sprite> iconSprites, List<RuntimeAnimatorController> controllers)
    {
        areaDivider = GetComponentInChildren<AreaDivider>();
        areaDivider.Init(iconSprites, controllers);
        dividerParents = areaDivider.Parts;
        //Set all parts disabled for introduction
        for(int i = 0; i < dividerParents.iconParent.childCount; i++ )
        {
            dividerParents.iconParent.GetChild(i).gameObject.SetActive(false);
            dividerParents.areaParent.GetChild(i).gameObject.SetActive(false);
        }
        dividerParents.barrelParent.gameObject.SetActive(false);
        picker.gameObject.SetActive(false);

        picker.position = new Vector2(picker.position.x, areaDivider.transform.position.y - areaDivider.height/3);
        runningFrames = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if( picker.position.x >= PickerBounds.y)
        {
            //picker.position = new Vector2 (PickerBounds.x, picker.position.y);
            picker.position = new Vector2 (PickerBounds.x, areaDivider.transform.position.y - areaDivider.height/3);
        }

        //Have to use a count otherwise the velocity will be zero on the first frame after beginning selection
        if(runningFrames > 10)
        {
            if(Mathf.Abs(picker.velocity.x) <= stopThreshold)
            {
               MakeSelection();
            }
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            BeginSelection();
        }

        if(runningFrames > 0)
        {
            runningFrames++;
        }

    }

    public void BeginSelection()
    {
        picker.AddForce(new Vector2(Random.Range(forceMin, forceMax), 0));
        runningFrames = 1;
    }

    public IEnumerator Introduction()
    {
        yield return new WaitForSeconds(elementIntroDelay);

        //Areas and icons

        for(int i = 0; i < dividerParents.iconParent.childCount; i++ )
        {
            dividerParents.iconParent.GetChild(i).gameObject.SetActive(true);
            dividerParents.areaParent.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(2 *elementIntroDelay / dividerParents.iconParent.childCount);
        }

        //Barrels
        dividerParents.barrelParent.gameObject.SetActive(true);
        yield return new WaitForSeconds(elementIntroDelay);

        //Picker
        picker.gameObject.SetActive(true);
        yield return new WaitForSeconds(elementIntroDelay);
        
        //Make Selection
        BeginSelection();
    }


    private void MakeSelection()
    {
        //Stop the picker and make a selection
        picker.velocity = Vector2.zero;
        for (int i = 0; i < areaDivider.areas.Count; i++)
        {
            //If the picker is in this area
            if (areaDivider.areas[i].GetComponent<Area>().objectsInArea.Contains(picker.transform))
            {
                onSelection?.Invoke(i);
                runningFrames = 0;
                return;
            }
        }
    }

    public Vector2 PickerBounds
    {
        get 
        {
            List<Transform> areas = areaDivider.areas;
            float min = areas[0].position.x - areas[0].localScale.x / 2;
            float max = areas[areas.Count - 1].position.x + areas[areas.Count - 1].localScale.x / 2;
            return new Vector2(min, max);
        }
    }

}
