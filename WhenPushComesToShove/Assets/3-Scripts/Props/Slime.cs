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
    [SerializeField] private Material slimeDefaultMat;

    [HideInInspector] public bool consumed = false;

    public SlimeRancherLogic logic;
    public int slimeSize = 1;
    public int pointWorth = 1;
    public int slimeTeamIndex = -1;

    public void OnEnable()
    {
        //Very basic movement code
        logic = transform.parent.GetComponentInParent<SlimeRancherLogic>();

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
        if (slimeSize > slimeSizeThreshold || slimeToConsume.slimeSize > slimeSizeThreshold || (slimeSize + slimeToConsume.slimeSize > slimeSizeThreshold)
            || consumed || slimeToConsume.consumed)
        {
            return;
        }

        if (slimeTeamIndex == slimeToConsume.slimeTeamIndex)
        {
            //Remove existing scores
            logic.UpdateTeamScore(slimeTeamIndex, pointWorth, false);
            logic.UpdateTeamScore(slimeToConsume.slimeTeamIndex, slimeToConsume.pointWorth, false);

            if (slimeSize >= slimeToConsume.slimeSize)
            {
                slimeSize += slimeToConsume.slimeSize;
                transform.localScale = new Vector3(slimeSize, slimeSize, 1);

                UpdatePointText();
                //Add new score
                logic.UpdateTeamScore(slimeTeamIndex, pointWorth, true);
                slimeToConsume.consumed = true;

                Destroy(slimeToConsume.gameObject);
            }
            else
            {
                slimeToConsume.slimeSize += slimeSize;
                slimeToConsume.transform.localScale = new Vector3(slimeToConsume.slimeSize, slimeToConsume.slimeSize, 1);

                slimeToConsume.UpdatePointText();
                //Add new score
                slimeToConsume.logic.UpdateTeamScore(slimeToConsume.slimeTeamIndex, slimeToConsume.pointWorth, true);
                consumed = true;

                Destroy(gameObject);
            }
        }
    }

    public void SplitSlime(int numOfSplits)
    {
        //Remove existing score
        logic.UpdateTeamScore(slimeTeamIndex, pointWorth, false);

        if (numOfSplits >= slimeSize)
        {
            numOfSplits = slimeSize - 1;
        }

        slimeSize -= numOfSplits;
        transform.localScale = new Vector3(slimeSize, slimeSize , 1);
        UpdatePointText();

        //Add new score
        logic.UpdateTeamScore(slimeTeamIndex, pointWorth, true);

        for (int i = 0; i < numOfSplits; i++)
        {
            Vector3 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            //Slime newSlime = Instantiate(SlimePrefab, transform.position + dir, Quaternion.identity).GetComponent<Slime>();
            Slime newSlime = Instantiate(SlimePrefab, transform.parent).GetComponent<Slime>();
            newSlime.transform.position = transform.position + dir;

            newSlime.slimeSize = 1;
            newSlime.slimeTeamIndex = -1;
            newSlime.GetComponent<SpriteRenderer>().material = slimeDefaultMat;
            newSlime.transform.localScale = new Vector3(1, 1, 1);
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
