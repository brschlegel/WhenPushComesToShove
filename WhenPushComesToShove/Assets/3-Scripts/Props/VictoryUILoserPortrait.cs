using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUILoserPortrait : MonoBehaviour
{
    [SerializeField] private RawImage[] portraits;

    
    private void OnEnable()
    {
        foreach (RawImage p in portraits)
        {
            p.transform.parent.parent.gameObject.SetActive(false);
            p.gameObject.SetActive(false);
        }
    }

    public void SetMaterial(int index, Material mat)
    {
        portraits[index].material = mat;
        portraits[index].transform.parent.parent.gameObject.SetActive(true);
        portraits[index].gameObject.SetActive(true);
    }

    //public void ThrowConfetti(int playerIndex)
    //{
    //    
    //    //PlayerComponentReferences references = GameState.players[index].GetComponent<PlayerComponentReferences>();
    //
    //    //confettiSpawnParent.gameObject.SetActive(true);
    //}
}
