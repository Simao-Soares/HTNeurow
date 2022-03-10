using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataLogger : MonoBehaviour
{
    string filename = "";
    public float amsFreq = 1f;



    //Definition of HandPoint class
    // [System.Serializable]
    // public class HandPoint
    // {
    //     public string idL;
    //     public string idR;
    //     public Vector3 posL;
    //     public Vector3 posR;
    //     public Vector3 rotL;
    //     public Vector3 rotR;
    //     public float speedL;
    //     public float speedR;
    // }
    ///////////////////////////////

    [System.Serializable]
    public class HandPoint
    {
        public string RightID;
        public GameObject RightObject;

        public string LeftID;
        public GameObject LeftObject;

		//private TextWriter tw;


        
    }


    //Array of HandPoints
    [System.Serializable]
    public class HandPointList
    {
        public HandPoint[] handPoint;
        //public GameObject handPoint1;

    }

   
    public HandPointList myHandPointList = new HandPointList();  //List of handPoints
    float elapsed = 0f; //elapsed time

    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/dataLog.csv";
		TextWriter tw = new StreamWriter(filename, false);
		tw.WriteLine("RightPointID, RPosX, RPosY, RPosZ, RRotX, RRotY, RRotZ, , LeftPointID, LPosX, LPosY, LPosZ, LRotX, LRotY, LRotZ");


    }

    // Update is called once per frame
    void FixedUpdate() {
        elapsed += Time.deltaTime;
        if (elapsed >= amsFreq) {
            //elapsed = elapsed % 1f;
            elapsed = 0f;

            //OutputTime();   //uncomment to show print time instances
            WriteCSV();
        }
    }
    void OutputTime() {
        Debug.Log(Time.time);
    }


    public void WriteCSV()
    {
        //there is data to write
        if(myHandPointList.handPoint.Length > 0)
        {
			TextWriter tw = new StreamWriter(filename, true);
            
            Vector3 auxPosR;
            Vector3 auxPosL;
            Quaternion auxRotR;
            Quaternion auxRotL;

            for (int i = 0; i < myHandPointList.handPoint.Length ; i++) //2 HandPoints per line, one for each hand
            {
                auxPosR = myHandPointList.handPoint[i].LeftObject.transform.position;
                auxPosL = myHandPointList.handPoint[i].RightObject.transform.position;

                auxRotR = myHandPointList.handPoint[i].LeftObject.transform.rotation;
                auxRotL = myHandPointList.handPoint[i].RightObject.transform.rotation;

                tw.WriteLine(myHandPointList.handPoint[i].RightID + "," +
                             auxPosR.x + "," + auxPosR.y + "," +
                             auxPosR.z + "," + auxRotR.x + "," +
                             auxRotR.y + "," + auxRotR.z + "," + "," +
                             myHandPointList.handPoint[i].LeftID + "," +
                             auxPosL.x + "," + auxPosL.y + "," +
                             auxPosL.z + "," + auxRotL.x + "," +
                             auxRotL.y + "," + auxRotL.z);
            }
            tw.Close();
        }
    }
}
