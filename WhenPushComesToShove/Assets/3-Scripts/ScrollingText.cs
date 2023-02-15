using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Code from: https://www.youtube.com/watch?v=AuZNU7JTeWQ
public class ScrollingText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float scrollSpeed = 5;
    private TextMeshProUGUI cloneTMP;

    private RectTransform textTransform;
    private string text;
    private string tempText;

    void Awake()
    {
        textTransform = tmp.GetComponent<RectTransform>();

        cloneTMP = Instantiate(tmp) as TextMeshProUGUI;
        RectTransform cloneTransform = cloneTMP.GetComponent<RectTransform>();
        cloneTransform.SetParent(tmp.transform);
        cloneTransform.position = new Vector3(cloneTransform.position.x, 552f, 0);
        cloneTransform.anchorMin = new Vector2(1, .5f);
        cloneTransform.localScale = Vector3.one;
    }

    IEnumerator Start()
    {
        float width = tmp.preferredWidth * tmp.fontSize / 8;
        Vector3 startPosition = textTransform.position;

        float scrollPosition = 0;

        while(true)
        {
            textTransform.position = new Vector3(-scrollPosition % width, startPosition.y, startPosition.z);
            scrollPosition += scrollSpeed * 20 * Time.deltaTime;

            yield return null;
        }
    }    
}
