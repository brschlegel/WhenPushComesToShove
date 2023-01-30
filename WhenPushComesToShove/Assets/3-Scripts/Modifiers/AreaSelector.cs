using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RetrieveIndex(int index);
public class AreaSelector : MonoBehaviour
{
    [HideInInspector]
    public AreaDivider areaDivider;
    [SerializeField]
    private Rigidbody2D picker;
    public event RetrieveIndex onSelection;
    [SerializeField]
    private float forceAmount;
    [SerializeField]
    private float stopThreshold;

    private int runningFrames;
    // Start is called before the first frame update
    void Start()
    {
        if (areaDivider == null)
        {
            Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("x pos: " + picker.position.x + " bounds: " + PickerBounds.y);
        if( picker.position.x >= PickerBounds.y)
        {
            //picker.position = new Vector2 (PickerBounds.x, picker.position.y);
            picker.position = new Vector2 (PickerBounds.x, areaDivider.transform.position.y - areaDivider.height/2 + 1);
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

    public void Init()
    {
        areaDivider = GetComponentInChildren<AreaDivider>();
        picker.position = new Vector2(picker.position.x, areaDivider.transform.position.y - areaDivider.height/2 + 1);
        runningFrames = 0;
    }

    public void BeginSelection()
    {
        picker.AddForce(new Vector2(forceAmount, 0));
        runningFrames = 1;
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
