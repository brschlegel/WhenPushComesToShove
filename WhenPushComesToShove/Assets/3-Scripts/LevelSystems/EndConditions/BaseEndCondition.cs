using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseEndCondition : MonoBehaviour
{
    protected LevelProperties levelProp;
    [Tooltip("Time between the end condition being met and transitioning to the next room.")]
    [SerializeField] protected float delayBeforeEndCondition;
    protected WaitForSeconds delay;
    protected bool conditionMet = false;

    public string roomExplanation = "";
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        levelProp = gameObject.transform.parent.parent.GetComponent<LevelProperties>();
        delay = new WaitForSeconds(delayBeforeEndCondition);

        text = GameObject.Find("RoomExplanation").GetComponent<TextMeshProUGUI>();
        text.text = roomExplanation;
    }

    protected virtual void OnDisable()
    {
        conditionMet = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!conditionMet)
            TestCondition();
    }

    
    protected virtual void TestCondition()
    {
        conditionMet = true;
        StartCoroutine(TransitionRooms());
    }

    protected virtual IEnumerator TransitionRooms()
    {
        yield return delay;
        LevelManager.onNewRoom.Invoke();
    }
}
