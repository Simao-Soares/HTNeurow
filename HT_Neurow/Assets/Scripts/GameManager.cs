using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject zone; //interaction zone

    //for the reload function
    public GameObject water;
    public GameObject player;
    

    //-------------------------------------------------- GAME SETTINGS --------------------------------------------------

    public static int ControlMethod = 1; //static -> instances of GameObject will share this value 
                                         //  1 -> BCI (arrowKeys)
                                         // -1 -> HT (leapMotion)

    public static int HemiLimb = 0;      //  0 -> No hemiparethic limb
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
		
        //trying to solve reflections bug:
        //reloadAssets(); 
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

    void reloadAssets(){
        water.SetActive(false);
        water.SetActive(true);
        player.SetActive(false);
        player.SetActive(true);
    }


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
}
