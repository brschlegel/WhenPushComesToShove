using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class SpringHazard : MonoBehaviour
{
    [SerializeField] float cooldownTime;
    WaitForSeconds cooldown;

    [SerializeField] DirectionProperties.Direction[] direction;

    [HideInInspector] public GameObject arrowParent;

    List<Vector2> possibleDirections = new List<Vector2>();

    int rng;

    KnockbackSetDirection kback;
    BoxCollider2D boxCollider;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        kback = GetComponentInChildren<KnockbackSetDirection>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        arrowParent = gameObject.transform.GetChild(1).gameObject;

        EnableArrows();

        cooldown = new WaitForSeconds(cooldownTime);

        foreach(DirectionProperties.Direction dir in direction)
        {
            if (dir.enabled)
                possibleDirections.Add(dir.direction);
        }

        //Assigns a random direction before the player hits it
        grabRandomIndex();
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            arrowParent = gameObject.transform.GetChild(1).gameObject;
            EnableArrows();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (boxCollider != null)
            boxCollider.enabled = true;
        if(sprite != null)
            sprite.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Resets velocity before adding a new force
        Rigidbody2D rb =  collision.transform.parent.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        ThrowInRandomDirection();
    }

    private void ThrowInRandomDirection()
    {
        StartCoroutine(StartCooldown());

        //Add a knockback force in the assigned random direction
        kback.direction = possibleDirections[rng];

        //Grabs a new random direction
        grabRandomIndex();
    }

    IEnumerator StartCooldown()
    {
        boxCollider.enabled = false;
        sprite.color = Color.gray;
        yield return cooldown;
        boxCollider.enabled = true;
        sprite.color = Color.white;
    }

    /// <summary>
    /// Sets up the arrows based on the direction of the spring
    /// </summary>
    public void EnableArrows()
    {
        for(int i = 0; i < arrowParent.transform.childCount; i++)
        {
            arrowParent.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = direction[i].enabled;
        }
    }

    //Helper functions
    private void grabRandomIndex() => rng = Random.Range(0, possibleDirections.Count);

}
