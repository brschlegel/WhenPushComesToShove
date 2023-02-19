using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RetrieveObject(GameObject obj);
[RequireComponent(typeof(Collider2D))]
public class TriggerEventPasser : MonoBehaviour
{
    public List<string> tagsToAccept;
    public event RetrieveObject triggerEnter;
    public event RetrieveObject triggerExit;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (tagsToAccept.Contains(collider.gameObject.tag))
        {
            triggerEnter?.Invoke(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (tagsToAccept.Contains(collider.gameObject.tag))
        {
            triggerExit?.Invoke(collider.gameObject);
        }
    }
}
