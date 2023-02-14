using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public List<string> tagsToIgnoreCollision = new List<string>();

    [SerializeField] private Health playerHealth;

    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (tagsToIgnoreCollision.Contains(collision.gameObject.tag))
        {
            Physics2D.IgnoreCollision(collision.collider, collider);
        }
    }
}
