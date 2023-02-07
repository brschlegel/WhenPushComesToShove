using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDataIndividual : PointData
{
    public int teamIndex;

    [SerializeField]
    private Color32[] colors;
    SpriteRenderer sprite;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        sprite.color = colors[4];
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detects who shoved the ball and changes the color accordingly
        for (int i = 0; i < GameState.players.Count; i++)
        {
            if (collision.transform.parent.IsChildOf(GameState.players[i]) && collision.tag == "Shove")
            {
                teamIndex = i;
                sprite.color = colors[i];
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //Only triggers if the object is neutral
        if(teamIndex == 4)
        {
            if(collision.transform.TryGetComponent<PointDataIndividual>(out PointDataIndividual pointData))
            {
                teamIndex = pointData.teamIndex;
                sprite.color = colors[teamIndex];
            }
            
        }
    }


}
