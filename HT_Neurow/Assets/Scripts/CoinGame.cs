using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject boat;
    public GameObject GameOverUI;
    public GameObject NormalUI;

    public int numberOfCoins;
    public TMP_Text playerScoreText;
    public TMP_Text finalScoreText;
    private int playerScore = 0;

    public float instructionsTime;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    public TMP_Text timeText;


    private bool auxI;

    public class CoinClass {
        public int CoinID;
        public GameObject CoinObject;

    }
    public List<CoinClass> listCoins = new List<CoinClass>();

    private int auxList;

    //Screen Freeze Couroutine
    public float freezeDuration = 10f;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;





    // Start is called before the first frame update
    void Start()
    {
        instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        auxI = true;
        GenerateObjectives();
        timerIsRunning = true;
    }

    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                //always show instructions for instructionsTime seconds at the start
                if (auxI == true) StartCoroutine(ShowInstructions(instructionsTime));
                //show instructions for instructionsTime seconds when i key is pressed
                if (Input.GetKeyDown("i")) StartCoroutine(ShowInstructions(instructionsTime)); //not perfect but enough

                auxList = UpdateList();
                if (auxList != -1) GenerateNewObjective(auxList);
            }
            else //TIME IS UP
            {
                Debug.Log("Time has run out!");  
                timeRemaining = 0;
                timerIsRunning = false;
                NormalUI.SetActive(false);
                this.finalScoreText.text = playerScore.ToString();

                GameOverUI.SetActive(true);

                pendingFreezeDuration = freezeDuration; //freeze screen on GameOver
                if (pendingFreezeDuration > 0 && !isFrozen) StartCoroutine(DoFreeze());

                
            }
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator ShowInstructions(float time) {
        instructions.SetActive(true);
        tempScoreUI.SetActive(true);
        yield return new WaitForSeconds(time);
        auxI = false;
        instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        //scoreUI.transform.localPosition = new Vector3(0,450f,0);  //tried to move the score up a bit but didnt work 
    }

    public void Score() {
        playerScore = playerScore + 1;
        this.playerScoreText.text = playerScore.ToString();
    }

    public static CoinClass FactoryOfCoin(int coinID)
    {
        CoinClass newCoin = new CoinClass();
        newCoin.CoinID = coinID;
        return newCoin;
    }

    private void GenerateObjectives() {
        //List<CoinClass> listCoins = new List<CoinClass>();
        float minDistanceToBoat = Mathf.Sqrt(minDistance * minDistance + 50f * 50f);

        for (int i = 0; i < numberOfCoins; i++)
        {
            listCoins.Add(FactoryOfCoin(i));
            Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));

            if (listCoins.Count > 0)
            {
                for (int j = 0; j < i; j++)        
                {
                    while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance || Vector3.Distance(boat.transform.position, newCoords) < minDistanceToBoat)
                    {                      
                        newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100.5f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));
                    }
                }
            }
            GameObject aux = Instantiate(Coin, newCoords, Quaternion.identity);
            Debug.Log(Vector3.Distance(boat.transform.position, newCoords));
            listCoins[i].CoinObject = aux;
        }
    }

    private void GenerateNewObjective(int newID) {
        //List<CoinClass> listCoins = new List<CoinClass>();

        listCoins.Add(FactoryOfCoin(newID)); //add new element to end of the list with CoinID of element that was removed
        Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));

        for (int j = 0; j < listCoins.Count - 1; j++) {
            while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance)
            {
                newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));
            }
        }
        GameObject aux = Instantiate(Coin, newCoords, Quaternion.identity);
        listCoins[listCoins.Count - 1].CoinObject = aux; //is listCoins.Count-1 the last position of the list? I think so
    }

    private int UpdateList() {
        for (int i = 0; i < listCoins.Count; i++)
        {
            if (listCoins[i].CoinObject == null) //verify if any coin has been destroyed but hasnt been eliminated from the list yet
            {
                int auxUpdate = listCoins[i].CoinID;
                listCoins.Remove(listCoins[i]);
                //PrintList();
                //Debug.Log(i);
                return (auxUpdate); //position on list corresponds to listCoins[i].CoinID 
            }
        }
        return (-1);
    }

    //aux function for debugging
    void PrintList() {
        for (int i = 0; i < listCoins.Count; i++)
        {
            Debug.Log(listCoins[i].CoinID);

        }

    }


    IEnumerator DoFreeze()
    {
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = original;
        pendingFreezeDuration = 0;
        isFrozen = false;
        SceneManager.LoadScene("Menu");
    }



}