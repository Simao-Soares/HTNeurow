using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class CoinGame : MonoBehaviour
{

    public GameObject instructions;
    public GameObject tempScoreUI;
    public GameObject scoreUI;
    public float coinGameArea = 400f; // length of the side of playable area
    public float distanceBetween = 50f;

    public GameObject Coin;

    public int numberOfCoins;

    public TMP_Text playerScoreText;
    private int playerScore = 0;
    
    public float instructionsTime;
    private bool auxI;

    public class CoinClass{
        public string CoinID;
        public GameObject CoinObject;

    }
    //Array of Coins aka Objectives
/* 	[System.Serializable]
	public class CoinList
	{
		public CoinClass[] coinClass;
	} */

	//public CoinList myCoinList = new CoinList();  


    // Start is called before the first frame update
    void Start()
    {
        instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        auxI = true;
        GenerateObjectives();
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
        //scoreUI.transform.localPosition = new Vector3(0,450f,0);  //tried to move the score up a bit but didnt work 
	}

    public void Score(){
        playerScore = playerScore + 1;
        this.playerScoreText.text = playerScore.ToString();
    }

    public static CoinClass FactoryOfCoin(string coinID)
    {
        CoinClass newCoin = new CoinClass();
        newCoin.CoinID = coinID;
        //Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));
       
        //newCoin.CoinObject.transform = newCoords;
        return newCoin;
    }
        

    private void GenerateObjectives(){
        List<CoinClass> listCoins = new List<CoinClass>();

        for (int i = 0; i < numberOfCoins; i++)
        {
            listCoins.Add(FactoryOfCoin(i.ToString()));
            Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));

            if(listCoins.Count > 0)
            {
                for (int j = 0; j < i; j++)
                {
                    while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < distanceBetween)
                    {
                        newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));
                    }
                }
            }
            GameObject aux = Instantiate(Coin, newCoords, Quaternion.identity);
            listCoins[i].CoinObject = aux;
        }

    }


    
            



}
