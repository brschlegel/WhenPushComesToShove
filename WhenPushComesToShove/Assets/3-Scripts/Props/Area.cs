using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Area : MonoBehaviour
{
    public List<string> tagsToInclude;
    public List<Transform> objectsInArea;



    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(tagsToInclude.Count > 0 && !tagsToInclude.Contains(collider.gameObject.tag))
        {
            return;
        }
        objectsInArea.Add(collider.gameObject.transform);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (objectsInArea.Contains(collider.gameObject.transform))
        {
            objectsInArea.Remove(collider.gameObject.transform);
        }
    }

    public void SetColor(bool active)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (active)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .4f);
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .2f);
        }
    }
}
