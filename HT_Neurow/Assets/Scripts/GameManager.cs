using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject zone;
    public GameObject water;
    public GameObject player;




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
}
