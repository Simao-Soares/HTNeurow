using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PathGame : MonoBehaviour
{

    //STILL NEED TO FIGURE OUT:
    public float boatSpeed;         //update boat movement method so this can be its actual speed
    public float challengeLevel;    //determine how to measure challenge level based on radius and angle of arcs (lower radius => tighter curves, lower angles => more curves)

    //PRIMARY TASK SETTINGS
    public float totalTaskTime;

    //SECUNDARY TASK SETTINGS
    public float instructionsTime;

    //UI and text
    public GameObject instructions;
    public GameObject normalUI;
    public GameObject gameOverUI;
    public TMP_Text timeText;
    public TMP_Text playerScoreText;
    public TMP_Text finalScoreText;

    //Screen Freeze Couroutine
    public float freezeDuration = 10f;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;

    //PRIVATE VARIABLES
    private int playerScore = 0;
    private bool timerIsRunning = false;
    private bool auxI;


    public class ArcClass
    {
        //Arc Properties
        public int arcID;
        public float radius;
        public int angle;

        //Arc Position
        public float centerX;
        public float centerY;
        public float yRotation;

    }
    private List<ArcClass> listArcs = new List<ArcClass>();

    public GameObject boat;



    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (totalTaskTime > 0)
            {
                totalTaskTime -= Time.deltaTime;
                DisplayTime(totalTaskTime);

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
                this.finalScoreText.text = playerScore.ToString();

                gameOverUI.SetActive(true);

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

    void DefinePath()
    {
        float totalPathLength = boatSpeed * totalTaskTime;
        float pathLength = 0;
        int nArcs = 0;
        float arcLength = 0;

        //while (pathLength < totalPathLength)
        for(int i=0; i<2; i++)                      //<-------------------------------------------------- testing 3 arcs
        { 
            //Later define range of radius and angle values based on dificulty level
            float newRadius = Random.Range(20f, 50f);
            float newAngle = Random.Range(45f, 225f);

            //Adding new arc to the list
            ArcClass newArc = new ArcClass();
            newArc.radius = newRadius;

            //ArcRenderer takes int arcAngle, +1 so that the path doesn't end sonner than the time
            newArc.angle = (int)Mathf.Round(newAngle)+1; 
            newArc.arcID = nArcs;
            listArcs.Add(newArc);

            //Calculating arc length and adding it to the total path length
            arcLength = (newAngle / 360f) * 2 * Mathf.PI * newRadius;
            pathLength += arcLength;
            nArcs++;
        }
    }

    private void PathRenderer()  // <--------------------------------------------------------------------------------------------------------- simplifica e testa por partes, E ANTES DISSO ACERTA COM 3 ArcMesh a mexer no inspector
    {

        ArcRenderer ar;
        int auxInit;
        float prevAngle, prevRadius, prevCenterX, prevCenterY, prevYrot;

        foreach (ArcClass arc in listArcs)
        {
            ar = gameObject.GetComponent<ArcRenderer>();
            ar.arcAngle = listArcs[arc.arcID].angle;
            ar.arcRadius = listArcs[arc.arcID].radius;

            float auxX = 0;
            float auxY = 0;

            auxInit = Random.Range(0, 1);   //defines direction of the first arc 
            ar.DrawArcMesh();

            if (arc.arcID == 0) //first arc
            {
                
                if (auxInit == 0) //turns right 
                {
                    listArcs[arc.arcID].centerX = ar.xPos = ar.arcRadius - ar.arcWidth / 2;
                    listArcs[arc.arcID].centerY = 0;
                    listArcs[arc.arcID].yRotation = ar.yRot = 0;
                } 
                else              //turns left
                {
                    listArcs[arc.arcID].centerX = ar.xPos = -ar.arcRadius + ar.arcWidth / 2;
                    listArcs[arc.arcID].centerY = 0;
                    listArcs[arc.arcID].yRotation = ar.yRot = 180f - ar.arcAngle;
                } 
            }

            else {
                prevAngle = listArcs[arc.arcID -1].angle;
                prevRadius = listArcs[arc.arcID -1].radius;
                prevCenterX = listArcs[arc.arcID - 1].centerX;
                prevCenterY = listArcs[arc.arcID - 1].centerY;
                prevYrot = listArcs[arc.arcID - 1].yRotation;

                auxX = (prevRadius + ar.arcRadius) * Mathf.Sin(ar.arcAngle);
                auxY = (prevRadius + ar.arcRadius) * Mathf.Cos(ar.arcAngle);

                if (arc.arcID % 2 == 1) //odd number arcs
                {
                    if (auxInit == 0) //first arc went to the right
                    {
                        if (listArcs[arc.arcID].angle < 90f)
                        {
                            listArcs[arc.arcID].centerX = ar.xPos = prevCenterX - auxX;
                            listArcs[arc.arcID].centerY = ar.zPos = prevCenterX + auxY;
                            listArcs[arc.arcID].yRotation = ar.yRot = prevYrot;






                        }


                    }

                }
                else if (arc.arcID % 2 == 0) //even number arcs
                {
                    if (auxInit == 0) ar.xPos = ar.arcRadius - ar.arcWidth / 2;
                    else ar.xPos = ar.arcRadius + ar.arcWidth / 2;

                }
            }

            //define position (xPos and zPos) based on previous arc             <---------------------------------------------------------------------------
            //xRot swaps between 0 and 180 and with 180 i think you need to substract the diameter in the localXPos idk
        }
            

    }










}
