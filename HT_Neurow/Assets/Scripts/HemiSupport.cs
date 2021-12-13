using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemiSupport : MonoBehaviour
{
    public Animator leftAnimator;
    public GameObject animatedLeft; //hand model with animator
    public GameObject openLeft; //opened hand model with hand binder
    public GameObject closedLeft; //opened hand model with hand binder


    //it may be possible to only have one object and disable/enable its components

    public Animator rightAnimator;
    public GameObject animatedRight;
    public GameObject openRight; 
    public GameObject closedRight;

	public GameObject rightWrist;

	public GameObject rightPaddle;

	private bool auxR = false;



    private void Update()
    {
        if (Input.GetKey("a"))
        {
            TestingLeft();
        }
        if (Input.GetKey("d"))
        {
            TestingRight();
        }

		while(auxR){
			RotRightPaddle();


			
		}
    }

	void RotRightPaddle()
	{
		
	}



    void OnTriggerEnter(Collider other) 
    {
                 
		if (other.gameObject.name == "Contact Fingerbone" && gameObject.name == "HemiZone_L")  //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??    -------------->          IMPLENT TRIGGER WITH ATTACHMENT HANDS Objects
        {
			openLeft.SetActive(false);
            animatedLeft.SetActive(true);
            leftAnimator.SetBool("startGrab", true);
			auxR = true;

        }

		if (other.gameObject.name == "Contact Fingerbone" && gameObject.name == "HemiZone_R") //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??
        {
			openRight.SetActive(false);
			animatedRight.SetActive(true);
			rightAnimator.SetBool("startGrab", true); 
        }
    }

    void TestingRight()
    {
        //openLeft.SetActive(false);
        animatedRight.SetActive(true);
        rightAnimator.SetBool("startGrab", true);
        //animatedRight.SetActive(false);
        //openRight.SetActive(false);
        //closedRight.SetActive(true);
    }

    void TestingLeft()
    {
        //openLeft.SetActive(false);
        animatedLeft.SetActive(true);
        leftAnimator.SetBool("startGrab", true);
        //animatedRight.SetActive(false);
        //openRight.SetActive(false);
        //closedRight.SetActive(true);
    }
}
