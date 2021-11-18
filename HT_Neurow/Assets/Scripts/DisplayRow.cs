using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays Row + Movement V1

public class DisplayRow : MonoBehaviour
{
    [SerializeField] Text customText;

	public float rotA;
	private bool rAux = false;
	private bool lAux = false;
	public GameObject myPlayer;


	public BoatMovement _playerScript;

	//Array of Water GameObjects
	[System.Serializable]
	public class TerrainList
	{
		public GameObject[] water;
	}

	public TerrainList myTerrainList = new TerrainList();  //List of terrain elements

	void Start(){
		customText.enabled = false;
		_playerScript = myPlayer.GetComponent<BoatMovement>();

	}

	void FixedUpdate()
	{
		//if(lAux) StartCoroutine(TurnRight());
		//if(rAux) StartCoroutine(TurnLeft());
		if(lAux)
		{
			_playerScript.turnLeft = true;
			lAux=false;
		}

		if(rAux)
		{
			_playerScript.turnRight = true;
			rAux=false;
		} 
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
//				for(int i = 0; i < myTerrainList.water.Length ; i++){
//					myTerrainList.water[i].transform.Rotate(Vector3.up * 100f * Time.deltaTime);
//				}
				lAux = true;  //right row turns left
			}

			if(gameObject.name == "L_RowCollider"){
//				for(int i = 0; i < myTerrainList.water.Length ; i++){
//					myTerrainList.water[i].transform.Rotate(Vector3.down * 100f * Time.deltaTime);
//				}
				rAux = true;  //left row turns right
			}
        }
    }

	//TENTATIVA DE DEFINIR A SPEED OF ROTATION, mas e uma merda
	IEnumerator TurnRight(){
		for (float ang = 0f; ang > -rotA; ang-= 1f){
			yield return null;
			for(int i = 0; i < myTerrainList.water.Length ; i++){
				myTerrainList.water[0].transform.Rotate(0, 1, 0, Space.Self);
			}
			yield return null;
			lAux = false;
		}
	}

	IEnumerator TurnLeft(){
		for (float ang = 0f; ang < rotA; ang+= 1f){
			yield return null;
			for(int i = 0; i < myTerrainList.water.Length ; i++){
				myTerrainList.water[0].transform.Rotate(0, -1, 0, Space.Self);

			}
			yield return null;
			rAux = false;
		}

	}



}
