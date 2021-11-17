using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PickedUp : MonoBehaviour
{
    public EventTrigger.TriggerEvent CoinPickup;
    public float shrinkStep = 0.1f;

    public GameObject player;

    private bool aux = false; 

    

    private void Update() {
        if(aux == true) StartCoroutine(ShrinkCoin());
        
    }

    IEnumerator ShrinkCoin(){

        float initRadius = gameObject.transform.localScale.x;
        for (float i = initRadius; i >= 0; i -= shrinkStep) 
        {
            gameObject.transform.localScale = new Vector3(i, 100f, i);
            yield return null;
        }
        aux = false;
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.GetComponent<Collider>().tag == "PlayerPos")
        {
            Collider collider = GetComponent<Collider>();
            collider.enabled = false; //fix double points bug
            
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            this.CoinPickup.Invoke(eventData);
            Color newColor = Color.green;
            newColor.a = 0.5f;
            gameObject.GetComponent<Renderer> ().material.color = newColor;
            aux = true;
        }
    }
}
