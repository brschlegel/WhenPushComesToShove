using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionListener
{
    void  OnCollisionEnter2D(Collision2D collision);
    void  OnCollisionExit2D(Collision2D collision);
}

public class CollisionEventSplitter : MonoBehaviour
{
    public List<ICollisionListener> listeners = new List<ICollisionListener>();

    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ICollisionListener l in listeners)
        {
            l.OnCollisionEnter2D(collision);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        foreach(ICollisionListener l in listeners)
        {
            l.OnCollisionExit2D(collision);
        }
    }
}
