using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slime : MonoBehaviour
{
    [SerializeField] private GameObject SlimePrefab;
    [SerializeField] private TextMeshPro pointText;
    [SerializeField] private float slimeSizeThreshold = 5;
    [SerializeField] private float timeBetweenMovements = 2;
    [SerializeField] private ProjectileMode pMode;
    [SerializeField] private float movementForce = 10;

    public int slimeSize = 1;
    public int pointWorth = 1;
    public int slimeTeamIndex = -1;

    public void OnEnable()
    {
        //Very basic movement code
        StartCoroutine(ApplyForceInRandomDirection());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Consume Smaller Slime
        if (collision.gameObject.TryGetComponent<Slime>(out Slime slimeScript))
        {
            if (slimeTeamIndex != -1)
            {
                ConsumeSlime(slimeScript);
            }
        }

    }

    public void ConsumeSlime(Slime slimeToConsume)
    {
        if (slimeSize > slimeSizeThreshold || slimeToConsume.slimeSize > slimeSizeThreshold || (slimeSize + slimeToConsume.slimeSize > slimeSizeThreshold))
        {
            return;
        }

        if (slimeTeamIndex == slimeToConsume.slimeTeamIndex)
        {
            if (slimeSize >= slimeToConsume.slimeSize)
            {
                slimeSize += slimeToConsume.slimeSize;
                transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
                UpdatePointText();
                Destroy(slimeToConsume.gameObject);
            }
            else
            {
                slimeToConsume.slimeSize += slimeSize;
                slimeToConsume.transform.localScale = new Vector3(slimeToConsume.slimeSize + 1, slimeToConsume.slimeSize + 1, 1);
                slimeToConsume.UpdatePointText();
                Destroy(gameObject);
            }
        }
    }

    public void SplitSlime(int numOfSplits)
    {
        if (numOfSplits >= slimeSize)
        {
            numOfSplits = slimeSize - 1;
        }

        slimeSize -= numOfSplits;
        transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
        UpdatePointText();

        for (int i = 0; i < numOfSplits; i++)
        {
            Vector3 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Slime newSlime = Instantiate(SlimePrefab, transform.position + dir, Quaternion.identity).GetComponent<Slime>();

            newSlime.slimeSize = 1;
            newSlime.transform.localScale = new Vector3(slimeSize + 1, slimeSize + 1, 1);
            newSlime.UpdatePointText();
        }
    }

    public void UpdatePointText()
    {
        pointWorth = Mathf.FloorToInt(Mathf.Pow(slimeSize, 2));

        if (pointText != null && pointText.gameObject.activeSelf)
        {
            pointText.text = pointWorth + " pt";
        }
    }

    private IEnumerator ApplyForceInRandomDirection()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        pMode.AddForce(dir * movementForce);
        Debug.Log(" " + dir.x + dir.y);
        yield return new WaitForSeconds(timeBetweenMovements);
        StartCoroutine(ApplyForceInRandomDirection());
    }
}
