using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = System.Random;
using Assets.LSL4Unity.Scripts.Examples;
using Assets.LSL4Unity.Scripts.AbstractInlets;

public class CoinGame : MonoBehaviour
{
    [Header("Main Task Parameters")]
    public float timeRemaining = 10;
    public float coinGameArea = 1000f; // length of the side of playable area -- overwritten in SetParameters()
    public float minDistance = 100f;
    public int coinRad = 10;
    public int numberOfCoins = 3;
    public float instructionsTime;
    public float coinShrinkStep = 0.5f;

    [Header("Assitive Parameters and UI elements")] // <---------------------------------------------------------------------------------
    public bool assistiveMechs;
    public Camera mainCamera;
    public float maxDistance;
    public float autoCorrectSpeed;
    [HideInInspector] public float maxAngleDev;
    public GameObject arrowRight;
    public GameObject arrowLeft;
    public float selfCorrectTime = 10f;
    public GameObject selfCorrectTimer;
    [HideInInspector] public bool auxCorrection = false;
    [HideInInspector] public bool auxSelfCorrect = false;
    [HideInInspector] public bool stopTimer;
    public float correctionSpeed = 0.5f;
    private Quaternion correctionRotation;
    public TMP_Text correctTimerText;
    private Rigidbody rb;

    //read by DataLogger
    [HideInInspector] public string assistAux = "NULL";
    [HideInInspector] public float difficultyAux;

    [HideInInspector] public float newRadius;



    [Header("Main Task Assets")]
    public GameObject CoinObject; 
    public GameObject buoy; 
    public GameObject boat;

    [Header("Main Task UI")]
    public GameObject instructions;
    public GameObject tempScoreUI;
    public GameObject scoreUI;
    public GameObject GameOverUI;
    public GameObject NormalUI;
    public TMP_Text playerScoreText;
    public TMP_Text finalScoreText;
    public TMP_Text timeText;


    [HideInInspector] public bool timerIsRunning = false;

    private int playerScore = 0;
    private bool auxI;
    private int auxList;

    public class CoinClass {
        public int CoinID;
        public GameObject CoinObject;

    }
    public List<CoinClass> listCoins = new List<CoinClass>();
    

    [Header("Screen Freeze Couroutine")]
    public float freezeDuration = 10f;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;

    [Header("BCI METHOD - objective position")]
    public float bciDistanceX;
    public float bciDistanceZ;


    [Header("BCI METHOD - UI ")]

    public bool training = true; //if false -> online

    public RawImage bciCross;
    public RawImage bciLeftArrow;
    public RawImage bciRightArrow;
    [HideInInspector] public static bool cross, left, right, hidearrow = false;


    [HideInInspector] public Vector3 bciObjectiveLocation;
    [HideInInspector] public int auxBCIturning = 0;     //0 -> ready to spawn new objective    //-1 -> left      1 -> right
    [HideInInspector] public bool turnCourotineAux = false;
    [HideInInspector] public Quaternion bciRotation = Quaternion.identity;
    [HideInInspector] BoatMovement movementScript;
    [HideInInspector] Animator R_rowAnimator;
    [HideInInspector] Animator L_rowAnimator;
    [HideInInspector] GameObject auxObjInstance;
    [HideInInspector] GameObject auxBuoyInstance;

    [HideInInspector] public float buoyHeight = 99.7f;

    //REGISTER GAME EVENT
    [HideInInspector] public string gameEventAux;
    [HideInInspector] public float gameEventDistance;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        instructions.SetActive(false);
        tempScoreUI.SetActive(false);
        auxI = true;
        movementScript = GetComponent<BoatMovement>();

        SetParameters();

        //HT
        if (GameManager.ControlMethod == -1)
        {
            maxAngleDev = mainCamera.fieldOfView; //used by the assistive mechanisms
            GenerateObjectives();
            movementScript.enabled = true;
        }

        //BCI
        if (training && GameManager.ControlMethod == 1)
        {
            tempScoreUI.SetActive(false);
            scoreUI.SetActive(false);
        }

        R_rowAnimator = movementScript.R_rowAnimator;
        L_rowAnimator = movementScript.L_rowAnimator;
        bciCross.enabled = false;
        bciLeftArrow.enabled = false;
        bciRightArrow.enabled = false;

        timerIsRunning = true;
    }

    void SetParameters()
    {
        //CoinLighting.SetActive(true);
        assistiveMechs = GameManager.assistiveMechs;
        timeRemaining = GameManager.taskDuration;

        coinGameArea =  50 + 15*(GameManager.playArea-1);

        numberOfCoins = GameManager.objectiveNum;

        maxDistance = GameManager.maxDistanceBuoy;
        selfCorrectTime = GameManager.selfCorrectBuoy;
        autoCorrectSpeed = GameManager.autoCorrectBuoy/5;

        bciDistanceX = GameManager.objectivePosX;
        bciDistanceZ = GameManager.objectivePosZ;

        CoinObject.transform.localScale = new Vector3 (GameManager.objectiveRad, 100, GameManager.objectiveRad);           

        CoinObject.GetComponent<PickedUp>().shrinkStep = GameManager.objectiveRad;

        newRadius = GameManager.objectiveRad;


    }

    void Update()
    {
        
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                if(GameManager.ControlMethod == -1) timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                //always show instructions for instructionsTime seconds at the start
                if (auxI == true) StartCoroutine(ShowInstructions(instructionsTime));
                //show instructions for instructionsTime seconds when i key is pressed
                if (Input.GetKeyDown("i")) StartCoroutine(ShowInstructions(instructionsTime)); //not perfect but enough

                //--------------------------------------------------------------------------------------------------------------
                if (GameManager.ControlMethod == -1)//HT
                {
                    auxList = UpdateList();
                    if (auxList != -1) GenerateNewObjective(auxList);

                    //REGISTER GAME EVENT
                    gameEventDistance = FindClosestBuoy()[1].x;
                    if (gameEventDistance <= GameManager.objectiveRad) gameEventAux = "pickedUp"; //no need to rely on collider which could be messy due to capture frequency
                    else gameEventAux = gameEventDistance.ToString(); //distance to closest buoy

                    difficultyAux = newRadius;


                    if (!auxI && assistiveMechs)//start checking for assistive mechanisms only when instructions disapear AND they are enabled
                    {
                        if (CheckMaxAngleDev())
                        {
                            assistAux = "NULL";
                            TurnOffArrows();
                            auxCorrection = false;
                            auxSelfCorrect = false;
                            movementScript.enabled = true;

                            stopTimer = false;
                            selfCorrectTimer.SetActive(false);
                            StopCoroutine("SelfCorrection");
                            movementScript.selfCorrection = false;
                        }

                        if (auxSelfCorrect)
                        {
                            StartCoroutine("SelfCorrection");
                            assistAux = "SELF";
                        }
                        if (auxCorrection)
                        {
                            assistAux = "AUTO";
                            StopCoroutine("SelfCorrection");
                            StartCoroutine(CorrectionCoroutine(autoCorrectSpeed, correctionRotation));
                            arrowLeft.SetActive(false);
                            arrowRight.SetActive(false);
                        }
                    }
                    
                   
                }

                //--------------------------------------------------------------------------------------------------------------
                else if (GameManager.ControlMethod == 1)//BCI
                {
                    //Check for BCI input (training)
                    //Debug.Log("GETASTIM!");
                    getStim();

                    //Turning Towads Objective
                    if(turnCourotineAux) StartCoroutine(BCITurning(GameManager.turnSpeed, bciRotation));

                    //Spawn Objective
                    if (auxBCIturning == 0) OneObjective();
                    else
                    {
                        Vector3 vectorToObjective = (bciObjectiveLocation - new Vector3(0, 99.9f - 0.4f, 0)) - transform.position;


                        bciRotation = Quaternion.LookRotation(vectorToObjective, new Vector3(0, 1, 0));
                        float angleToObjective = Vector3.Angle(transform.forward, vectorToObjective);

                        //Check for right user input
                        if ((Input.GetKey(KeyCode.RightArrow) || (right && hidearrow)) && auxBCIturning == 1 ||
                            (Input.GetKey(KeyCode.LeftArrow)  || (left && hidearrow))  && auxBCIturning == -1) 
                        {
                            turnCourotineAux = true;
                            //movementScript.selfCorrection = true; 
                            if(GameManager.invertTurn && auxBCIturning == 1) R_rowAnimator.SetBool("Turning", true);
                            else if(!GameManager.invertTurn && auxBCIturning == 1) L_rowAnimator.SetBool("Turning", true);
                            else if(GameManager.invertTurn && auxBCIturning == -1) L_rowAnimator.SetBool("Turning", true);
                            else if (!GameManager.invertTurn && auxBCIturning == -1) R_rowAnimator.SetBool("Turning", true);
                        }
                        //Check for wrong user input
                        else if (Input.GetKey(KeyCode.RightArrow) && auxBCIturning == -1 || Input.GetKey(KeyCode.LeftArrow) && auxBCIturning == 1)
                        {
                            Debug.Log("tried to turn to the wrong side");
                        }

                        //Check for rotation completion or if the boat already passed by objective                          
                        if (angleToObjective < 5f || Vector3.Dot(bciObjectiveLocation - new Vector3(0, 99.9f - 0.4f, 0) - transform.position, transform.forward) < 0.5)
                        {
                            turnCourotineAux = false;
                            auxBCIturning = 0;
                            //movementScript.selfCorrection = false;
                            R_rowAnimator.SetBool("Turning", false);
                            L_rowAnimator.SetBool("Turning", false);
                            if (Vector3.Dot(bciObjectiveLocation - new Vector3(0, 99.9f - 0.4f, 0) - transform.position, transform.forward) < 0.5) Destroy(auxObjInstance);
                        }
                    }
                }

                //--------------------------------------------------------------------------------------------------------------
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

    //BCI
    void OneObjective()
    {
        Transform boatT = boat.transform;
        var height = 99.5f * Vector3.up;

        if (Input.GetKeyUp(KeyCode.E) || right)
        {
            auxBCIturning = 1;
            bciObjectiveLocation = (boatT.position + boatT.forward * bciDistanceZ + boatT.right * bciDistanceX + height);
            auxObjInstance = Instantiate(CoinObject, bciObjectiveLocation, Quaternion.identity);
            auxBuoyInstance = Instantiate(buoy, bciObjectiveLocation - new Vector3(0, buoyHeight, 0), Quaternion.Euler(-90, 0, 0));
        }

        else if (Input.GetKeyUp(KeyCode.Q) || left)
        {
            auxBCIturning = -1;
            bciObjectiveLocation = (boatT.position + boatT.forward * bciDistanceZ - boatT.right * bciDistanceX + height);
            auxObjInstance = Instantiate(CoinObject, bciObjectiveLocation, Quaternion.identity);
            auxBuoyInstance = Instantiate(buoy, bciObjectiveLocation - new Vector3(0, buoyHeight, 0), Quaternion.Euler(-90, 0, 0));
        }

    }

    IEnumerator BCITurning(float speed, Quaternion rotation)
    {
        Quaternion current = transform.rotation;
        transform.localRotation = Quaternion.Lerp(current, rotation, speed * Time.deltaTime);
        yield return null;
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
        if(!training) tempScoreUI.SetActive(true);
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

        for (int i = 0; i < numberOfCoins; i++)
        {
            listCoins.Add(FactoryOfCoin(i));
            Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));

            if (listCoins.Count > 0)
            {
                for (int j = 0; j < i; j++)        
                {
                    while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance || Vector3.Distance(boat.transform.position + new Vector3(0, 100, 0), newCoords) < minDistance)
                    {                      
                        newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));
                    }
                }
            }
            
            //Debug.Log(minDistance);
            //Debug.Log("distance to boat " + Vector3.Distance(boat.transform.position + new Vector3(0, 100, 0), newCoords));

            GameObject aux = Instantiate(CoinObject, newCoords, Quaternion.identity);
            auxBuoyInstance = Instantiate(buoy, newCoords - new Vector3(0, buoyHeight, 0), Quaternion.Euler(-90, 0, 0));
            listCoins[i].CoinObject = aux;
        }
    }

    private void GenerateNewObjective(int newID) {
        //List<CoinClass> listCoins = new List<CoinClass>();

        listCoins.Add(FactoryOfCoin(newID)); //add new element to end of the list with CoinID of element that was removed
        Vector3 newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));

        for (int j = 0; j < listCoins.Count - 1; j++) {
            while (Vector3.Distance(listCoins[j].CoinObject.transform.position, newCoords) < minDistance || Vector3.Distance(boat.transform.position + new Vector3(0, 100, 0), newCoords) < minDistance)
            {
                newCoords = new Vector3(UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2), 100f, UnityEngine.Random.Range(-coinGameArea / 2, coinGameArea / 2));
            }
        }
        GameObject aux = Instantiate(CoinObject, newCoords, Quaternion.identity);
        auxBuoyInstance = Instantiate(buoy, newCoords - new Vector3(0, buoyHeight, 0), Quaternion.Euler(-90, 0, 0));
        listCoins[listCoins.Count - 1].CoinObject = aux; //is listCoins.Count-1 the last position of the list? I think so
        ResizeObjetives();
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

    public void getStim()
    {
        //int stim = Assets.LSL4Unity.Scripts.Examples.ReceiveLSLmarkers.markerint;
        int stim = ReceiveLSLmarkers.markerint;
        //Debug.Log("STIM: "+ stim);

        switch (stim)
        {
            case 800:
            case 10: //hide cross
                bciCross.enabled = false;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = false;
                cross = false;
                left = false;
                right = false;
                hidearrow = false;
                break;

            case 33282:
            case 6: //beep
                bciCross.enabled = true;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = false;
                cross = true;
                left = false;
                right = false;
                hidearrow = false;
                break;

            case 786:
            case 5: // show cross
                bciCross.enabled = true;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = false;
                cross = true;
                left = false;
                right = false;
                hidearrow = false;
                break;

            case 770:
            case 8: // right arrow
                bciCross.enabled = true;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = true;
                cross = true;
                left = false;
                right = true;
                hidearrow = false;
                break;

            case 769:
            case 7: // left arrow
                bciCross.enabled = true;
                bciLeftArrow.enabled = true;
                bciRightArrow.enabled = false;
                cross = true;
                left = true;
                right = false;
                hidearrow = false;
                break;

            case 781:
            case 9: // hide arrow
                bciCross.enabled = true;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = false;
                cross = true;
                hidearrow = true;
                //left= false;
                //right= false;
                break;


            case 32770: // END OF SESSION
                timeRemaining = 0;
                break;


            default:
                bciCross.enabled = false;
                bciLeftArrow.enabled = false;
                bciRightArrow.enabled = false;
                cross = false;
                left = false;
                right = false;
                hidearrow = false;
                break;
        }
    }


    //-----------------------------------------------   ASSISTIVE MECHANISMS   -----------------------------------------------
    public bool CheckMaxAngleDev()
    {
        var boatOrientation = boat.transform.forward;
        var coinHeight = new Vector3(0, 100, 0);

        foreach (CoinClass buoy in listCoins)
        {
            var vectorToBuoy =  buoy.CoinObject.transform.position - coinHeight - boat.transform.position;
            var auxAngle = Vector3.Angle(boatOrientation, vectorToBuoy);
            if (auxAngle < maxAngleDev-20) return true;
        }

        //if every buoy is OUT of the player's F.O.V.

        var closestBuoyInfo = FindClosestBuoy();
        var vectorToClosestBuoy = boat.transform.position - closestBuoyInfo[0];
        var directionAux = AngleDir(boatOrientation, vectorToClosestBuoy, Vector3.up); //check if buoy is to the left or to the right

        var rowing = movementScript.cooldownActivated; //<----------------------------------------------------------------------------------------------   IDK

        if (closestBuoyInfo[1].x > maxDistance && !rowing && !auxSelfCorrect) //full correction process -> closestBuoyInfo[1].x is where I save the distance to closest buoy
        {
            Debug.Log("full correction process ->  distance=" + closestBuoyInfo[1].x);
            movementScript.selfCorrection = true;
            selfCorrectTimer.SetActive(true);
            auxSelfCorrect = true;
            stopTimer = true;
        }


        else if (directionAux == 1 || directionAux == 0)
        {
            arrowLeft.SetActive(true);
            arrowRight.SetActive(false);
        }
        else if (directionAux == -1)
        {
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
        }

        return false;
    }

    public Vector3[] FindClosestBuoy()
    {
        Vector3 closest = Vector3.zero;
        float distance = Mathf.Infinity;
        //Vector2 position = new Vector2(transform.position.x, transform.position.z);

        foreach (CoinClass buoy in listCoins)
        {
            var buoyPos = buoy.CoinObject.transform.position - new Vector3(0, 100, 0);
            float curDistance = Vector3.Distance(buoyPos, transform.position); 
            if (curDistance < distance)
            {
                closest = buoyPos;
                distance = curDistance;
            }
        }
        var info = new Vector3[2];
        info[0] = closest;
        info[1] = new Vector3(distance, 0, 0);
        return info;
    }

    public void TurnOffArrows()
    {
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
    }


    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) //check if buoy is to the left or to the right of the player
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    IEnumerator CorrectionCoroutine(float speed, Quaternion rotation)
    {
        Quaternion current = transform.rotation;
        movementScript.enabled = false;

        var closestBuoyInfo = FindClosestBuoy();
        var vectorToClosestBuoy = closestBuoyInfo[0]- boat.transform.position;
        rotation = Quaternion.LookRotation(vectorToClosestBuoy, new Vector3(0, 1, 0));

        transform.localRotation = Quaternion.Lerp(current, rotation, speed * Time.deltaTime);
        yield return null;
    }

    IEnumerator SelfCorrection()
    {
        float currTime = selfCorrectTime;

        while (currTime > 0)
        {
            currTime -= Time.deltaTime;
            correctTimerText.text = ((int)currTime).ToString();

            rb.velocity = Vector3.zero;

            yield return null;
        }
        selfCorrectTimer.SetActive(false);
        //auxSelfCorrect = false;
        auxCorrection = true;
        //yield return null;
    }



    //----------------------------------------------   DIFFICULTY PROGRESSION   ----------------------------------------------
    public void ResizeObjetives()
    {
        
        
        float newRadiusAux = newRadius - coinShrinkStep;
        if (newRadiusAux <= 0)
        {
            coinShrinkStep = coinShrinkStep / 2;
            newRadius -= coinShrinkStep;
        }
        else newRadius = newRadiusAux;

        foreach (CoinClass coin in listCoins)
        {
            coin.CoinObject.transform.localScale = new Vector3(newRadius, 100, newRadius);
        }

        Debug.Log("radius - " + newRadius);
        

    }
}