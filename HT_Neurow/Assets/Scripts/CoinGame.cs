using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinGame : MonoBehaviour
{

    public GameObject instructions;
    public GameObject tempScoreUI;
    public GameObject scoreUI;

    public TMP_Text playerScoreText;
    private int playerScore = 0;
    
    public float instructionsTime;
    private bool auxI;


    // Start is called before the first frame update
    void Start()
    {
        instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        auxI = true;
    }

    void FixedUpdate()
    {
        //always show instructions for instructionsTime seconds at the start
        if(auxI == true) StartCoroutine(ShowInstructions(instructionsTime));
        //show instructions for instructionsTime seconds when i key is pressed
        if(Input.GetKeyDown("i")) StartCoroutine(ShowInstructions(instructionsTime)); //not perfect but enough
    }

    IEnumerator ShowInstructions(float time){
		instructions.SetActive(true);
        tempScoreUI.SetActive(true);
		yield return new WaitForSeconds(time);
        auxI = false;
		instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        //scoreUI.transform.localPosition = new Vector3(0,450f,0);  //tried to move the score up a bit but didnt work oh well
	}

    public void Score(){
        playerScore = playerScore + 1;
        this.playerScoreText.text = playerScore.ToString();
    }


    
            



}
