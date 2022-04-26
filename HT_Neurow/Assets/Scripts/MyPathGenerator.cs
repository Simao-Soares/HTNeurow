using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class MyPathGenerator : MonoBehaviour
{
    public GameObject player;
    public PathCreator pathCreator;
    public GameObject RoadRenderer;

    private VertexPath newVertexPath;
    private BezierPath newBezierPath;
    private int challengeLevel;
    private float boatSpeed, totalTaskTime, pathLength;
    
    [HideInInspector] public List<Vector2> anchorPoints = new List<Vector2>();
    [HideInInspector] public List<Vector2> colliderPoints = new List<Vector2>();


    [HideInInspector]
    public List<Vector2> fixedAnchorsList;



    // Start is called before the first frame update
    void Start()
    {

        FillTrainingAnchors();
        challengeLevel = GameManager.challengeLevel;

        boatSpeed = GameManager.boatSpeed;
        totalTaskTime = GameManager.taskDuration;

        

        pathLength = boatSpeed * totalTaskTime;

        // IN TRAINING 
        if (GameManager.training)
        {
            fixedAnchorsList.Insert(0, Vector2.zero);
            newVertexPath = GeneratePath(fixedAnchorsList.ToArray(), false);
            

        }
        else
        {
            GenerateAnchorPoints(challengeLevel);
            newVertexPath = GeneratePath(anchorPoints.ToArray(), false);
        }





        foreach (Vector3 point in newVertexPath.localPoints)
        {
            Vector2 pointNorm = new Vector2(point.z, point.x);
            colliderPoints.Add(pointNorm);
            //Debug.Log(pointNorm);
        }


        pathCreator.bezierPath = newBezierPath;

        

       


        //-----------------//-----------------//-----------------------------------------//-----------------//-----------------
        //Debug.Log(colliderPoints[1]);
        //Debug.Log(anchorPoints[2]);



        //-----------------//-----------------//-----------------------------------------//-----------------//-----------------
    }

   

    VertexPath GeneratePath(Vector2[] points, bool closedPath)
    {
        // Create a closed, 2D bezier path from the supplied points array
        // These points are treated as anchors, which the path will pass through
        // The control points for the path will be generated automatically

        newBezierPath = new BezierPath(points, closedPath, PathSpace.xz);

        // Then create a vertex path from the bezier path, to be used for movement etc

        return new VertexPath(newBezierPath, transform);  
    }

    void GenerateAnchorPoints(float challengeLevel)
    {
        float currentPathLength = 0;

        //Debug.Log(pathLength);
        while (currentPathLength < pathLength)
        {
            var count = anchorPoints.Count;

            var yAuxVar = Random.Range(0, 2);
            float yVar;
            float xVar;



            float y_minFunction = 4.78228f * Mathf.Exp(0.278084f * challengeLevel)-4.28458f;
            float y_maxFunction = 5.22976f * Mathf.Exp(0.290293f * challengeLevel)-2.17223f;



            if (yAuxVar == 0)
            {
                //if(challengeLevel==1) yVar = Random.Range(-3, -1);  
                //else yVar = Random.Range(-3 * challengeLevel, -minFunctionAux);
                yVar = Random.Range(-y_maxFunction, -y_minFunction);
            }
            else
            {
                //if (challengeLevel == 1) yVar = Random.Range(1, 3); 
                //else yVar = Random.Range(minFunctionAux, 3 * challengeLevel);
                yVar = Random.Range(y_minFunction, y_maxFunction);

            }

            //if (challengeLevel == 1) xVar = Random.Range(5 * 1 / challengeLevel, 10 * 1 / challengeLevel); //<--------------------------------- ugly exception to fix distance bug
            //else
            xVar = Random.Range(15 * 1 / challengeLevel, 30 * 1 / challengeLevel);

            //xVar = Random.Range(x_minFunction, x_maxFunction);


            if (count == 0)
            {
                anchorPoints.Add(Vector2.zero);
            }

            //so that path always starts straight(ish)
            else if (count == 1)                        
            {
                anchorPoints.Add(new Vector2(10f, 0f));
            }

            else
            {
                anchorPoints.Add(new Vector2(anchorPoints[count - 1].x + xVar, anchorPoints[count - 1].y + yVar));
                currentPathLength += Vector2.Distance(anchorPoints[count - 1], anchorPoints[count]);
            }
        }
        Debug.Log(anchorPoints.Count);
    }


    void FillTrainingAnchors()
    {
        if (fixedAnchorsList.Count != 0)
        {
            fixedAnchorsList.Clear();
            fixedAnchorsList.TrimExcess();
        }

        fixedAnchorsList = new List<Vector2>()
        {
            new Vector2(10f, 0f),
            new Vector2(20f, 2f),
            new Vector2(30f, -2f),
            new Vector2(40f, 4f),
            new Vector2(50f, -4f),
            new Vector2(60f, 8f),
            new Vector2(70f, -8f),
            new Vector2(75f, 0f),
            new Vector2(80f, 8f),
            new Vector2(100f, -20f),

            new Vector2(105f, -5f),
            new Vector2(120f, 30f),
            new Vector2(130f, -10f),
            new Vector2(135f, 20f),
            new Vector2(140f, 35f),
            new Vector2(150f, 0f),
            new Vector2(155f, 20f),
            new Vector2(170f, 10f),
            new Vector2(180f, -5f),
            new Vector2(185f, 15f),

            new Vector2(190f, 0f),
            new Vector2(210f, 40f),
            new Vector2(230f, -10f),
            new Vector2(245f, 10f),
            new Vector2(255f, -10f),
            new Vector2(265f, 0f),
            new Vector2(275f, 25f),
            new Vector2(285f, 5f),
            new Vector2(300f, 40f),
            new Vector2(320f, 0f),

            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),

            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
            //new Vector2(15f, -5f),
        };
    }
    
}
