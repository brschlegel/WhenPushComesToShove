using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    SpriteRenderer r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Color c)
    {
        r.color = c;
    }

    public void SetRandom()
    {
        Color c = new Color(Random.value, Random.value, Random.value);
        r.color = c;
    }

}
