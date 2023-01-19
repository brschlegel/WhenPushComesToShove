using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Stopping threshold, listen to selection in modifierselection logic script
public class AreaSelector : MonoBehaviour
{
    private AreaDivider areaDivider;
    [SerializeField]
    private Rigidbody2D picker;
    [SerializeField]
    private float forceAmount;
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
        if( picker.position.x >= PickerBounds.y)
        {
            picker.position = new Vector2 (PickerBounds.x, picker.position.y);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            BeginSelection();
        }
    }

    public void Init()
    {
        areaDivider = GetComponentInChildren<AreaDivider>();
    }

    public void BeginSelection()
    {
        picker.AddForce(new Vector2(forceAmount, 0));
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
