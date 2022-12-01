using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEvent : UnityEvent<Collider2D>
{

}

[RequireComponent(typeof(Collider2D))]
public class TriggerEventPasser : MonoBehaviour
{
   public TriggerEvent enterEvent;
   public TriggerEvent exitEvent;
   public TriggerEvent stayEvent;

   private void OnTriggerEnter2D(Collider2D collider)
   {
        enterEvent?.Invoke(collider);
   }

   private void OnTriggerStay2D(Collider2D collider)
   {
        stayEvent?.Invoke(collider);
   }

   private void OnTriggerExit2D(Collider2D collider)
   {
        exitEvent?.Invoke(collider);
   }
}
