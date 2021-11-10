using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    
    public GameObject main;
    public GameObject optCamera;


    public GameObject optPanel;

    public GameObject mainPanel;

    private bool playAux = false;
    
    
    
    public void StartGame(){
        SceneManager.LoadScene("RowingSim");
    }

    public void StartPressed(){
        playAux = true;
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Options(){
        optCamera.SetActive(true);
        main.SetActive(false);

        mainPanel.SetActive(false);
        optCamera.GetComponent<CPC_CameraPath>().PlayPath(3);

        
        optPanel.SetActive(true);

    }

    private void Update() {
        //COROUTINE WITH PlayIsPressed
    }


}
