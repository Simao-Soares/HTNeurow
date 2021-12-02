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








    //-------------------------------------------------- GAME SETTINGS --------------------------------------------------

    public static int ControlMethod = 1; //static -> instances of GameObject will share this value 
                                         //  1 -> BCI (arrowKeys)
                                         // -1 -> HT (leapMotion)

    public static int HemiLimb = -1;     //  0 -> No hemiparethic limb                                                                  // FOR TESTING
                                         //  1 -> Right hemiparethic limb
                                         // -1 -> Left hemiparethic limb

    public static int Gender = 1;        //  1 -> Male
                                         // -1 -> Female

    public static int Forward = 1;       //  1 -> Auto (always moving forward)
                                         // -1 -> Manual (forward movement based on rowing)

    //-------------------------------------------------------------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------------- SCENE SETUP -------------------------------------------------
        rightPaddleZone.SetActive(false);
        leftPaddleZone.SetActive(false);
        //---------------------------------------------------------------------------------------------------------------




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

        if (GameManager.HemiLimb == 1) //activate RIGHT hemiparetic limb support
        {
            rightPaddleZone.SetActive(true);
        }

        else if (GameManager.HemiLimb == -1) //activate LEFT hemiparetic limb support
        {
            leftPaddleZone.SetActive(true);
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
        HemiLimb = HemiAux;
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
