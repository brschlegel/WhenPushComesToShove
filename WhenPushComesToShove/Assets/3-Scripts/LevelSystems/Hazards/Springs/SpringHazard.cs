using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpringHazard : MonoBehaviour
{
    [SerializeField] DirectionProperties.Direction[] direction;
    List<Vector2> possibleDirections = new List<Vector2>();

    KnockbackSetDirection kback;

    GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        kback = GetComponentInChildren<KnockbackSetDirection>();

        foreach(DirectionProperties.Direction dir in direction)
        {
            if (dir.enabled)
                possibleDirections.Add(dir.direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThrowInRandomDirection();
    }

    private void ThrowInRandomDirection()
    {
        int rng = Random.Range(0, possibleDirections.Count);

        kback.direction = possibleDirections[rng];
    }
}
