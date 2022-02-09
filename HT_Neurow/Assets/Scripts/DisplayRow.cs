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

	[HideInInspector] public float prevWristPos;
	[HideInInspector] public float currWristPos;



	void Start(){
		customText.enabled = false;
		_playerScript = myPlayer.GetComponent<BoatMovement>();

		//waterLevel = transform.position.y + (transform.localScale.y) / 2; //as it stands should be equal to -0.35f
		wristRB = wristTrackerAUX.GetComponent<Rigidbody>();
		playerRB = myPlayer.GetComponent<Rigidbody>();

		wristTrackerAUX.transform.position = wristTracker.transform.position;
		currWristPos = wristTrackerAUX.transform.localPosition.z;
		prevWristPos = currWristPos;

	}

	void FixedUpdate()
	{
		wristTrackerAUX.transform.position = wristTracker.transform.position;
		currWristPos = wristTrackerAUX.transform.localPosition.z;

		var deltaPos = (Mathf.Abs(prevWristPos) - Mathf.Abs(currWristPos));
		if (deltaPos > 0.001f) paddleSpeed = Mathf.Round((deltaPos / Time.deltaTime) * 10f) / 10f;
		else paddleSpeed = 0;
		prevWristPos = currWristPos;

	}



	void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            customText.enabled = true;
			waterLevel = rowCollider.transform.position.y;
		}
	}


    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			paddleDepth = waterLevel - rowCollider.transform.position.y;

			if (paddleDepth > 0) rotationSpeed = Mathf.Abs(GameManager.turnSense*paddleDepth*paddleSpeed); 
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
