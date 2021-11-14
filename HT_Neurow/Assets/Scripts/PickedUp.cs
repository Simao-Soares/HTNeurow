using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickedUp : MonoBehaviour
{
    public EventTrigger.TriggerEvent CoinPickup;
    private void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.GetComponent<Collider>().tag == "PlayerPos")
        {
            Debug.Log("+1");
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            this.CoinPickup.Invoke(eventData);
        }
    }
}
