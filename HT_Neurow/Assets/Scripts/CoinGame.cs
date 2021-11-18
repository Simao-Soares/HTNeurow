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
    public float minDistance = 50f;

    public GameObject Coin;

    public int numberOfCoins; 
    public TMP_Text playerScoreText;
    private int playerScore = 0;
    
    public float instructionsTime;
    private bool auxI;

    public class CoinClass{
        public int CoinID;
        public GameObject CoinObject;

    }
    public List<CoinClass> listCoins = new List<CoinClass>();

    private int auxList;

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

        
        auxList = UpdateList();
        
        //auxList = numberOfCoins - listCoins.Count;
        //Debug.Log(auxList);

        if(auxList != -1){
             GenerateNewObjective(auxList);
        }
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



    public static CoinClass FactoryOfCoin(int coinID)
    {
        CoinClass newCoin = new CoinClass();
        newCoin.CoinID = coinID;
        return newCoin;
    }
        

    private void GenerateObjectives(){
        //List<CoinClass> listCoins = new List<CoinClass>();

        for (int i = 0; i < numberOfCoins; i++)
        {
            listCoins.Add(FactoryOfCoin(i));
            Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));

            if(listCoins.Count > 0)
            {
                for (int j = 0; j < i; j++)
                {
                    while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance)
                    {
                        newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));
                    }
                }
            }
            GameObject aux = Instantiate(Coin, newCoords, Quaternion.identity);
            listCoins[i].CoinObject = aux;
        }
    }

    private void GenerateNewObjective(int newID){
        //List<CoinClass> listCoins = new List<CoinClass>();

        listCoins.Add(FactoryOfCoin(newID)); //add new element to end of the list with CoinID of element that was removed
        Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));

        for (int j = 0; j < listCoins.Count-1; j++){ 
            while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance)
            {
                newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2), 100f, UnityEngine.Random.Range(-coinGameArea/2, coinGameArea/2));
            }
        }
        GameObject aux = Instantiate(Coin, newCoords, Quaternion.identity);
        listCoins[listCoins.Count-1].CoinObject = aux; //is listCoins.Count-1 the last position of the list? I think so
    }

    private int UpdateList(){ 
        for (int i = 0; i < listCoins.Count; i++)
        {
            if(listCoins[i].CoinObject == null) //verify if any coin has been destroyed but hasnt been eliminated from the list yet
            {
                int auxUpdate = listCoins[i].CoinID;
                listCoins.Remove(listCoins[i]);
                //PrintList();
                //Debug.Log(i);
                return(auxUpdate); //position on list corresponds to listCoins[i].CoinID 
            }
        }
        return(-1);
    }
    
    void PrintList(){
        for (int i = 0; i < listCoins.Count; i++)
        {
            Debug.Log(listCoins[i].CoinID);

        }

    }
}
