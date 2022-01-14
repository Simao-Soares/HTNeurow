using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPathGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-------------------------  TRASH  --------------------------

    //void DefinePath()
    //{
    //    float totalPathLength = boatSpeed * totalTaskTime;
    //    float pathLength = 0;
    //    int nArcs = 0;
    //    float arcLength = 0;

    //    ArcClass newArc = new ArcClass();
    //    ArcClass newArc1 = new ArcClass();
    //    ArcClass newArc2 = new ArcClass();

    //    newArc.arcID = 0;
    //    newArc.angle = 60;
    //    newArc.radius = 20f;

    //    newArc1.arcID = 1;
    //    newArc1.angle = 40;
    //    newArc1.radius = 30f;

    //    newArc2.arcID = 2;
    //    newArc2.angle = 40;
    //    newArc2.radius = 20;

    //    listArcs.Add(newArc);
    //    listArcs.Add(newArc1);
    //    listArcs.Add(newArc2);





    //    //while (pathLength < totalPathLength)
    //    //for (int i = 0; i < 3; i++)                      //<-------------------------------------------------- testing 3 arcs
    //    //{
    //    //    //Later define range of radius and angle values based on dificulty level
    //    //    float newRadius = Random.Range(20f, 50f);
    //    //    float newAngle = Random.Range(45f, 225f);

    //    //    //Adding new arc to the list
    //    //    ArcClass newArc = new ArcClass();
    //    //    newArc.radius = newRadius;

    //    //    //ArcRenderer takes int arcAngle, +1 so that the path doesn't end sonner than the time
    //    //    newArc.angle = (int)Mathf.Round(newAngle) + 1;
    //    //    newArc.arcID = nArcs;
    //    //    listArcs.Add(newArc);

    //    //    //Calculating arc length and adding it to the total path length
    //    //    arcLength = (newAngle / 360f) * 2 * Mathf.PI * newRadius;
    //    //    pathLength += arcLength;
    //    //    nArcs++;
    //    //}
    //}

    //private void PathRendererFunc()
    //{
    //    int auxInit;
    //    float prevAngle, prevRadius, prevCenterX, prevCenterY, prevYrot;

    //    foreach (ArcClass arc in listArcs)
    //    {
    //        //ar = gameObject.GetComponent<ArcRenderer>();
    //        GameObject rendererObj = Instantiate(renderer);
    //        arc.arcObj = rendererObj;                                 //<-------------------------------------------------- clean, no need for rendererObj
    //        ArcRenderer ar = rendererObj.GetComponent<ArcRenderer>();

    //        ar.arcWidth = pathWidth;
    //        ar.arcAngle = listArcs[arc.arcID].angle;
    //        ar.arcRadius = listArcs[arc.arcID].radius;


    //        float auxX = 0;
    //        float auxY = 0;

    //        auxInit = Random.Range(0, 1);   //defines direction of the first arc 
    //        if (auxInit == 0) ar.xRot = true;
    //        else ar.xRot = false;
    //        if (arc.arcID == 0) //first arc
    //        {

    //            if (auxInit == 0) //turns right 
    //            {
    //                listArcs[arc.arcID].centerX = ar.xPos = ar.arcRadius - pathWidth / 2;
    //                listArcs[arc.arcID].centerY = 0;
    //                listArcs[arc.arcID].yRotation = ar.yRot = -90f;
    //            }
    //            else              //turns left
    //            {
    //                listArcs[arc.arcID].centerX = ar.xPos = -ar.arcRadius + pathWidth / 2;
    //                listArcs[arc.arcID].centerY = 0;
    //                listArcs[arc.arcID].yRotation = ar.yRot = -90f;
    //            }
    //        }

    //        else
    //        {

    //            prevAngle = listArcs[arc.arcID - 1].angle;
    //            prevRadius = listArcs[arc.arcID - 1].radius;
    //            prevCenterX = listArcs[arc.arcID - 1].centerX;
    //            prevCenterY = listArcs[arc.arcID - 1].centerY;
    //            prevYrot = listArcs[arc.arcID - 1].yRotation;

    //            if (arc.arcID == 1)
    //            {
    //                auxX = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Cos((180f - prevAngle) * Mathf.Deg2Rad);
    //                auxY = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Sin((180f - prevAngle) * Mathf.Deg2Rad);
    //            }
    //            else if (arc.arcID == 2)
    //            { // bem martelado simao apaga isto
    //                auxX = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Cos((prevAngle - (listArcs[0].angle - 90)) * Mathf.Deg2Rad);
    //                auxY = (prevRadius + ar.arcRadius - pathWidth) * Mathf.Sin((prevAngle - (listArcs[0].angle - 90)) * Mathf.Deg2Rad);
    //            }


    //            if (arc.arcID % 2 == 1) //odd number arcs
    //            {
    //                listArcs[arc.arcID].centerX = ar.xPos = prevCenterX + auxX;
    //                listArcs[arc.arcID].centerY = ar.zPos = prevCenterY + auxY;
    //                listArcs[arc.arcID].yRotation = ar.yRot = 90f + prevAngle - ar.arcAngle;

    //            }

    //            else if (arc.arcID % 2 == 0) //even number arcs
    //            {

    //                listArcs[arc.arcID].centerX = ar.xPos = prevCenterX + auxY;
    //                listArcs[arc.arcID].centerY = ar.zPos = prevCenterY - auxX;
    //                listArcs[arc.arcID].yRotation = ar.yRot = -(prevAngle - (listArcs[0].angle - 90));

    //            }
    //        }
    //        ar.DrawArcMesh();
    //    }
    //}

    //public GameObject FindClosestArc(Vector3 point)
    //{
    //    GameObject closest = null;
    //    float distance = Mathf.Infinity;
    //    //Vector3 boatPosition = gameObject.transform.position;
    //    foreach (ArcClass arc in listArcs)
    //    {
    //        var centerArc = new Vector3(arc.centerX, 0f, arc.centerY);              //---------------------------------------------------------> CHANGE centerY to centerZ and arcID to id
    //        float curDistance = Vector3.Distance(centerArc, point) - arc.radius;

    //        if (curDistance < distance)
    //        {
    //            closest = arc.arcObj;
    //            distance = curDistance;
    //        }
    //    }
    //    return closest;
    //}

    //------------------------------------------------------------

}
