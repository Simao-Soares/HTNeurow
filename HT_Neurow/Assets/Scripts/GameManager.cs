using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Leap.Unity;
using Assets.LSL4Unity.Scripts.Examples;

public class GameManager : MonoBehaviour
{

    public GameObject Player;
    public GameObject canvas;

    [Header("Menu Scene Panels and Sliders")]
    public GameObject mainPanel;
    public GameObject optPanel;
    public GameObject choicePanel;

    public Slider timeSlider;
    [HideInInspector] public int inputMinutes;
    [HideInInspector] public int inputSeconds;

    public Slider turnAngleSlider;
    public Slider boatSpeedSlider;
    public Slider turnSpeedSlider;
    public Slider turnSenseSlider;

    public Slider motionRangeSlider;
    public Slider colliderSizeSlider;

    public Slider challengeLevelSlider;
    public Slider angleDevSlider;
    public Slider maxDistanceSlider;
    public Slider maxDistance2Slider;
    public Slider selfCorrectSlider;
    public Slider autoCorrectSlider;

    public Slider playAreaSlider;
    public Slider objectiveNumSlider;
    public Slider objectiveRadSlider;

    public Slider maxDistanceBuoySlider;
    public Slider selfCorrectBuoySlider;
    public Slider autoCorrectBuoySlider;

    public Slider objectivePosZSlider;
    public Slider objectivePosXSlider;
    

    [System.Serializable]
    public class BCI_Hands
    {
        public GameObject RightHand;
        public GameObject LeftHand;
    }

    [System.Serializable]
    public class BCI_Hands_List
    {
        public BCI_Hands[] BCI_HandModels;
    }

    [Header("BCI Hands and Animators")]
    public BCI_Hands_List myBCI_Hands_List = new BCI_Hands_List();  //List of handPoints
	public Animator leftPaddleAnim;
	public Animator rightPaddleAnim;


    [Header("HT ELEMENTS:")]
    public GameObject LeapHandController;
    public GameObject InteractionManager;
    public GameObject handModels;
    public GameObject zone; //interaction zone
    [Header("HT - Impaired Limb Support:")]
    public GameObject auxTrackerR;
    public GameObject auxTrackerL;
    public GameObject attachtmentHands;
    public GameObject hemiAnimHand_R;
    public GameObject hemiAnimHand_L;
    public GameObject leftPaddleZone;
    public GameObject rightPaddleZone;


    [Header("Task #1 UI and Settings")]
    public GameObject PathUI;
    public GameObject PathElements;



    [Header("Task #2 UI and Settings")]
    public GameObject CoinUI;
    public GameObject CoinLighting;
    public GameObject TimeUI;

    [HideInInspector] public static bool editAuxGM;

    [Header("LSL")]
    public GameObject listener;

    //-----------------//-----------------//---------------  DEBUG  -----------------//-----------------//-----------------
    public static bool debugArrowMovement = true;
    //-----------------//-----------------//----------------------//-----------------//-----------------//-----------------

    //-----------------//-----------------//---------------  TRAINING  -----------------//-----------------//-----------------
    public static bool training = true;
    //-----------------//-----------------//----------------------//-----------------//-----------------//-----------------



    //-------------------------------------------------- DEFAULT GAME SETTINGS --------------------------------------------------		 
    //static -> instances of GameObject will share this value

    public static int TaskChoice = 1;   //  0 -> disabled                                                            
                                        //  1 -> task1 (path)       
                                        // -1 -> task2 (coins)

    public static int SelectedPreset = -1; 

    public static int ControlMethod = 1;  //  1 -> BCI (arrowKeys)
                                          // -1 -> HT (leapMotion)


    public static int HemiLimb = 0;      //  0 -> No hemiparethic limb                                                                 
                                         //  1 -> Right hemiparethic limb
                                         // -1 -> Left hemiparethic limb
                                         //  2 -> Both

    public static int Gender =  1;       //  1 -> Male
                                         // -1 -> Female

    //public static int Forward = 1;     //  1 -> Auto (always moving forward)
    // -1 -> Manual (forward movement based on rowing)


    public static int taskDuration = 120;

    public static bool assistiveMechs = true; //-> if assistivr mechanisms are enabled or not


    //HEMI SUPPORT
    public static float motionRange = 0.15f;
    public static float colliderSize = 0.3f;
    public static int trackAxis = -1;      //  0 -> X axis                                                               
                                           //  1 -> Y axis        
                                           // -1 -> Z axis

    //LSL OPTIONS
    public static string streamName = "openvibeMarkers";
    public static string streamType = "Markers";

    //BOAT MOVEMENT
    public static float turnAngle = 20f;
    public static float boatSpeed = 4f;
    public static float turnSpeed = 1f;
    public static int turnSense = 5;
    public static bool invertTurn = false;

    //TASK #1
    public static int challengeLevel = 2;
    public static int angleDev = 150;
    public static float maxDistance = 5f;
    public static float maxDistance2 = 10f;
    public static int selfCorrect = 10;
    public static float autoCorrect = 3f;

    //TASK #2
    public static int playArea = 2;
    public static int objectiveNum = 3;
    public static float objectiveRad = 2f;

    public static float maxDistanceBuoy = 30f;
    public static int selfCorrectBuoy = 10;
    public static float autoCorrectBuoy = 3f;

    public static int objectivePosZ = 5;
    public static int objectivePosX = 3;

    public static bool bciSpecificUI = true;

    //-------------------------------------------------------------------------------------------------------------------

    public static bool optionsMenuAux = false;

    [HideInInspector] public static bool updateSettings;


    // Start is called before the first frame update
    void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Menu")
        {
            mainPanel.SetActive(true);
            optPanel.SetActive(false);
            choicePanel.SetActive(false);
        }

        else if (sceneName == "RowingSim")
        {
            //------------------------------------------- RowingSim SCENE SETUP ---------------------------------------------

            //-----------------------------------------------------------------------
            //Non-Task specific Settings:
            //-----------------------------------------------------------------------

            //--------------- Boat Movement ---------------
            var movement = Player.GetComponent<BoatMovement>();
            movement.rotAngle = turnAngle;
            movement.spinningTime = 1 / turnSpeed;
            movement.forwardForce = boatSpeed/2;

            //------------------ General ------------------
            if (Gender == 1) //activate MALE hand models
            {
                myBCI_Hands_List.BCI_HandModels[0].RightHand.SetActive(true);
                myBCI_Hands_List.BCI_HandModels[0].LeftHand.SetActive(true);

                handModels.GetComponent<HandModelManager>().EnableGroup("MaleMedium");
                handModels.GetComponent<HandModelManager>().DisableGroup("FemaleMedium");
            }
            else if (Gender == -1) //activate FEMALE hand models
            {
                myBCI_Hands_List.BCI_HandModels[1].RightHand.SetActive(true);
                myBCI_Hands_List.BCI_HandModels[1].LeftHand.SetActive(true);

                handModels.GetComponent<HandModelManager>().EnableGroup("FemaleMedium");
                handModels.GetComponent<HandModelManager>().DisableGroup("MaleMedium");
            }

            //BCI
            if (ControlMethod == 1)
            {
                TimeUI.SetActive(false);
                LeapHandController.GetComponent<LeapServiceProvider>().enabled = false;
                InteractionManager.SetActive(false);
                leftPaddleAnim.enabled = true;
                rightPaddleAnim.enabled = true;

                rightPaddleZone.SetActive(false);
                leftPaddleZone.SetActive(false);

                auxTrackerR.SetActive(false);
                auxTrackerL.SetActive(false);
                attachtmentHands.SetActive(false);

                //LSL
                listener.GetComponent<ReceiveLSLmarkers>().StreamName = streamName;
                listener.GetComponent<ReceiveLSLmarkers>().StreamType = streamType;

            }

            //HT
            else if (ControlMethod == -1)
            {
                LeapHandController.GetComponent<LeapServiceProvider>().enabled = true;
                //deactivate BCI Hand models
                myBCI_Hands_List.BCI_HandModels[0].RightHand.SetActive(false);
                myBCI_Hands_List.BCI_HandModels[0].LeftHand.SetActive(false);
                myBCI_Hands_List.BCI_HandModels[1].RightHand.SetActive(false);
                myBCI_Hands_List.BCI_HandModels[1].LeftHand.SetActive(false);

                leftPaddleAnim.enabled = false;
                rightPaddleAnim.enabled = false;

                InteractionManager.SetActive(true);

                switch (HemiLimb)
                {
                    case 0:
                        //attachtmentHands.SetActive(false);

                        rightPaddleZone.SetActive(false);
                        leftPaddleZone.SetActive(false);


                        //auxTrackerR.SetActive(false); //----------------------------------------------------------------------- using them to measure rowing speed
                        //auxTrackerL.SetActive(false);


                        //hemiAnimHand_R.SetActive(false);
                        //hemiAnimHand_L.SetActive(false);
                        break;

                    case 1:
                        rightPaddleZone.SetActive(true);
                        auxTrackerR.SetActive(true);
                        //hemiAnimHand_R.SetActive(true);

                        leftPaddleZone.SetActive(false);
                        auxTrackerL.SetActive(false);
                        //hemiAnimHand_L.SetActive(false);

                        break;

                    case -1:
                        leftPaddleZone.SetActive(true);
                        auxTrackerL.SetActive(true);
                        //hemiAnimHand_L.SetActive(true);

                        rightPaddleZone.SetActive(false);
                        auxTrackerR.SetActive(false);
                        //hemiAnimHand_R.SetActive(false);
                        break;

                    case 2:
                        rightPaddleZone.SetActive(true);
                        leftPaddleZone.SetActive(true);
                        auxTrackerR.SetActive(true);
                        auxTrackerL.SetActive(true);

                        //hemiAnimHand_R.SetActive(true);
                        //hemiAnimHand_L.SetActive(true);
                        break;
                }

                if (HemiLimb != 0)
                {
                    //HemiZone -> HemiSupport
                    var R_Support = rightPaddleZone.GetComponent<HemiSupport>();
                    var L_Support = leftPaddleZone.GetComponent<HemiSupport>();
                    R_Support.maxReach = motionRange;
                    L_Support.maxReach = motionRange;

                    //Still to be implemented               //<----------------------------------------------------------------------- 
                    //R_Support.colliderRad = colliderSize;
                    //L_Support.colliderRad = colliderSize;
                    //R_Support.orientation = trackAxis;
                    //L_Support.orientation = trackAxis;
                }
            }

            //TASK
            if (TaskChoice == 1)
            {
                Player.GetComponent<PathGame>().enabled = true;
                PathUI.SetActive(true);
                PathElements.SetActive(true);

                Player.GetComponent<CoinGame>().enabled = false;
                CoinUI.SetActive(false);
            }
            else if (TaskChoice == -1)
            {
                Player.GetComponent<PathGame>().enabled = false;
                PathUI.SetActive(false);
                PathElements.SetActive(false);

                Player.GetComponent<CoinGame>().enabled = true;
                CoinUI.SetActive(true);
            }


        }
    }



    // Update is called once per frame
    void Update()
    {
        if (updateSettings)
        {
            canvas.GetComponent<MenuScript>().updateSettingsAux = true;
        }
        updateSettings = false;


        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;


        if(sceneName == "RowingSim") {
            
            if (Input.GetKey("l")){
                zone.SetActive(true);
            }
            else{
                zone.SetActive(false);
            }


        }


        if (Input.GetKey("escape")) SceneManager.LoadScene("Menu");      //<----------------------------------------------------------------------- for Debug Purposes
        if (Input.GetKey("escape") && sceneName == "Menu") Application.Quit();


        if (optionsMenuAux && sceneName == "Menu")
        {
            editAuxGM = GetComponent<PresetFiller>().editAux;

            //Set inicial slider parameters
            timeSlider.value = taskDuration;

            motionRangeSlider.value = motionRange;
            colliderSizeSlider.value = colliderSize;

            turnAngleSlider.value = turnAngle;
            boatSpeedSlider.value = boatSpeed;
            turnSpeedSlider.value = turnSpeed;
            turnSenseSlider.value = turnSense;

            challengeLevelSlider.value = challengeLevel;
            angleDevSlider.value = angleDev;
            maxDistanceSlider.value = maxDistance;
            maxDistance2Slider.value = maxDistance2;
            selfCorrectSlider.value = selfCorrect;
            autoCorrectSlider.value = autoCorrect;

            playAreaSlider.value = playArea;
            objectiveNumSlider.value = objectiveNum;
            objectiveRadSlider.value = objectiveRad;
            maxDistanceBuoySlider.value = maxDistanceBuoy;
            selfCorrectBuoySlider.value = selfCorrectBuoy;
            autoCorrectBuoySlider.value = autoCorrectBuoy;
            objectivePosZSlider.value = objectivePosZ;
            objectivePosXSlider.value = objectivePosX;
        }
    }

    public void ReadInputMinutes(string input)
    {
        inputMinutes = int.Parse(input);
        SetTime(1);
    }

    public void ReadInputSeconds(string input)
    {
        inputSeconds = int.Parse(input);
        SetTime(2);
    }


    //------------------------------------------  SET FUNCTIONS  ------------------------------------------

    //-----------------------------------------------------------------------
    //General Settings:
    //-----------------------------------------------------------------------

    public void SetTask(int taskAux)
    {
        TaskChoice = taskAux;
        SceneManager.LoadScene("RowingSim");
    }

    public void SetBCIControlMethod(){
		ControlMethod = 1;
        updateSettings = true;
    }
	public void SetHTControlMethod(){
		ControlMethod = -1;
        updateSettings = true;
    }
    public void SetHemiLimb(int HemiAux)
    {
        switch (HemiLimb)
        {
            case 0:
                HemiLimb = HemiAux;
                updateSettings = true;
                break;
            case 1:
                HemiLimb = -HemiAux + 1;
                updateSettings = true;
                break;
            case -1:
                HemiLimb = HemiAux + 1;
                updateSettings = true;
                break;
            case 2:
                HemiLimb = - HemiAux;
                updateSettings = true;
                break;
        }
    }

    public void SetGender(int GenderAux)
    {
        Gender = GenderAux;
        updateSettings = true;
    }

    public void SetTime(int auxTime)
    {
        //slider
        if (auxTime == 0) taskDuration = (int)(Mathf.Round(timeSlider.value / 10f) * 10f);
        //minutesInput
        else if (auxTime == 1 && inputMinutes<=30)
        {
            
            taskDuration = taskDuration % 60;
            taskDuration += inputMinutes*60;
        }
        //else if (auxTime == 2)
        //{
        //    taskDuration -= taskDuration % 60;
        //    taskDuration += inputSeconds;
        //}
        updateSettings = true;
        //Debug.Log(taskDuration);
        
    }

    public void SetAssistiveMechs()
    {
        if (assistiveMechs) assistiveMechs = false;
        else assistiveMechs = true;
        updateSettings = true;

    }

    //-----------------------------------------------------------------------
    //Hemi Support Settings:
    //-----------------------------------------------------------------------

    public void SetMotionRange()
    {
        motionRange = Mathf.Round(motionRangeSlider.value * 100f) / 100f;
        updateSettings = true;
    }

    public void SetTrackedAxis(int AxisAux)
    {
        trackAxis = AxisAux;
        updateSettings = true;

    }
    public void SetColliderSize ()
    {
        colliderSize = Mathf.Round(colliderSizeSlider.value * 100f) / 100f;
        updateSettings = true;
    }


    //-----------------------------------------------------------------------
    //LSL Settings:
    //-----------------------------------------------------------------------

    public void SetStreamName(string input)
    {
        streamName = input;
    }

    public void SetStreamType(string input)
    {
        streamType = input;
    }


    //-----------------------------------------------------------------------
    //Boat Movement Settings:
    //-----------------------------------------------------------------------
    public void SetTurnAngle()
    {
        turnAngle = ((int)turnAngleSlider.value);
        updateSettings = true;
    }
    public void SetBoatSpeed()
    {
        boatSpeed = Mathf.Round(boatSpeedSlider.value * 10f) / 10f;
        updateSettings = true;
    }
    public void SetTurnSpeed()
    {
        turnSpeed = ((int)turnSpeedSlider.value);
        updateSettings = true;
    }
    public void SetTurnSense()
    {
        turnSense = ((int)turnSenseSlider.value);
        updateSettings = true;
    }
    public void SetInvertTurn()
    {
        if(invertTurn) invertTurn=false;
        else invertTurn = true;
        updateSettings = true;

    }
    //-----------------------------------------------------------------------
    //Task #1 Settings:
    //-----------------------------------------------------------------------
    public void SetChallengeLevel()
    {
        challengeLevel = ((int)challengeLevelSlider.value);
        updateSettings = true;
    }
    public void SetAngleDev()
    {
        angleDev = ((int)angleDevSlider.value);
        updateSettings = true;
    }
    public void SetMaxDistance()
    {
        maxDistance = ((int)maxDistanceSlider.value);
        updateSettings = true;
    }
    public void SetMaxDistance2()
    {
        maxDistance2 = ((int)maxDistance2Slider.value);
        updateSettings = true;
    }
    public void SetSelfCorrectTime()
    {
        selfCorrect = ((int)selfCorrectSlider.value);
        updateSettings = true;
    }
    public void SetAutoCorrectSpeed()
    {
        autoCorrect = ((int)autoCorrectSlider.value);
        updateSettings = true;
    }

    //-----------------------------------------------------------------------
    //Task #2 Settings:
    //-----------------------------------------------------------------------
    public void SetPlayAreaSize()
    {
        playArea = ((int)playAreaSlider.value);
        updateSettings = true;
    }
    public void SetNumberOfObjectives()
    {
        objectiveNum = ((int)objectiveNumSlider.value);
        updateSettings = true;
    }
    public void SetObjectiveRad()
    {
        objectiveRad = ((int)objectiveRadSlider.value);
        updateSettings = true;
    }

    public void SetMaxDistanceBuoy()
    {
        maxDistanceBuoy = ((int)maxDistanceBuoySlider.value);
        updateSettings = true;
    }
    public void SetSelfCorrectBuoy()
    {
        selfCorrectBuoy = ((int)selfCorrectBuoySlider.value);
        updateSettings = true;
    }
    public void SetAutoCorrectBuoy()
    {
        autoCorrectBuoy = ((int)autoCorrectBuoySlider.value);
        updateSettings = true;
    }


    public void SetObjectivePosZ()
    {
        objectivePosZ = ((int)objectivePosZSlider.value);
        updateSettings = true;
    }

    public void SetObjectivePosX()
    {
        objectivePosX = ((int)objectivePosXSlider.value);
        updateSettings = true;
    }

    public void SetBCISpecificUI()
    {
        if (bciSpecificUI) bciSpecificUI = false;
        else bciSpecificUI = true;
        updateSettings = true;
    }

    //------------------------------------------------------------------------------------------------------------------------


    //GameOver
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}


