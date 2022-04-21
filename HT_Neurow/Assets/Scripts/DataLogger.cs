using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataLogger : MonoBehaviour
{
    public GameObject boat;
    string filename = "";
    public int amsFreq = 3;    //----> 90fps is divided by this
    public TextWriter tw;

    
    [HideInInspector] public float timeAux; //timeStamp
    [HideInInspector] public float freqAux; 

    [HideInInspector] public static string RowR; //startRow, rowing or endRow (RIGHT paddle)
    [HideInInspector] public static string RowL; //startRow, rowing or endRow (LEFT paddle)

    [HideInInspector] public static string taskName; //Task1 or Task2
    [HideInInspector] public static string taskInfo;
    [HideInInspector] public static string assistInfo; //NULL, SELF, AUTO
    [HideInInspector] public static float difficultyInfo; //Task1-> 1/width    Task2-> objective radius;



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
    }

    //Array of HandPoints
    [System.Serializable]
    public class HandPointList
    {
        public HandPoint[] handPoint;
        //public GameObject handPoint1;

    }
    public HandPointList myHandPointList = new HandPointList();  //List of handPoints


    // Start is called before the first frame update
    void Start()
    {
        freqAux = 0;
        timeAux = 0;
       
        RowR = "NULL";
        RowL = "NULL";
        taskInfo = "NULL";

        
    }

    private void Awake()
    {
        if (GameManager.TaskChoice == 1) taskName = "task1";
        else taskName = "task2";

        filename = Application.dataPath + "/DataFiles/Log-" + System.DateTime.Now.ToString("HH-mm") + ".csv";
        tw = new StreamWriter(filename);
        tw.WriteLine(", RWristPosX, RWristPosY, RWristPosZ, RWristRotX, RWristRotY, RWristRotZ, " +
                     "  LWristPosX, LWristPosY, LWristPosZ, LWristRotX, LWristRotY, LWristRotZ,," +

                     "  RThumbPosX, RThumbPosY, RThumbPosZ, RThumbRotX, RThumbRotY, RThumbRotZ, " +
                     "  LThumbPosX, LThumbPosY, LThumbPosZ, LThumbRotX, LThumbRotY, LThumbRotZ,," +

                     "  RIndexPosX, RIndexPosY, RIndexPosZ, RIndexRotX, RIndexRotY, RIndexRotZ, " +
                     "  LIndexPosX, LIndexPosY, LIndexPosZ, LIndexRotX, LIndexRotY, LIndexRotZ,," +

                     "  RMiddlePosX, RMiddlePosY, RMiddlePosZ, RMiddleRotX, RMiddleRotY, RMiddleRotZ, " +
                     "  LMiddlePosX, LMiddlePosY, LMiddlePosZ, LMiddleRotX, LMiddleRotY, LMiddleRotZ,," +

                     "  RRingPosX, RRingPosY, RRingPosZ, RRingRotX, RRingRotY, RRingRotZ, " +
                     "  LRingPosX, LRingPosY, LRingPosZ, LRingRotX, LRingRotY, LRingRotZ,," +

                     "  RPinkyPosX, RPinkyPosY, RPinkyPosZ, RPinkyRotX, RPinkyRotY, RPinkyRotZ, " +
                     "  LPinkyPosX, LPinkyPosY, LPinkyPosZ, LPinkyRotX, LPinkyRotY, LPinkyRotZ,," +

                     "  BoatX, BoatZ, BoatRotY,, " +
                     "  RowR, RowL,," + taskName + ", Difficulty, Assist");



        tw.Close();

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (taskName == "task2")
        {
            taskInfo = boat.GetComponent<CoinGame>().gameEventAux;
            assistInfo = boat.GetComponent<CoinGame>().assistAux;
            difficultyInfo = boat.GetComponent<CoinGame>().difficultyAux;
        }
        else
        {
            taskInfo = boat.GetComponent<PathGame>().gameEventAux;
            assistInfo = boat.GetComponent<PathGame>().assistAux;
            difficultyInfo = boat.GetComponent<PathGame>().difficultyAux;
        }
        
        //Debug.Log(taskName + " - " + taskInfo);

        timeAux += Time.fixedDeltaTime;
        if(freqAux == 0) WriteCSV();
        //if (freqAux < amsFreq) freqAux++;
        //else freqAux = 0;
    }
    void OutputTime() {
        Debug.Log(Time.time);
    }


    public void WriteCSV()
    {
        if (myHandPointList.handPoint.Length > 0)
        {
            //WRISTS
            Vector3 auxPosR;
            Vector3 auxPosL;
            Quaternion auxRotR;
            Quaternion auxRotL;
            auxPosR = myHandPointList.handPoint[0].LeftObject.transform.localPosition;
            auxPosL = myHandPointList.handPoint[0].RightObject.transform.localPosition;
            auxRotR = myHandPointList.handPoint[0].LeftObject.transform.localRotation;
            auxRotL = myHandPointList.handPoint[0].RightObject.transform.localRotation;

            //THUMBS
            Vector3 auxPos1R;
            Vector3 auxPos1L;
            Quaternion auxRot1R;
            Quaternion auxRot1L;
            auxPos1R = myHandPointList.handPoint[1].LeftObject.transform.localPosition;
            auxPos1L = myHandPointList.handPoint[1].RightObject.transform.localPosition;
            auxRot1R = myHandPointList.handPoint[1].LeftObject.transform.localRotation;
            auxRot1L = myHandPointList.handPoint[1].RightObject.transform.localRotation;

            //INDEXES
            Vector3 auxPos2R;
            Vector3 auxPos2L;
            Quaternion auxRot2R;
            Quaternion auxRot2L;
            auxPos2R = myHandPointList.handPoint[2].LeftObject.transform.localPosition;
            auxPos2L = myHandPointList.handPoint[2].RightObject.transform.localPosition;
            auxRot2R = myHandPointList.handPoint[2].LeftObject.transform.localRotation;
            auxRot2L = myHandPointList.handPoint[2].RightObject.transform.localRotation;

            //MIDDLES
            Vector3 auxPos3R;
            Vector3 auxPos3L;
            Quaternion auxRot3R;
            Quaternion auxRot3L;
            auxPos3R = myHandPointList.handPoint[3].LeftObject.transform.localPosition;
            auxPos3L = myHandPointList.handPoint[3].RightObject.transform.localPosition;
            auxRot3R = myHandPointList.handPoint[3].LeftObject.transform.localRotation;
            auxRot3L = myHandPointList.handPoint[3].RightObject.transform.localRotation;

            //RINGS
            Vector3 auxPos4R;
            Vector3 auxPos4L;
            Quaternion auxRot4R;
            Quaternion auxRot4L;
            auxPos4R = myHandPointList.handPoint[4].LeftObject.transform.localPosition;
            auxPos4L = myHandPointList.handPoint[4].RightObject.transform.localPosition;
            auxRot4R = myHandPointList.handPoint[4].LeftObject.transform.localRotation;
            auxRot4L = myHandPointList.handPoint[4].RightObject.transform.localRotation;

            //PINKIES
            Vector3 auxPos5R;
            Vector3 auxPos5L;
            Quaternion auxRot5R;
            Quaternion auxRot5L;
            auxPos5R = myHandPointList.handPoint[5].LeftObject.transform.localPosition;
            auxPos5L = myHandPointList.handPoint[5].RightObject.transform.localPosition;
            auxRot5R = myHandPointList.handPoint[5].LeftObject.transform.localRotation;
            auxRot5L = myHandPointList.handPoint[5].RightObject.transform.localRotation;



            tw = new StreamWriter(filename, true);


            tw.WriteLine(timeAux + "," +
                            //WRISTS
                            auxPosR.x + "," + auxPosR.y + "," + auxPosR.z + "," +
                            auxRotR.x + "," + auxRotR.y + "," + auxRotR.z + "," + 
                            auxPosL.x + "," + auxPosL.y + "," + auxPosL.z + "," + 
                            auxRotL.x + "," + auxRotL.y + "," + auxRotL.z + "," + "," +
                            //THUMBS
                            auxPos1R.x + "," + auxPos1R.y + "," + auxPos1R.z + "," +
                            auxRot1R.x + "," + auxRot1R.y + "," + auxRot1R.z + "," +
                            auxPos1L.x + "," + auxPos1L.y + "," + auxPos1L.z + "," +
                            auxRot1L.x + "," + auxRot1L.y + "," + auxRot1L.z + "," + "," +
                            //INDEXES
                            auxPos2R.x + "," + auxPos2R.y + "," + auxPos2R.z + "," +
                            auxRot2R.x + "," + auxRot2R.y + "," + auxRot2R.z + "," +
                            auxPos2L.x + "," + auxPos2L.y + "," + auxPos2L.z + "," +
                            auxRot2L.x + "," + auxRot2L.y + "," + auxRot2L.z + "," + "," +
                            //MIDDLES
                            auxPos3R.x + "," + auxPos3R.y + "," + auxPos3R.z + "," +
                            auxRot3R.x + "," + auxRot3R.y + "," + auxRot3R.z + "," +
                            auxPos3L.x + "," + auxPos3L.y + "," + auxPos3L.z + "," +
                            auxRot3L.x + "," + auxRot3L.y + "," + auxRot3L.z + "," + "," +
                            //RINGS
                            auxPos4R.x + "," + auxPos4R.y + "," + auxPos4R.z + "," +
                            auxRot4R.x + "," + auxRot4R.y + "," + auxRot4R.z + "," +
                            auxPos4L.x + "," + auxPos4L.y + "," + auxPos4L.z + "," +
                            auxRot4L.x + "," + auxRot4L.y + "," + auxRot4L.z + "," + "," +
                            //PINKIES
                            auxPos5R.x + "," + auxPos5R.y + "," + auxPos5R.z + "," +
                            auxRot5R.x + "," + auxRot5R.y + "," + auxRot5R.z + "," +
                            auxPos5L.x + "," + auxPos5L.y + "," + auxPos5L.z + "," +
                            auxRot5L.x + "," + auxRot5L.y + "," + auxRot5L.z + "," + "," +
                            //------------------------------------------------------------
                            //BOAT'S 2D COORDINATES and ROTATION(Y)
                            boat.transform.position.x + "," + boat.transform.position.z + "," + boat.transform.rotation.eulerAngles.y+"," + ","
                            //ROWING + TASK INFO + ASSIST INFO
                            + RowR + "," + RowL + "," + "," + taskInfo + "," + difficultyInfo + "," + assistInfo);

            tw.Close();
        }
    }
}
