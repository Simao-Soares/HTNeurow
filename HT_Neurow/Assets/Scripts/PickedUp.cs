using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PickedUp : MonoBehaviour
{
    public EventTrigger.TriggerEvent CoinPickup;
    public float shrinkStep;

    public GameObject player;
    public GameObject buoyLight;
    public GameObject greenBuoyLight;
    


    private bool auxCoinCollision = false;

    public Sound coinAudio;
    

    private void Start()
    {

        //If this script is applied to a buoy
        if (gameObject.tag != "coin")
        {
            gameObject.GetComponent<CapsuleCollider>().radius = GameManager.objectiveRad/1.8f;
            greenBuoyLight.SetActive(false); 
        }
        else coinAudio.source = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if(auxCoinCollision == true) StartCoroutine(ShrinkCoin());
        
    }

    IEnumerator ShrinkCoin(){

        float initRadius = gameObject.transform.localScale.x;
        if (gameObject.tag == "coin")
        {
            for (float i = initRadius; i >= 0; i -= shrinkStep / 150)
            {
                gameObject.transform.localScale = new Vector3(i, 100f, i);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        auxCoinCollision = false;
        Destroy(gameObject);
        
    }


    private void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.GetComponent<Collider>().tag == "PlayerPos")
        {
            //player.GetComponent<CoinGame>().gameEventAux = "pickedUp(c)";
            Color newColor = Color.green;
            newColor.a = 0.1f;

            if (gameObject.tag == "coin")
            {
                Collider collider = GetComponent<Collider>();
                collider.enabled = false; //fix double points bug
                BaseEventData eventData = new BaseEventData(EventSystem.current);
                this.CoinPickup.Invoke(eventData);
                gameObject.GetComponent<Renderer>().material.color = newColor;
                coinAudio.source.Play();
            }

            else
            {
                buoyLight.SetActive(false);
                greenBuoyLight.SetActive(true);
                
            }
            auxCoinCollision = true;  
        }
    }
}
