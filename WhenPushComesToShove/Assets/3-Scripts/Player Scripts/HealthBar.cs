using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float maxScale;


    [SerializeField]
    private Health combat;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3((combat.CurrentHealth * maxScale) / combat.maxHealth,transform.localScale.y, transform.localScale.z);
    }
}
