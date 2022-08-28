using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//direction and speed stored seperately so that speed can be tweened
public struct VelocityData
{
    public Vector2 direction;
    public float speed;
}
[RequireComponent(typeof(Rigidbody2D))]
public class VelocitySetter : MonoBehaviour
{
    private Rigidbody2D rb;
    Dictionary<string, VelocityData> sources;
    void Start()
    {
        if(sources == null)
        {
            Init();
        }
    }

    public void Init()
    {
        sources = new Dictionary<string, VelocityData>();
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
        VelocityData data = new VelocityData();
        data.direction = direction.normalized;
        data.speed = speed;
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
    /// Adds a velocity source, and then cancels it after some time
    /// </summary>
    /// <param name="sourceID">String ID the contribution is stored under</param>
    /// <param name="direction">Direction of the target velocity. WILL BE NORMALIZED INSIDE OF FUNCTION</param>
    /// <param name="speed">Magnitude of the target vector</param>
    /// <param name="time">Seconds before source is cancelled</param>
    public void AddSource(string sourceID, Vector2 direction, float speed, float time)
    {
        VelocityData data = new VelocityData();
        data.direction = direction.normalized;
        data.speed = speed;
        if(!sources.ContainsKey(sourceID))
        {
            sources.Add(sourceID, data);
        }
        else
        {
            sources[sourceID] = data;
        }
        StartCoroutine(CancelAfterSeconds(sourceID, time));
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
            velocity = sources[sourceID].direction * sources[sourceID].speed;
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
    #endregion

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
            VelocityData data = sources[key];
            velocity += data.direction * data.speed;
        }
        rb.velocity = velocity;
    }
}
