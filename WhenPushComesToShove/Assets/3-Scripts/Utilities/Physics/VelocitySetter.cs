using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



[RequireComponent(typeof(Rigidbody2D))]
public class VelocitySetter : MonoBehaviour
{
    private Rigidbody2D rb;
    Dictionary<string, Vector2> sources;
    void Start()
    {
        if(sources == null)
        {
            Init();
        }
    }

    public void Init()
    {
        sources = new Dictionary<string, Vector2>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// Adds a velocity source, will overwrite previous source with same ID
    /// </summary>
    /// <param name="sourceID">String ID the contribution is stored under</param>
    /// <param name="direction">Direction of the target velocity. WILL BE NORMALIZED INSIDE OF FUNCTION</param>
    /// <param name="speed">Magnitude of the target vector</param>
    public void AddSource(string sourceID, Vector2 direction, float speed)
    {
        Vector2 data = direction.normalized * speed;
        if(!sources.ContainsKey(sourceID))
        {
            sources.Add(sourceID, data);
        }
        else
        {
            sources[sourceID] = data;
        }
    }

        
    /// <summary>
    /// Adds a velocity source, will overwrite previous source with same ID
    /// </summary>
    /// <param name="sourceID">String ID the contribution is stored under</param>
    /// <param name="Source">Source</param>
    public void AddSource(string sourceID, Vector2 source)
    {
        if(!sources.ContainsKey(sourceID))
        {
            sources.Add(sourceID, source);
        }
        else
        {
            sources[sourceID] = source;
        }
    }

    /// <summary>
    /// Adds a velocity source, and then cancels it after some time
    /// </summary>
    /// <param name="sourceID">String ID the contribution is stored under</param>
    /// <param name="source">Vector2 source</param>
    /// <param name="time">Seconds before source is cancelled</param>
    public void AddSourceForTime(string sourceID, Vector2 source, float time)
    {
        if(!sources.ContainsKey(sourceID))
        {
            sources.Add(sourceID, source);
        }
        else
        {
            sources[sourceID] = source;
        }
        StartCoroutine(CancelAfterSeconds(sourceID, time));
    }
    /// <summary>
    /// Adds a velocity source, then cancels it after tween is completed
    /// </summary>
    /// <param name="sourceID">String ID the contribution is stored under</param>
    /// <param name="source">Vector2 source</param>
    /// <param name="tween">Tween used on velocity source</param>
    public void AddSource(string sourceID, Vector2 source, Tween tween)
    {
        AddSource(sourceID, source);
        StartCoroutine(CancelAfterTween(sourceID, tween));
    }

    /// <summary>
    /// Checks for the velocity associated with a sourceID
    /// </summary>
    /// <param name="sourceID">ID to check for</param>
    /// <param name="velocity">Velocity at that ID</param>
    /// <returns>If sourceID has a velocity attached</returns>
    public bool QuerySource(string sourceID, out Vector2 velocity)
    {
        if (sources.ContainsKey(sourceID))
        {
            velocity = sources[sourceID];
            return true;
        }
        velocity = Vector2.zero;
        return false;
    }
    /// <summary>
    /// Remove a velocity source
    /// </summary>
    /// <param name="sourceID">String ID of the source to cancel</param>
    public void Cancel(string sourceID)
    {
        if(sources.ContainsKey(sourceID))
        {
            sources.Remove(sourceID);
        }
    }
    /// <summary>
    /// Cancel all sources
    /// </summary>
    public void CancelAll()
    {
        foreach(string r in sources.Keys)
        {
            Cancel(r);
        }
    }

    #region Coroutines

    private IEnumerator CancelAfterSeconds(string requestID, float time)
    {
        yield return new WaitForSeconds(time);
        Cancel(requestID);
    }
    
    private IEnumerator CancelAfterTween(string requestID, Tween tween)
    {
        yield return tween.WaitForCompletion();
        Cancel(requestID);
    }
    #endregion

    /// <summary>
    /// For use with virtual tweens
    /// </summary>
    /// <param name="f">float to update</param>
    /// <param name="sourceID">Source ID</param>
    public void UpdateVelocityMagnitude(string sourceID, float f)
    {
        sources[sourceID] = sources[sourceID].normalized * f;
    }
    #region Properties
    public Vector2 Velocity
    {
        get {return rb.velocity;}
    }
    #endregion

    private void FixedUpdate()
    {
        CalculateVelocity();
    }
    private void CalculateVelocity()
    {
        Vector2 velocity = Vector2.zero;
        //Apply requests 
        foreach(string key in sources.Keys)
        {
            Vector2 data = sources[key];
            velocity += data;
            //Debug.Log(data.magnitude);
        }
        rb.velocity = velocity;
    }
}
