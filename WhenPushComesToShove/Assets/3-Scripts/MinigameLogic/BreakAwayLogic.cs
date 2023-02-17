using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//Put basket into game, hooking up respawning logic
//Give each barrel a team identity
//Flip flop barrel spawn points
//Add some hazards in
//Get player respawning in 
public class BreakAwayLogic : MinigameLogic
{
    [SerializeField]
    private Vector2 forceOnBarrel;
    [SerializeField]
    private List<Transform> barrels;
    [SerializeField]
    private TriggerEventPasser basket;

    public override void Init()
    {
        basket.triggerEnter += ScoreBasket;
        base.Init();
    }
    public override void StartGame()
    {
        foreach(Transform barrel in barrels)
        {
            barrel.GetComponent<ConstantForce2D>().force = forceOnBarrel;
        }
        base.StartGame();
    }

    public void ScoreBasket(GameObject barrel)
    {
        barrel.SetActive(false);
    }
}
