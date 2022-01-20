using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject zone; //interaction zone

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

    public BCI_Hands_List myBCI_Hands_List = new BCI_Hands_List();  //List of handPoints

    //Contact zones for Hemiparetic Limb Support
    public GameObject leftPaddleZone;
    public GameObject rightPaddleZone;

	public Animator leftPaddleAnim;
	public Animator rightPaddleAnim;


	public GameObject InteractionManager;

    //General
    public Slider timeSlider;
    public Slider turnAngleSlider;
    public Slider boatSpeedSlider;
    public Slider turnSpeedSlider;

    //Task #1
    public Slider challengeLevelSlider;
    public Slider angleDevSlider;
    public Slider maxDistanceSlider;
    public Slider maxDistance2Slider;
    public Slider selfCorrectSlider;
    public Slider autoCorrectSlider;

    //Task #2
    public Slider playAreaSlider;
    public Slider objectiveNumSlider;
    public Slider objectiveRadSlider;




    //-------------------------------------------------- GAME SETTINGS --------------------------------------------------		 // NOT DEFAULT, ONLY FOR TESTING

    public static int ControlMethod = -1; //static -> instances of GameObject will share this value 
                                          //  1 -> BCI (arrowKeys)
                                          // -1 -> HT (leapMotion)

    public static int HemiLimb = 2;      //  0 -> No hemiparethic limb                                                                 
                                         //  1 -> Right hemiparethic limb
                                         // -1 -> Left hemiparethic limb
                                         //  2 -> Both

    public static int Gender = -1;       //  1 -> Male
                                         // -1 -> Female

    //public static int Forward = 1;     //  1 -> Auto (always moving forward)
                                         // -1 -> Manual (forward movement based on rowing)

    public static int taskDuration = 120;

    //BOAT MOVEMENT
    public static float turnAngle = 20f;
    public static float boatSpeed = 1f;
    public static float turnSpeed = 1f;

    //TASK #1
    public static int challengeLevel = 2;
    public static int angleDev = 150;
    public static float maxDistance = 5f;
    public static float maxDistance2 = 10f;
    public static int selfCorrect = 10;
    public static float autoCorrect = 1f;

    //TASK #2
    public static int playArea = 300;
    public static int objectiveNum = 3;
    public static float objectiveRad = 1f;

    //-------------------------------------------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        
        //Set inicial parameters
        timeSlider.value = taskDuration;
        turnAngleSlider.value = turnAngle;
        boatSpeedSlider.value = boatSpeed;
        turnSpeedSlider.value = turnSpeed;
        challengeLevelSlider.value = challengeLevel;
        angleDevSlider.value = angleDev;
        maxDistanceSlider.value = maxDistance;
        maxDistance2Slider.value = maxDistance2;
        selfCorrectSlider.value = selfCorrect;
        autoCorrectSlider.value = autoCorrect;
        playAreaSlider.value = playArea;
        objectiveNumSlider.value = objectiveNum;
        objectiveRadSlider.value = objectiveRad;

        if (sceneName == "RowingSim")
        {
            //------------------------------------------- RowingSim SCENE SETUP ---------------------------------------------

            rightPaddleZone.SetActive(false);
            leftPaddleZone.SetActive(false);

            if (GameManager.Gender == 1) //activate MALE hand models
            {
                myBCI_Hands_List.BCI_HandModels[0].RightHand.SetActive(true);
                myBCI_Hands_List.BCI_HandModels[0].LeftHand.SetActive(true);
            }

            else if (GameManager.Gender == -1) //activate FEMALE hand models
            {
                myBCI_Hands_List.BCI_HandModels[1].RightHand.SetActive(true);
                myBCI_Hands_List.BCI_HandModels[1].LeftHand.SetActive(true);
            }

            switch (GameManager.HemiLimb)
            {
                case 1:
                    rightPaddleZone.SetActive(true);
                    break;

                case -1:
                    leftPaddleZone.SetActive(true);
                    break;

                case 2:
                    rightPaddleZone.SetActive(true);
                    leftPaddleZone.SetActive(true);
                    break;
            }


            //SEPARATE CONTROL METHODS
            //BCI
            if (GameManager.ControlMethod == 1){ 

				InteractionManager.SetActive(false);
				leftPaddleAnim.enabled = true;
				rightPaddleAnim.enabled = true;

                rightPaddleZone.SetActive(false);
                leftPaddleZone.SetActive(false);

                //falta os InteractionBehaviour das paddles e mais coisas de certeza
            }
            //HT
            else if (GameManager.ControlMethod == -1){ 
				
				//deactivate BCI Hand models
				myBCI_Hands_List.BCI_HandModels[0].RightHand.SetActive(false);
				myBCI_Hands_List.BCI_HandModels[0].LeftHand.SetActive(false);
				myBCI_Hands_List.BCI_HandModels[1].RightHand.SetActive(false);
				myBCI_Hands_List.BCI_HandModels[1].LeftHand.SetActive(false);

				leftPaddleAnim.enabled = false;
				rightPaddleAnim.enabled = false;
			}
            //---------------------------------------------------------------------------------------------------------------
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("l")){
            zone.SetActive(true);
        }
        else{
            zone.SetActive(false);
        }
        if (Input.GetKey("escape")) Application.Quit();
    }

    //void reloadAssets(){
    //    water.SetActive(false);
    //    water.SetActive(true);
    //    player.SetActive(false);
    //    player.SetActive(true);
    //}

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //------------------------------------------  SET FUNCTIONS  ------------------------------------------


    //General Settings:

    public void SetBCIControlMethod(){
		ControlMethod = 1;
	}

	public void SetHTControlMethod(){
		ControlMethod = -1;
	}

    public void SetHemiLimb(int HemiAux)
    {
        switch (HemiLimb)
        {
            case 0:
                HemiLimb = HemiAux;
                break;
            case 1:
                HemiLimb = -HemiAux + 1;
                break;
            case -1:
                HemiLimb = HemiAux + 1;
                break;
            case 2:
                HemiLimb = - HemiAux;
                break;
        }
    }

    public void SetGender(int GenderAux)
    {
        Gender = GenderAux;
    }

    public void SetTime()
    {
        taskDuration = ((int)timeSlider.value);
    }

    public void SetTurnAngle()
    {
        turnAngle = ((int)turnAngleSlider.value);
    }

    public void SetBoatSpeed()
    {
        boatSpeed = Mathf.Round(boatSpeedSlider.value * 100f) / 100f;
    }

    public void SetTurnSpeed()
    {
        turnSpeed = ((int)turnSpeedSlider.value);
    }

    //public void SetForward(int ForwardAux)
    //{
    //    Forward = ForwardAux;
    //}


    //Task #1 Settings:

    public void SetChallengeLevel()
    {
        challengeLevel = ((int)challengeLevelSlider.value);
    }
    public void SetAngleDev()
    {
        angleDev = ((int)angleDevSlider.value);
    }
    public void SetMaxDistance()
    {
        maxDistance = ((int)maxDistanceSlider.value);
    }
    public void SetMaxDistance2()
    {
        maxDistance2 = ((int)maxDistance2Slider.value);
    }
    public void SetSelfCorrectTime()
    {
        selfCorrect = ((int)selfCorrectSlider.value);
    }
    public void SetAutoCorrectSpeed()
    {
        autoCorrect = ((int)autoCorrectSlider.value);
    }


    //Task #2 Settings:

    public void SetPlayAreaSize()
    {
        playArea = ((int)playAreaSlider.value);
    }
    public void SetNumberOfObjectives()
    {
        objectiveNum = ((int)objectiveNumSlider.value);
    }
    public void SetObjectiveRad()
    {
        objectiveRad = ((int)objectiveRadSlider.value);
    }




    //------------------------------------------------------------------------------------------------------------------------
}
