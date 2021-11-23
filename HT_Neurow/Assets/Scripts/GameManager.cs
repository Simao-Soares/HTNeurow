using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//for the reload function
    [SerializeField] private GameObject zone;
    public GameObject water;
    public GameObject player;


	public GameObject myPlayer;
	public BoatMovement _playerScript;




    // Start is called before the first frame update
    void Start()
    {
		_playerScript = myPlayer.GetComponent<BoatMovement>();
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


	void SetBCIControlMethod(){
		_playerScript.ControlMethod = 1;
	}

	void SetHTControlMethod(){
		_playerScript.ControlMethod = -1;
	}
}
