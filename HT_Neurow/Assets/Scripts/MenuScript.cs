using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
    //Variables for camera ao scene transitions
    public GameObject mainCam;
    public GameObject optCam;
	public GameObject optBackCam;
    public GameObject optPanel;
    public GameObject mainPanel;
	public GameObject buttons;
	public GameObject titleElements;

	public GameObject chooseTaskPannel;

    private bool playAux = false;
	private bool optAux = false;
	private bool optBackAux = false;




	//Variables for default game settings
	public Button controlButtonBCI;
	public Button controlButtonHT;
	public Button hemiButtonRight; //default is none selected
	public Button hemiButtonLeft;
	public Button genderButtonM;
	public Button genderButtonF;
	//public Button forwardButtonA;
	//public Button forwardButtonM;
	public Button axisButtonX;
	public Button axisButtonY;
	public Button axisButtonZ;





	//Slider Values
	public TMP_Text durationText;
	public TMP_Text motionRangeText;
	public TMP_Text colliderSizeText;

	public TMP_Text turnAngleText;
	public TMP_Text boatSpeedText;
	public TMP_Text turnSpeedText;

	public TMP_Text challengeLevelText;
	public TMP_Text angleDevText;
	public TMP_Text maxDistanceText;
	public TMP_Text maxDistance2Text;
	public TMP_Text selfCorrectText;
	public TMP_Text autoCorrectText;

	public TMP_Text playAreaText;
	public TMP_Text objectiveNumText;
	public TMP_Text objectiveRadText;







	private void Update()
    {

		if (optPanel.activeSelf) UpdateSettings();

		if (optAux) StartCoroutine(Options());
		if (playAux) StartCoroutine(StartGame());
		if (optBackAux) StartCoroutine(OptBack());
		
	}

	public void QuitGame(){
		Application.Quit();
	}


	public void StartPressed(){
		playAux = true;
	}

	public void OptPressed(){
		optAux = true;
		GameManager.optionsMenuAux = true; //-----------> to update parameter values
	}

	public void OptBackPressed(){
		optBackAux = true;
		GameManager.optionsMenuAux = false;
	}

	public void BackFromChosing()
    {
		SceneManager.LoadScene("Menu");
    }



	//public void TaskChoice(int task)
	//   {
	//	taskChoiceAux = task;
	//	SceneManager.LoadScene("RowingSim");
	//}


	IEnumerator StartGame(){
		buttons.SetActive(false);
		mainCam.GetComponent<CPC_CameraPath>().PlayPath(3);
		playAux = false;
		yield return new WaitForSeconds(3f);
		titleElements.SetActive(false);
		yield return new WaitForSeconds(.1f);

		chooseTaskPannel.SetActive(true);

        
    }
		

    IEnumerator Options(){
		optCam.SetActive(true);
        mainCam.SetActive(false);
        mainPanel.SetActive(false);
        optCam.GetComponent<CPC_CameraPath>().PlayPath(3);
		optAux = false;
		yield return new WaitForSeconds(3f);
		optPanel.SetActive(true);
    }

	IEnumerator OptBack(){
        optBackCam.SetActive(true);
        optCam.SetActive(false);
		optPanel.SetActive(false);
		optBackCam.GetComponent<CPC_CameraPath>().PlayPath(1);
		optBackAux = false;
		yield return new WaitForSeconds(1f);
        mainPanel.SetActive(true);
		optBackCam.SetActive(false);
		mainCam.SetActive(true);
		
    }

	private void UpdateSettings()
	{
		if (GameManager.ControlMethod == 1) controlButtonBCI.Select();
		else controlButtonHT.Select();

		if (GameManager.Gender == 1) genderButtonM.Select();
		else genderButtonF.Select();

		if (GameManager.HemiLimb == 1) hemiButtonRight.Select();
		else if (GameManager.HemiLimb == -1) hemiButtonLeft.Select();
		else if (GameManager.HemiLimb == 2) {
			hemiButtonLeft.Select();
			hemiButtonRight.Select();
		}

		if (GameManager.trackAxis == 0) axisButtonX.Select();
		else if (GameManager.trackAxis == 1) axisButtonY.Select();
		else if (GameManager.trackAxis == -1) axisButtonZ.Select();


		//if (GameManager.Forward == 1) forwardButtonA.Select();
		//else forwardButtonM.Select();

		var taskTime = GameManager.taskDuration;
		float minutes = Mathf.FloorToInt(taskTime / 60);
		float seconds = Mathf.FloorToInt(taskTime % 60);
		durationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

		motionRangeText.text = GameManager.motionRange.ToString();
		colliderSizeText.text = GameManager.colliderSize.ToString();

		turnAngleText.text = GameManager.turnAngle.ToString();
		boatSpeedText.text = GameManager.boatSpeed.ToString();
		turnSpeedText.text = GameManager.turnSpeed.ToString();

		challengeLevelText.text = GameManager.challengeLevel.ToString();
		angleDevText.text = GameManager.angleDev.ToString();
		maxDistanceText.text = GameManager.maxDistance.ToString();
		maxDistance2Text.text = GameManager.maxDistance2.ToString();
		selfCorrectText.text = GameManager.selfCorrect.ToString();
		autoCorrectText.text = GameManager.autoCorrect.ToString();

		playAreaText.text = GameManager.playArea.ToString();
		objectiveNumText.text = GameManager.objectiveNum.ToString();
		objectiveRadText.text = GameManager.objectiveRad.ToString();
	}

}
