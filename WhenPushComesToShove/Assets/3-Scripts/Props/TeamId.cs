using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team {None, Orange, Purple}
public class TeamId : MonoBehaviour
{
   public Team team;
   public Team GetOppositeTeam()
   {
        if(team == Team.None)
        {
        return team;
        }
        else
        {
            return team == Team.Orange ? Team.Purple : Team.Orange;
        }
   }
}
