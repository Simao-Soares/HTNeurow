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
        PathGame gameScript = player.GetComponent<PathGame>();
        //var roadRenderer = this.GetComponent<RoadMeshCreator>();
         
        challengeLevel = gameScript.challengeLevel;
        boatSpeed = gameScript.boatSpeed;
        totalTaskTime = gameScript.totalTaskTime;

        Debug.Log(pathLength);

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

        Debug.Log(pathLength);
        while (currentPathLength < pathLength)
        {
            var count = anchorPoints.Count; 
            var yVar = Random.Range (-3 * challengeLevel, 3 * challengeLevel);
            var xVar = Random.Range(20 * 1 / challengeLevel, 40 * 1 / challengeLevel);
            if (count == 0)
            {
                anchorPoints.Add(Vector2.zero);
            }
            //so that path always starts straightish
            else if (count == 1)                        
            {
                anchorPoints.Add(new Vector2(20f, 0f));
            }

            else
            {
                anchorPoints.Add(new Vector2(anchorPoints[count - 1].x + xVar, anchorPoints[count - 1].y + yVar));
                currentPathLength += Vector2.Distance(anchorPoints[count - 1], anchorPoints[count]);
            }
        }

         


        

}



}
