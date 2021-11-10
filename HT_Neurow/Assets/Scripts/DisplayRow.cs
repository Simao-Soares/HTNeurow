using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRow : MonoBehaviour
{
    [SerializeField] private Text customText;


	//Array of Water GameObjects
	[System.Serializable]
	public class TerrainList
	{
		public GameObject[] water;
	}

	public TerrainList myTerrainList = new TerrainList();  //List of handPoints

	void Start(){
		customText.enabled = false;
	}



	void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = true;
            //Debug.Log("RowStart");
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = false;
            //Debug.Log("RowEnd");

			if(gameObject.name == "R_RowCollider"){
				for(int i = 0; i < myTerrainList.water.Length ; i++){
					myTerrainList.water[i].transform.Rotate(Vector3.up * 100f * Time.deltaTime);
				}
			}

			if(gameObject.name == "L_RowCollider"){
				for(int i = 0; i < myTerrainList.water.Length ; i++){
					myTerrainList.water[i].transform.Rotate(Vector3.down * 100f * Time.deltaTime);
				}
			}


        }
    }
}
