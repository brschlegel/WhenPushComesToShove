using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringHazard : MonoBehaviour
{
    [SerializeField] Vector2 springDirection;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}
