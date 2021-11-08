using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void StartGame(){
        float aux = Time.deltaTime;
        while (true){
            if(Time.deltaTime - aux > 3f){
                SceneManager.LoadScene("RowingSim");
            }
        }
    }

    public void QuitGame(){
        Application.Quit();
    }
}
