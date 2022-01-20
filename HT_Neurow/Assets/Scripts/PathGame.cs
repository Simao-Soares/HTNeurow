using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PathGame : MonoBehaviour
{

        


    //PRIMARY ELEMENTS
    public GameObject meshHolder;
    public GameObject pathRenderer;
    public GameObject pathCreator;

    public Material pathMaterial;
   

    [HideInInspector] public List<Vector2> anchorPoints;    //(-x, z)
    [HideInInspector] public List<Vector2> vectorPoints;    //(z, -x)

    [HideInInspector] public float boatSpeed;

    //PRIMARY TASK SETTINGS
    [Range(1, 5)]
    public int challengeLevel;
    public float totalTaskTime = 60f;
    public float scoreMultiplier = 0.1f;


    public float selfCorrectTime = 10f; 


    //Test these ranges:
    [Range(120, 170)]           
    public float maxDeviationAngle = 150f;
    [Range(5, 10)]
    public float maxDistance = 5f;
    [Range(10, 20)]
    public float maxDistanceNoAngle = 10f;
    //[Range(10, 20)]
    public float correctionSpeed = 0.5f;

    [HideInInspector] public bool auxCorrection = false;
    [HideInInspector] public bool auxSelfCorrect = false;

    public bool stopTimer;

    //SECUNDARY TASK SETTINGS
    public float instructionsTime;

    //UI and text
    public GameObject instructions;
    public GameObject normalUI;
    public GameObject gameOverUI;
    public TMP_Text timeText;
    public TMP_Text playerScoreText;
    public TMP_Text finalScoreText;

    public TMP_Text distanceText;

    public TMP_Text correctTimerText;
    public GameObject selfCorrectTimer;

    //Screen Freeze Couroutine
    public float freezeDuration = 10f;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;

    //PRIVATE VARIABLES
    private float playerScore = 0;
    private bool timerIsRunning = false;
    private bool auxI;

    private Vector3 closestPointBoat;



    private Rigidbody rb;

    
    private Quaternion correctionRotation;

    BoatMovement movementScript;


    //-----------------//-----------------//---------------  DEBUG  -----------------//-----------------//-----------------
    public GameObject ball;
    public GameObject ball2;
    public GameObject ball3;
    //-----------------//-----------------//-----------------------------------------//-----------------//-----------------


    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        stopTimer = false;

        instructions.SetActive(false);
        selfCorrectTimer.SetActive(false);
        auxI = true;

        rb = GetComponent<Rigidbody>();
        boatSpeed = rb.velocity.magnitude;
        movementScript = GetComponent<BoatMovement>();

        anchorPoints = pathCreator.GetComponent<MyPathGenerator>().anchorPoints;
        vectorPoints = pathCreator.GetComponent<MyPathGenerator>().colliderPoints;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (totalTaskTime > 0)
            {
                if (!stopTimer) totalTaskTime -= Time.deltaTime;
                DisplayTime(totalTaskTime);

                //------------------------------------------------------------

                closestPointBoat = FindClosestVectorPoint();
                
                ball.transform.position = closestPointBoat;


                //-----------------//-----------------//---------------  DEBUG  -----------------//-----------------//-----------------

                var AUXfrontAnchor = FindClosestFrontAnchor();
                var AUXfrontAnchorNorm = new Vector3(-AUXfrontAnchor.y, 0, AUXfrontAnchor.x);
                ball2.transform.position = AUXfrontAnchorNorm;

                //-----------------//-----------------//----------------------//-----------------//-----------------//-----------------


                UpdateScore(closestPointBoat);

                playerScoreText.text = ((int)playerScore).ToString();

                BackOnTrack();
                if (auxSelfCorrect) StartCoroutine("SelfCorrection");
                if (auxCorrection) {
                    StopCoroutine("SelfCorrection");
                    StartCoroutine(CorrectionCoroutine(correctionSpeed, correctionRotation));
                } 


                //------------------------------------------------------------

                //always show instructions for instructionsTime seconds at the start
                if (auxI == true) StartCoroutine(ShowInstructions(instructionsTime));
                //show instructions for instructionsTime seconds when i key is pressed
                if (Input.GetKeyDown("i")) StartCoroutine(ShowInstructions(instructionsTime)); //not perfect but enough
            }
            else //TIME IS UP
            {
                Debug.Log("Time has run out!");
                totalTaskTime = 0;
                timerIsRunning = false;
                normalUI.SetActive(false);
                finalScoreText.text = ((int)playerScore).ToString();

                gameOverUI.SetActive(true);

                pendingFreezeDuration = freezeDuration; //freeze screen on GameOver
                if (pendingFreezeDuration > 0 && !isFrozen) StartCoroutine(DoFreeze());


            }
        }

    }


    //------------------------  TIMER FUNCTIONS  -----------------      ---------------------------------------> should probably be moved to a different file, since they're common to both tasks

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator ShowInstructions(float time)
    {
        instructions.SetActive(true);
        yield return new WaitForSeconds(time);
        auxI = false;
        instructions.SetActive(false);
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

    //------------------------------------------------------------


    //To the collider
    //public Vector3 FindClosestPoint(Vector3 point)
    //{
    //    var collider = edgeCollider.GetComponent<EdgeCollider2D>();
    //    Vector3 closestPoint = collider.ClosestPoint(point);
    //    return closestPoint;                                                                                                                       
    //}

    public void UpdateScore(Vector3 closestPoint)  //Maybe change multipliers
    {
        var distance = Vector3.Distance(transform.position, closestPoint);
        this.distanceText.text = ((int)distance).ToString();
        if(distance <= maxDistance)
        {
            if (distance > 1f) playerScore += scoreMultiplier / (2 * distance);
            else playerScore += scoreMultiplier;
        }
    }


    public Vector3 FindClosestFrontAnchor() 
    {
        Vector2 closest = Vector2.zero;
        float distance = Mathf.Infinity;
        Vector2 position = new Vector2(transform.position.x, transform.position.z); 
        foreach (Vector2 anchor in anchorPoints)
        {
            //Debug.Log(anchor);
            if (position.y < anchor.x)
            {                                                                          
                float curDistance = Vector2.Distance(new Vector2(-anchor.y, anchor.x), position);
                if (curDistance < distance)
                {
                    closest = anchor;
                    distance = curDistance;
                }

            }
        }
        return closest;
    }


    public Vector3 FindClosestVectorPoint()
    {
        Vector2 closest = Vector2.zero;
        float distance = Mathf.Infinity;
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        foreach (Vector2 point in vectorPoints)
        {
            //Debug.Log(point);
            float curDistance = Vector2.Distance(new Vector2(-point.x, point.y), position); //making elements of collideList (x,z)
            if (curDistance < distance)
            {
                closest = point;
                distance = curDistance;
            }
        }
        return new Vector3(-closest.x, 0, closest.y); // turning (-x,z) into (x,0,z)
    }




    public void BackOnTrack()              
    {
        Vector3 boatPos, frontOfBoat, toClosestVectorPoint, vectorToAnchor;
        var rowing = movementScript.cooldownActivated; //turning

        boatPos = transform.position;
        frontOfBoat = boatPos + 5*transform.forward;
        var boatOrientation = frontOfBoat - boatPos;

        toClosestVectorPoint = closestPointBoat + new Vector3(0, 0.5f, 0) - boatPos; //when the path is straight the vector points can be the same as the anchor points

        var frontAnchor = FindClosestFrontAnchor();
        var frontAnchorNorm = new Vector3(-frontAnchor.y, 0, frontAnchor.x);
        vectorToAnchor = frontAnchorNorm + new Vector3(0, 0.5f, 0) - boatPos;

        float angleTan = Vector3.Angle(boatOrientation, toClosestVectorPoint);
        float angleAnchor = Vector3.Angle(boatOrientation, vectorToAnchor);
        float distanceBoatToPath = Vector3.Distance(boatPos, closestPointBoat);

        if (vectorToAnchor != Vector3.zero) correctionRotation = Quaternion.LookRotation(vectorToAnchor, new Vector3(0, 1, 0));

        PathColor(angleTan, distanceBoatToPath);
                       
        if (!rowing && !auxSelfCorrect) {  
            if (angleTan > maxDeviationAngle && distanceBoatToPath > maxDistance || distanceBoatToPath > maxDistanceNoAngle)    //maxDeviation verified with angle deviation in relation to tan to path
            {
                //movementScript.enabled = false;
                movementScript.selfCorrection = true; 

                selfCorrectTimer.SetActive(true);
                auxSelfCorrect = true;
                stopTimer = true;
            }
        }
        
        if (angleAnchor < 5f)
        {
            auxCorrection = false;
            auxSelfCorrect = false;
            movementScript.enabled = true;

            stopTimer = false;
            selfCorrectTimer.SetActive(false);
            StopCoroutine("SelfCorrection");
            movementScript.selfCorrection = false;
        }
        
    }

    IEnumerator CorrectionCoroutine(float speed, Quaternion rotation)
    {
        Quaternion current = transform.rotation;
        movementScript.enabled = false;
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

    void PathColor(float angle, float distance)
    {
        //How to consider maxDistanceNoAngle??
        Color color;
        float red = 0f;
        float green = 0f;
        float blue = 0f;

        var angleComponent = (angle-90f) / maxDeviationAngle;
        var distanceComponent = 0f;
        if (distance >= 1f) distanceComponent = (distance-1) / maxDistance;

        //var deviation = angleComponent/2 + distanceComponent/2;
        var deviation =  distanceComponent;
        Debug.Log(deviation);
        if (deviation <= 0.5)
        {
            green = 1f;
            red = 2 * deviation;
        }
        else if(deviation > 0.5)
        {
            green = -2 * deviation + 2;
            red = 1f;
        }
        color = new Color(red, green, blue);
        pathMaterial.color = color;
        playerScoreText.color = color;
        //uiCue.GetComponent<Material>().color = color;
    }
}
