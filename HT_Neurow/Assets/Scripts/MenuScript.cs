using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
	[Header("Camera and Scene transitions")]
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
	[HideInInspector]public bool updateSettingsAux = false;
	[HideInInspector] public bool editPreset = false;


	[Header("Toggles aka Buttons")]
	public Toggle editButton;
	public Toggle controlButtonBCI;
	public Toggle controlButtonHT;

	public Toggle hemiButtonRight; //default is none selected
	public Toggle hemiButtonLeft;

	public Toggle genderButtonM;
	public Toggle genderButtonF;

	//public Button forwardButtonA;
	//public Button forwardButtonM;

	public Toggle axisButtonX;
	public Toggle axisButtonY;
	public Toggle axisButtonZ;

	public Toggle invertButton;
	public Toggle bciSpecificButton;

	public Toggle[] presetButtons;



	[Header("Text elements of sliders")]
	public TMP_InputField durationTextMinutes;
	public TMP_InputField durationTextSeconds;

	public TMP_Text motionRangeText;
	public TMP_Text colliderSizeText;

	public TMP_Text turnAngleText;
	public TMP_Text boatSpeedText;
	public TMP_Text turnSpeedText;
	public TMP_Text turnSenseText;

	public TMP_Text challengeLevelText;
	public TMP_Text angleDevText;
	public TMP_Text maxDistanceText;
	public TMP_Text maxDistance2Text;
	public TMP_Text selfCorrectText;
	public TMP_Text autoCorrectText;

	public TMP_Text playAreaText;
	public TMP_Text objectiveNumText;
	public TMP_Text objectiveRadText;
	public TMP_Text objectivePosX;
	public TMP_Text objectivePosZ;


	

	[System.Serializable]
	public class BlockList
	{
		public GameObject[] BlockObj;
	}

	[Header("Blocking Panels")]
	public BlockList BCI_BlockList = new BlockList();
	public BlockList HT_BlockList = new BlockList();



	private void Update()
    {
		if (updateSettingsAux)
		{
			UpdateSettings();
			updateSettingsAux = false;
		}

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

		if (GameManager.ControlMethod == 1)
		{
			controlButtonBCI.SetIsOnWithoutNotify(true);
			//Deactivate locks on BCI Parameters 
			for (int i = 0; i < BCI_BlockList.BlockObj.Length; i++)
            {
				BCI_BlockList.BlockObj[i].SetActive(true);
			}
			//Activate locks on HT Parameters 
			for (int i = 0; i < HT_BlockList.BlockObj.Length; i++)
			{
				HT_BlockList.BlockObj[i].SetActive(false);
			}
		}
		else
		{
			controlButtonHT.SetIsOnWithoutNotify(true);
			//Deactivate locks on HT Parameters 
			for (int i = 0; i < HT_BlockList.BlockObj.Length; i++)
			{
				HT_BlockList.BlockObj[i].SetActive(true);
			}
			//Activate locks on BCI Parameters 
			for (int i = 0; i < BCI_BlockList.BlockObj.Length; i++)
			{
				BCI_BlockList.BlockObj[i].SetActive(false);
			}
		}

		if (GameManager.Gender == 1) genderButtonM.SetIsOnWithoutNotify(true);
		else genderButtonF.SetIsOnWithoutNotify(true);

		if (GameManager.HemiLimb == 1) hemiButtonRight.SetIsOnWithoutNotify(true);
		else if (GameManager.HemiLimb == -1) hemiButtonLeft.SetIsOnWithoutNotify(true);
		else if (GameManager.HemiLimb == 2) {
			hemiButtonLeft.SetIsOnWithoutNotify(true);
			hemiButtonRight.SetIsOnWithoutNotify(true);
		}
		else if (GameManager.HemiLimb == 0)
		{
			hemiButtonLeft.SetIsOnWithoutNotify(false);
			hemiButtonRight.SetIsOnWithoutNotify(false);
		}

		if (GameManager.trackAxis == 0) axisButtonX.SetIsOnWithoutNotify(true);
		else if (GameManager.trackAxis == 1) axisButtonY.SetIsOnWithoutNotify(true);
		else if (GameManager.trackAxis == -1) axisButtonZ.SetIsOnWithoutNotify(true);

		if (!GameManager.invertTurn) invertButton.SetIsOnWithoutNotify(true); //instead of changing this value when it was red I just changed it here
		if (GameManager.bciSpecificUI) bciSpecificButton.SetIsOnWithoutNotify(true);

		//Debug.Log(GameManager.SelectedPreset);
		switch (GameManager.SelectedPreset)
		{
			case 0:
				presetButtons[0].SetIsOnWithoutNotify(true);
				break;
			case 1:
				presetButtons[1].SetIsOnWithoutNotify(true);
				break;
			case 2:
				presetButtons[2].SetIsOnWithoutNotify(true);
				break;
			case 3:
				presetButtons[3].SetIsOnWithoutNotify(true);
				break;
			case 4:
				presetButtons[4].SetIsOnWithoutNotify(true);
				break;
			case 5:
				presetButtons[5].SetIsOnWithoutNotify(true);
				break;
		}

		//if (GameManager.Forward == 1) forwardButtonA.Select();
		//else forwardButtonM.Select();

		var taskTime = GameManager.taskDuration;
		float minutes = Mathf.FloorToInt(taskTime / 60);
		float seconds = Mathf.FloorToInt(taskTime % 60);

		//durationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

		durationTextMinutes.text = minutes.ToString();
		durationTextSeconds.text = string.Format("{0:00}", seconds);

		motionRangeText.text = GameManager.motionRange.ToString();
		colliderSizeText.text = GameManager.colliderSize.ToString();

		turnAngleText.text = GameManager.turnAngle.ToString();
		boatSpeedText.text = GameManager.boatSpeed.ToString();
		turnSpeedText.text = GameManager.turnSpeed.ToString();
		turnSenseText.text = GameManager.turnSense.ToString();

		challengeLevelText.text = GameManager.challengeLevel.ToString();
		angleDevText.text = GameManager.angleDev.ToString();
		maxDistanceText.text = GameManager.maxDistance.ToString();
		maxDistance2Text.text = GameManager.maxDistance2.ToString();
		selfCorrectText.text = GameManager.selfCorrect.ToString();
		autoCorrectText.text = GameManager.autoCorrect.ToString();

		playAreaText.text = GameManager.playArea.ToString();
		objectiveNumText.text = GameManager.objectiveNum.ToString();
		objectiveRadText.text = GameManager.objectiveRad.ToString();
		objectivePosX.text = GameManager.objectivePosX.ToString();
		objectivePosZ.text = GameManager.objectivePosZ.ToString();
	}

}
