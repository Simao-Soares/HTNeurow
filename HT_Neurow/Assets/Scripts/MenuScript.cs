using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    
    public GameObject mainCam;
    public GameObject optCam;


    public GameObject optPanel;

    public GameObject mainPanel;

	public GameObject buttons;
	public GameObject titleElements;

    private bool playAux = false;
	private bool optAux = false;


	public void QuitGame(){
		Application.Quit();
	}

	private void Update() {
		if(optAux) StartCoroutine(Options());
		if(playAux) StartCoroutine(StartGame());
	}

	public void StartPressed(){
		playAux = true;
	}

	public void OptPressed(){
		optAux = true;
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


}
