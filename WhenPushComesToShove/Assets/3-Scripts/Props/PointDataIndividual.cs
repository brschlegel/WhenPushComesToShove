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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < GameState.players.Count; i++)
        {
            Debug.Log(GameState.players[i] + " " + collision.transform);
            if(GameState.players[i] == collision.transform)
            {
                teamIndex = i;
                sprite.color = colors[i];
            }
        }
    }
}
