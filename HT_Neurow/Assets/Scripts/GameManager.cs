using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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








	//-------------------------------------------------- GAME SETTINGS --------------------------------------------------		 // NOT DEFAULT, ONLY FOR TESTING

    public static int ControlMethod = 1; //static -> instances of GameObject will share this value 
                                         //  1 -> BCI (arrowKeys)
                                         // -1 -> HT (leapMotion)

    public static int HemiLimb = 2;     //  0 -> No hemiparethic limb                                                                 
                                         //  1 -> Right hemiparethic limb
                                         // -1 -> Left hemiparethic limb
                                         //  2 -> Both

    public static int Gender = 1;        //  1 -> Male
                                         // -1 -> Female

    public static int Forward = 1;       //  1 -> Auto (always moving forward)
                                         // -1 -> Manual (forward movement based on rowing)

    //-------------------------------------------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

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

                rightPaddleZone.SetActive(true);
                leftPaddleZone.SetActive(true);
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

    public void SetForward(int ForwardAux)
    {
        Forward = ForwardAux;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");

    }
}
