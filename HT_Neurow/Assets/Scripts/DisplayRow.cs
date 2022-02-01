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
	public GameObject wristTracker;
	public GameObject wristTrackerAUX;

	[HideInInspector] public float waterLevel;

	[HideInInspector] public float paddleDepth = 0;
	[HideInInspector] public float paddleSpeed = 0;


	Rigidbody wristRB;
	Rigidbody playerRB;

	[HideInInspector] public float rotationSpeed;



	void Start(){
		customText.enabled = false;
		_playerScript = myPlayer.GetComponent<BoatMovement>();

		//waterLevel = transform.position.y + (transform.localScale.y) / 2; //as it stands should be equal to -0.35f
		wristRB = wristTrackerAUX.GetComponent<Rigidbody>();
		playerRB = myPlayer.GetComponent<Rigidbody>();

	}

	void FixedUpdate()
	{
		wristTrackerAUX.transform.position = wristTracker.transform.position;
		paddleSpeed = wristRB.velocity.sqrMagnitude;

	}



	void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = true;
			waterLevel = rowCollider.transform.position.y;
			


			////---------------------------acho que nao e preciso nada disto
			//if (gameObject.name == "R_RowCollider")
			//{
			//	_playerScript.turnRight = true;
			//}
			//else if (gameObject.name == "L_RowCollider")
			//{
			//	_playerScript.turnLeft = true;
			//}
			////---------------------------
		}
	}


    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			paddleDepth = waterLevel - rowCollider.transform.position.y;
			//paddleSpeed = wristRB.velocity.sqrMagnitude; //------------------------------------------------------------------------------------------
			Debug.Log(playerRB.velocity);
			if (paddleDepth > 0) rotationSpeed = Mathf.Abs(paddleDepth*2); //* paddleSpeed
			else rotationSpeed = 0;
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
