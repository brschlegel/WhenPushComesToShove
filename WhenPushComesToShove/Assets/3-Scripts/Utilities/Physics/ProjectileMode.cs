using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector2 lastVelocity;
    private float speedChange;

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
        lastVelocity = Vector2.zero;
        pHitbox.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        speedChange = (rb.velocity.magnitude - lastVelocity.magnitude) / Time.fixedDeltaTime;
        lastVelocity = rb.velocity;
        if(rb.velocity.magnitude <= velocityThreshold && speedChange < 0 )
        {
            this.enabled = false;
        }

    }

    void OnDisable()
    {
        rb.drag = sDrag;
        rb.sharedMaterial = sMaterial;
        pHitbox.gameObject.SetActive(false);
        pHitbox.OwnersToIgnore.Clear();
    }

    public float Mass
    {
        get {return rb.mass;}
    }

    public Vector2 Velocity
    {
        get{return rb.velocity;}
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force);
    }


}
