using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PathGame : MonoBehaviour
{

    //STILL NEED TO FIGURE OUT:
    private float boatSpeed;         //update boat movement method so this can be its actual speed
    public float challengeLevel;    //determine how to measure challenge level based on radius and angle of arcs (lower radius => tighter curves, lower angles => more curves)

    //PRIMARY TASK SETTINGS
    public float totalTaskTime;
    public float pathWidth;

    public GameObject renderer;

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

    //Screen Freeze Couroutine
    public float freezeDuration = 10f;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;

    //PRIVATE VARIABLES
    private float playerScore = 0;
    private bool timerIsRunning = false;
    private bool auxI;
    private float distanceToPath;


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

    //public GameObject boat;



    // Start is called before the first frame update
    void Start()
    {
        DefinePath();
        PathRenderer();

        timerIsRunning = true;
        instructions.SetActive(false);
        //tempScoreUI.SetActive(false);
        auxI = true;
        timerIsRunning = true;
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

                //------------------------------------------------------------

                FindDistanceToPath();
                UpdateScore(distanceToPath);
                this.playerScoreText.text = ((int)playerScore).ToString();
                this.distanceText.text = ((int)distanceToPath).ToString();

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

        //ArcClass newArc = new ArcClass();
        //ArcClass newArc1 = new ArcClass();
        //ArcClass newArc2 = new ArcClass();

        //newArc.arcID = 0;
        //newArc.angle = 125;
        //newArc.radius = 20f;

        //newArc1.arcID = 1;
        //newArc1.angle = 55;
        //newArc1.radius = 30f;

        //newArc2.arcID = 2;
        //newArc2.angle = 40;
        //newArc2.radius = 50f;
        //listArcs.Add(newArc);
        //listArcs.Add(newArc1);
        //listArcs.Add(newArc2);





        //while (pathLength < totalPathLength)
        for (int i = 0; i < 3; i++)                      //<-------------------------------------------------- testing 3 arcs
        {
            //Later define range of radius and angle values based on dificulty level
            float newRadius = Random.Range(20f, 50f);
            float newAngle = Random.Range(45f, 225f);

            //Adding new arc to the list
            ArcClass newArc = new ArcClass();
            newArc.radius = newRadius;

            //ArcRenderer takes int arcAngle, +1 so that the path doesn't end sonner than the time
            newArc.angle = (int)Mathf.Round(newAngle) + 1;
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


        int auxInit;
        float prevAngle, prevRadius, prevCenterX, prevCenterY, prevYrot;

        foreach (ArcClass arc in listArcs)
        {
            //ar = gameObject.GetComponent<ArcRenderer>();
            GameObject rendererObj = Instantiate(renderer);
            ArcRenderer ar = rendererObj.GetComponent<ArcRenderer>();

            ar.arcWidth = pathWidth;
            ar.arcAngle = listArcs[arc.arcID].angle;
            ar.arcRadius = listArcs[arc.arcID].radius;


            float auxX = 0;
            float auxY = 0;

            auxInit = Random.Range(0, 1);   //defines direction of the first arc 
            if (auxInit == 0) ar.xRot = true;
            else ar.xRot = false;
            if (arc.arcID == 0) //first arc
            {

                if (auxInit == 0) //turns right 
                {
                    listArcs[arc.arcID].centerX = ar.xPos = ar.arcRadius - pathWidth / 2;
                    listArcs[arc.arcID].centerY = 0;
                    listArcs[arc.arcID].yRotation = ar.yRot = -90f;
                }
                else              //turns left
                {
                    listArcs[arc.arcID].centerX = ar.xPos = -ar.arcRadius + pathWidth / 2;
                    listArcs[arc.arcID].centerY = 0;
                    listArcs[arc.arcID].yRotation = ar.yRot = -90f;
                }
            }

            else
            {

                prevAngle = listArcs[arc.arcID - 1].angle;
                prevRadius = listArcs[arc.arcID - 1].radius;
                prevCenterX = listArcs[arc.arcID - 1].centerX;
                prevCenterY = listArcs[arc.arcID - 1].centerY;
                prevYrot = listArcs[arc.arcID - 1].yRotation;

                if (arc.arcID == 1)
                {
                    auxX = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Cos((180f - prevAngle) * Mathf.Deg2Rad);
                    auxY = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Sin((180f - prevAngle) * Mathf.Deg2Rad);
                }
                else if (arc.arcID == 2)
                { // bem martelado simao apaga isto
                    auxX = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Cos((prevAngle - (listArcs[0].angle - 90)) * Mathf.Deg2Rad);
                    auxY = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Sin((prevAngle - (listArcs[0].angle - 90)) * Mathf.Deg2Rad);
                }


                if (arc.arcID % 2 == 1) //odd number arcs
                {
                    listArcs[arc.arcID].centerX = ar.xPos = prevCenterX + auxX;
                    listArcs[arc.arcID].centerY = ar.zPos = prevCenterY + auxY;
                    listArcs[arc.arcID].yRotation = ar.yRot = 90f + prevAngle - ar.arcAngle;

                }

                else if (arc.arcID % 2 == 0) //even number arcs
                {

                    listArcs[arc.arcID].centerX = ar.xPos = prevCenterX + auxY;
                    listArcs[arc.arcID].centerY = ar.zPos = prevCenterY - auxX;
                    listArcs[arc.arcID].yRotation = ar.yRot = -(prevAngle - (listArcs[0].angle - 90));

                }


            }

            ar.DrawArcMesh();
        }
    }


    public GameObject FindClosestArc()
    {
        GameObject[] arcs;
        arcs = GameObject.FindGameObjectsWithTag("PathArcs");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 boatPosition = gameObject.transform.position;
        foreach (GameObject arc in arcs)
        {
            Vector3 diff = arc.transform.position - boatPosition;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = arc;
                distance = curDistance;
            }
        }
        return closest;
    }

    public void FindDistanceToPath()
    {
        var closestArc = FindClosestArc();
        var collider = closestArc.GetComponent<MeshCollider>();
        var boatPosition = gameObject.transform.position;

        Vector3 closestPoint = collider.ClosestPoint(boatPosition);
        Debug.Log(closestPoint);
        distanceToPath = Vector3.Distance(boatPosition, closestPoint);
    }

    public void UpdateScore(float distance)  //Maybe change multipliers
    {
        if (distance > 1f) playerScore += 1 / (10*distance);
        else playerScore+=0.2f;
    }

}
