using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifierItem : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string displayName;

    public void Init()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponentInChildren<TextMeshProUGUI>().text = displayName;
    }
}
