using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInDangerZone : MonoBehaviour
{
    [SerializeField] float delayDestroy;

    // Start is called before the first frame update
    void Start()
    {

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
        if (collision.gameObject.tag == "DangerZone")
        {
            StartCoroutine(DelayDestroy());
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(delayDestroy);
        Destroy(gameObject);
    }
}
