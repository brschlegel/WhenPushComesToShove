using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    [Tooltip("To face transform.right, should the sprite be flipped?")]
    [SerializeField]
    private bool isRightFlipped;
    [SerializeField]
    private StateBrain brain;
    [SerializeField]
    private Rigidbody2D rb;
    
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(brain.target != null)
        {
            Vector2 toTarget = brain.target.position - brain.transform.position;
            bool isXPositive = toTarget.x >= 0;
            // same as: (isRightFlipped && isXPositive) || (!isRightFlipped && !isXPositive);
            sr.flipX = isRightFlipped == isXPositive;
        }
        else if(rb.velocity.magnitude >= .001f)
        {
            bool isXPositive = rb.velocity.x >= 0;
            // same as: (isRightFlipped && isXPositive) || (!isRightFlipped && !isXPositive);
            sr.flipX = isRightFlipped == isXPositive;
        }
    }
}
