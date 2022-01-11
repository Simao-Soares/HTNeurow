using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class MyPathGenerator : MonoBehaviour
{

    public PathCreator pathCreator;
    private Vector2[] anchorPoints;


    // Start is called before the first frame update
    void Start()
    {
        
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
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xy);
        // Then create a vertex path from the bezier path, to be used for movement etc

        return new VertexPath(bezierPath, transform);  //----------------------> transform probably wrong because it should incorporate vertex path options
    }

    void GenerateAnchorPoints(float challengeLevel)
    {
        switch (challengeLevel)
        {
            case 1:

                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;


        }

}



}
