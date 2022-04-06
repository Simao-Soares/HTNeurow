using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataLogger : MonoBehaviour
{
    public GameObject boat;
    string filename = "";
    public float amsFreq = 1f;
    public TextWriter tw;

    
    [HideInInspector] public float timeAux; //timeStamp
    [HideInInspector] public string eventAux;



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
        timeAux = 0;
        eventAux = "NULL";
    }

    private void Awake()
    {
        filename = Application.dataPath + "/DataFiles/Log-" + System.DateTime.Now.ToString("HH-mm") + ".csv";
        tw = new StreamWriter(filename);
        tw.WriteLine(", RPosX, RPosY, RPosZ, , LPosX, LPosY, LPosZ, , BoatPosX, BoatPosY, , Event"); //for only 1 tracking point per hand
        tw.Close();

    }

    // Update is called once per frame
    void FixedUpdate() {
        elapsed += Time.deltaTime;
        //if (elapsed >= 1 / amsFreq)
        //{
        //    //elapsed = elapsed % 1f;
        //    elapsed = 0f;

        //    //OutputTime();   //uncomment to show print time instances
        //    WriteCSV();
        //}
        timeAux += Time.fixedDeltaTime;
        WriteCSV();
    }
    void OutputTime() {
        Debug.Log(Time.time);
    }


    public void WriteCSV()
    {
        if (myHandPointList.handPoint.Length > 0)
        {
            Vector3 auxPosR;
            Vector3 auxPosL;
            tw = new StreamWriter(filename, true);

            //for (int i = 0; i < myHandPointList.handPoint.Length ; i++) //2 HandPoints per line, one for each hand
            //{
                auxPosR = myHandPointList.handPoint[0].LeftObject.transform.localPosition;
                auxPosL = myHandPointList.handPoint[0].RightObject.transform.localPosition;
                //auxRotR = myHandPointList.handPoint[i].LeftObject.transform.rotation;
                //auxRotL = myHandPointList.handPoint[i].RightObject.transform.rotation;

                tw.WriteLine(timeAux + "," +
                             auxPosR.x + "," + auxPosR.y + "," +
                             auxPosR.z + "," + "," +
                             auxPosL.x + "," + auxPosL.y + "," +
                             auxPosL.z + "," + "," + boat.transform.position.x + "," + boat.transform.position.z + "," + "," + eventAux);
            //}
            tw.Close();
        }
    }
}
