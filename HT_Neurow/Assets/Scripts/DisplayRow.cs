using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRow : MonoBehaviour
{
    [SerializeField] private Text customText;

	void Start(){
		customText.enabled = false;
	}



	void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = true;
            Debug.Log("RowStart");
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = false;
            Debug.Log("RowEnd");
        }
    }
}
