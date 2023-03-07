using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUILoserPortrait : MonoBehaviour
{
    [SerializeField] private RawImage[] portraits;
    Dictionary<int, Transform> confettiLocations = new Dictionary<int, Transform>();
    [SerializeField] private GameObject[] UIConfetti = new GameObject[4];
    [SerializeField] private Vector3 offset = new Vector3(-2, 1, 0);

    
    private void OnEnable()
    {
        foreach (RawImage p in portraits)
        {
            p.transform.parent.parent.gameObject.SetActive(false);
            p.gameObject.SetActive(false);
        }
    }

    public void SetMaterial(int index, Material mat, int playerIndex)
    {
        confettiLocations.Add(playerIndex, portraits[index].transform);
        portraits[index].material = mat;
        portraits[index].transform.parent.parent.gameObject.SetActive(true);
        portraits[index].gameObject.SetActive(true);
    }

    public void ThrowConfetti(int playerIndex)
    {
        Instantiate(UIConfetti[playerIndex], confettiLocations[playerIndex].position + offset, Quaternion.identity);
    }
}
