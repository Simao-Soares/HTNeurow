using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class MyPathGenerator : MonoBehaviour
{
    public GameObject player;
    public PathCreator pathCreator;
    private BezierPath newBezierPath;
    private int challengeLevel;
    private float boatSpeed, totalTaskTime, pathLength;
    
    [HideInInspector] public List<Vector2> anchorPoints = new List<Vector2>();



    // Start is called before the first frame update
    void Start()
    {
        PathGame gameScript = player.GetComponent<PathGame>();
        challengeLevel = gameScript.challengeLevel;
        boatSpeed = gameScript.boatSpeed;
        totalTaskTime = gameScript.totalTaskTime;
        pathLength = boatSpeed * totalTaskTime;

        GenerateAnchorPoints(challengeLevel);
        GeneratePath(anchorPoints.ToArray(), false);  

        pathCreator.bezierPath = newBezierPath;
       

        Debug.Log(anchorPoints[0]);
        Debug.Log(anchorPoints[1]);
        Debug.Log(anchorPoints[2]);
        Debug.Log(anchorPoints[3]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    VertexPath GeneratePath(Vector2[] points, bool closedPath)
    {
        // Create a closed, 2D bezier path from the supplied points array
        // These points are treated as anchors, which the path will pass through
        // The control points for the path will be generated automatically

        newBezierPath = new BezierPath(points, closedPath, PathSpace.xz);

        // Then create a vertex path from the bezier path, to be used for movement etc

        return new VertexPath(newBezierPath, transform);  //----------------------> transform probably wrong because it should incorporate vertex path options
    }

    void GenerateAnchorPoints(float challengeLevel)
    {
        float currentPathLength = 0;


        while (currentPathLength < pathLength)
        {
            var count = anchorPoints.Count; 
            var yVar = Random.Range (-2 * challengeLevel, 2 * challengeLevel);
            var xVar = Random.Range(10 * 1 / challengeLevel, 20 * 1 / challengeLevel);
            if (count == 0)
            {
                anchorPoints.Add(new Vector2(xVar, yVar));
                currentPathLength = Vector2.Distance(Vector2.zero, anchorPoints[0]);
            }

            else
            {
                anchorPoints.Add(new Vector2(anchorPoints[count - 1].x + xVar, anchorPoints[count - 1].y + yVar));
                currentPathLength += Vector2.Distance(anchorPoints[count - 1], anchorPoints[count]);
            }
        }

         


        

}



}
