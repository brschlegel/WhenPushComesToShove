using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RestrictedShoveVisualizer : MonoBehaviour
{
    Rigidbody2D rb;
    RigidbodyConstraints2D con; 
    private void OnEnable()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        //if(rb.cons)
        //    con.
    }
}
