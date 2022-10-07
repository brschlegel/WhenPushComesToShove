using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMode : MonoBehaviour
{

    [Header("In Projectile Mode")]
    [SerializeField]
    private float pDrag;
    [SerializeField]
    private PhysicsMaterial2D pMaterial;
    [SerializeField]
    private float velocityThreshold;
    
    public ProjectileHitbox pHitbox;


    [Header("In Static Mode")]
    [SerializeField]
    private float sDrag;
    [SerializeField]
    private PhysicsMaterial2D sMaterial;


    private Rigidbody2D rb;
    [Header("Events")]  
    public UnityEvent onPModeEnter;
    public UnityEvent onPModeExit;

    private float frames;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        pHitbox.pMode = this;
    }
    void OnEnable()
    {
        if(rb == null)
            Init();

        rb.drag = pDrag;
        rb.sharedMaterial = pMaterial;
        pHitbox.gameObject.SetActive(true);
        frames = 0;
        onPModeEnter?.Invoke();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
     
        //First frame after adding a force the force hasnt been applied. 
        if(frames > 0 && rb.velocity.magnitude <= velocityThreshold )
        {
            this.enabled = false;
        }
        frames++;

        if(Velocity.magnitude > GlobalSettings.terminalVelocity)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, GlobalSettings.terminalVelocity);
        }
    }

    void OnDisable()
    {
        rb.drag = sDrag;
        rb.sharedMaterial = sMaterial;
        pHitbox.gameObject.SetActive(false);
        pHitbox.OwnersToIgnore.Clear();
        onPModeExit?.Invoke();
    }

    public float Mass
    {
        get { return rb.mass; }
    }

    public Vector2 Velocity
    {
        get
        {
            if (rb == null)
            {
                Init();
            }
            return rb.velocity;
        }
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force);
    }


}
