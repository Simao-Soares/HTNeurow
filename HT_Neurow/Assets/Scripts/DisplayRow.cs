using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays Row + Movement V1

public class DisplayRow : MonoBehaviour
{
    [SerializeField] Text customText;

	//public float rotA;
	private bool rAux = false;
	private bool lAux = false;
	public GameObject myPlayer;
	

	[HideInInspector] public BoatMovement _playerScript;

	public GameObject rowCollider;
	public GameObject paddle;

	public float waterLevel;
	public float paddleDepth = 0;
	public float paddleSpeed = 0;


	Rigidbody rowRB;

	public float rotationSpeed;



	void Start(){
		customText.enabled = false;
		_playerScript = myPlayer.GetComponent<BoatMovement>();

		//waterLevel = transform.position.y + (transform.localScale.y) / 2; //as it stands should be equal to -0.35f
		rowRB = paddle.GetComponent<Rigidbody>();

	}

	//void FixedUpdate()
	//{
	//	//if(lAux) StartCoroutine(TurnRight());
	//	//if(rAux) StartCoroutine(TurnLeft());
	//	if(lAux)
	//	{
	//		_playerScript.turnLeft = true;
	//		lAux=false;
	//	}

	//	if(rAux)
	//	{
	//		_playerScript.turnRight = true;
	//		rAux=false;
	//	} 
	//}



	void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = true;
			waterLevel = rowCollider.transform.position.y;
			if (gameObject.name == "R_RowCollider") _playerScript.turnRight = true;
			else if (gameObject.name == "L_RowCollider") _playerScript.turnLeft = true;
		}
    }

    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			paddleDepth = waterLevel - rowCollider.transform.position.y;
			paddleSpeed = rowRB.angularVelocity.z; //------------------------------------doesnt work since the boat is always moving forward (in z)

			if (gameObject.name == "R_RowCollider")
			{
				rotationSpeed = Mathf.Abs(paddleDepth * paddleSpeed);
				Debug.Log(paddleSpeed);
			}

			if (gameObject.name == "L_RowCollider")
			{
				rotationSpeed = -Mathf.Abs(paddleDepth * paddleSpeed);
				//Debug.Log(paddleSpeed);
			}
			}
	}	



    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = false;
			_playerScript.turnLeft = false;
			_playerScript.turnRight = false;
		}
    }
}
