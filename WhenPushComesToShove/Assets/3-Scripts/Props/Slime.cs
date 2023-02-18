using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] public GameObject SlimePrefab;
    public float slimeSize = 1;
    public float slimeTeamIndex = -1;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Consume Smaller Slime
        if (collision.gameObject.TryGetComponent<Slime>(out Slime slimeScript))
        {
            ConsumeSlime(slimeScript);
        }

    }

    public void ConsumeSlime(Slime slimeToConsume)
    {
        if (slimeTeamIndex == slimeToConsume.slimeTeamIndex)
        {
            if (slimeSize >= slimeToConsume.slimeSize)
            {
                slimeSize += slimeToConsume.slimeSize;
                transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
                Destroy(slimeToConsume.gameObject);
            }
            else
            {
                slimeToConsume.slimeSize += slimeSize;
                slimeToConsume.transform.localScale = new Vector3(slimeToConsume.slimeSize + 1, slimeToConsume.slimeSize + 1, 1);
                Destroy(gameObject);
            }
        }
    }

    public void SplitSlime()
    {
        slimeSize -= 1;
        transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
        Slime newSlime = Instantiate(SlimePrefab, transform.position, Quaternion.identity).GetComponent<Slime>();

        newSlime.slimeSize = slimeSize;
        newSlime.transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
    }
}
