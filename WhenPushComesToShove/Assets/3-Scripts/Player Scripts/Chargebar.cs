using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargebar : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHandler inputHandler;
    [SerializeField]
    private float maxScale;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //If heavy shove is charging then sprite renderer should be on, if it isnt then sprite renderer shouldnt be on
        if(inputHandler.heavyShoveIsCharging != sr.enabled)
        {
            sr.enabled = inputHandler.heavyShoveIsCharging;
        }
        transform.localScale = new Vector3(Mathf.Clamp((inputHandler.heavyShoveCharge * maxScale) / inputHandler.heavyShoveChargeTime, 0 , maxScale),transform.localScale.y, transform.localScale.z);
      
    }
}
