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




    // Start is called before the first frame update
    void Start()
    {

         
        challengeLevel = GameManager.challengeLevel;

        boatSpeed = GameManager.boatSpeed;
        totalTaskTime = GameManager.taskDuration;

        

        pathLength = boatSpeed * totalTaskTime;

        GenerateAnchorPoints(challengeLevel);
        newVertexPath = GeneratePath(anchorPoints.ToArray(), false);

        //RoadRenderer.GetComponent<RoadMeshCreator>()



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

         


        

}



}
