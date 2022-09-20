using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerHealth.dead)
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(collision.collider, collider);
            }
        }
    }
}
