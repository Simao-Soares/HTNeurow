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
        public int arcID;
        public float radius;
        public int angle;
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

        while (pathLength < totalPathLength)
        { 
            //Later define range of radius and angle values based on dificulty level
            float newRadius = Random.Range(20f, 50f);
            float newAngle = Random.Range(45f, 225f);

            //Adding new arc to the list
            ArcClass newArc = new ArcClass();
            newArc.radius = newRadius;
            newArc.angle = (int)Mathf.Round(newAngle)+1; //ArcRenderer takes int arcAngle, +1 so that the path doesn't end sonner than the time
            newArc.arcID = nArcs;
            listArcs.Add(newArc);

            //Calculating arc length and adding it to the total path length
            arcLength = (newAngle / 360f) * 2 * Mathf.PI * newRadius;
            pathLength += arcLength;
            nArcs++;
        }
    }

    private void PathRenderer()
    {

        ArcRenderer ar;
        int auxInit;

        foreach (ArcClass arc in listArcs)
        {
            ar = gameObject.GetComponent<ArcRenderer>();
            ar.arcAngle = listArcs[arc.arcID].angle;
            ar.arcRadius = listArcs[arc.arcID].radius;
            ar.DrawArcMesh();
                    
            if(arc.arcID == 0)
            {
                auxInit = Random.Range(0, 1);
                if(auxInit == 0) ar.xPos = boat.transform.position.x + ar.arcRadius - ar.arcWidth/2;
                else ar.xPos = boat.transform.position.x + ar.arcRadius + ar.arcWidth / 2;
            }

            //define position (xPos and zPos) based on previous arc             <---------------------------------------------------------------------------
            //xRot swaps between 0 and 180 and with 180 i think you need to substract the diameter in the localXPos idk
        }
            

    }










}
