using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	public Button forwardButtonA;
	public Button forwardButtonM;






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
	}

	public void OptBackPressed(){
		optBackAux = true;
	}


    IEnumerator StartGame(){
		buttons.SetActive(false);
		mainCam.GetComponent<CPC_CameraPath>().PlayPath(3);
		playAux = false;
		yield return new WaitForSeconds(3f);
		titleElements.SetActive(false);
		yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene("RowingSim");
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

		if (GameManager.Forward == 1) forwardButtonA.Select();
		else forwardButtonM.Select();

	}

}
