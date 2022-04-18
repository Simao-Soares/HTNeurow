using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays Row + Movement V3

public class DisplayRow : MonoBehaviour
{
    [SerializeField] Text customText;

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

	public Sound rowAudio;



	void Start(){
		customText.enabled = false;
		_playerScript = myPlayer.GetComponent<BoatMovement>();

		//waterLevel = transform.position.y + (transform.localScale.y) / 2; //as it stands should be equal to -0.35f
		wristRB = wristTrackerAUX.GetComponent<Rigidbody>();
		playerRB = myPlayer.GetComponent<Rigidbody>();

		wristTrackerAUX.transform.position = wristTracker.transform.position;
		currWristPos = wristTrackerAUX.transform.localPosition.z;
		prevWristPos = currWristPos;





		rowAudio.source = GetComponent<AudioSource>();

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
			//Rowing directional audio + Rowing event Log
			if (gameObject.name == "R_RowCollider")
			{
				rowAudio.source.panStereo = 0.8f;
				DataLogger.RowR = "Row Start";
			}

			else
			{
				rowAudio.source.panStereo = -0.8f;
				DataLogger.RowL = "Row Start";
			}

			rowAudio.source.Play();

			customText.enabled = true;
			waterLevel = rowCollider.transform.position.y;
			
		}
	}


    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			paddleDepth = waterLevel - rowCollider.transform.position.y;

			if (paddleDepth > 0) //not sure if necessary
			{
				rotationSpeed = Mathf.Abs(GameManager.turnSense * paddleDepth * paddleSpeed);
				if (gameObject.name == "R_RowCollider") DataLogger.RowR = (rotationSpeed/GameManager.turnSense).ToString(); //divided by sense to normalize for every parameter choice
				else DataLogger.RowL = (rotationSpeed / GameManager.turnSense).ToString();
			} 
			else rotationSpeed = 0;
		}
	}	



    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
			if (gameObject.name == "R_RowCollider") DataLogger.RowR = "Row End";
			else DataLogger.RowL = "Row End";

			customText.enabled = false;
			_playerScript.turnLeft = false;
			_playerScript.turnRight = false;
		}
    }
}
