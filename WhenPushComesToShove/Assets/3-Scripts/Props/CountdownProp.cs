using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CountdownProp : MonoBehaviour
{
    private TextMeshProUGUI text;
    public UnityEvent onFinished;
    public float startingValue;

    private float currentValue;

    private bool finished;
    // Start is called before the first frame update
    void Start()
    {
        currentValue = startingValue;
        text = GetComponentInChildren<TextMeshProUGUI>();
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
            currentValue -= Time.deltaTime;
            text.text = (((int)currentValue) + 1).ToString();

            if (currentValue <= 0 )
            {
                onFinished?.Invoke();
                finished = true;
                text.text = "";
            }
        }

    }

}
