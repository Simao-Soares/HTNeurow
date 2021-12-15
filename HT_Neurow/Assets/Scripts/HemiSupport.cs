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


    /// <summary>
    /// wow
    /// </summary>

	public GameObject rightWrist;

	public GameObject rightPaddle;

	private bool auxR = false;

    public float shift = 1f; //maximum delta z  

	private float oldPos;

	private void Start()
	{
		oldPos = rightWrist.transform.position.z; //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement

	}



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

		//oldPos = rightWrist.transform.position.z;
		RotRightPaddle();

    }

	void RotRightPaddle()
	{
        //float xAngle = 30f;
        float yRot = 0;
        float zRot = -90f;

		float yAngle = rightPaddle.transform.eulerAngles.y;
		float zAngle = rightPaddle.transform.eulerAngles.z;


        float delta;


		if(auxR){
			

	        if (rightWrist.transform.position.z > oldPos) {

	            delta = rightWrist.transform.position.z - oldPos;

	            yRot += (delta / shift) * -15f; //wont work for second part of forward motion (0 -> -15 -> 0) 

	            zRot += (delta / shift) * -30f;

				Debug.Log(yAngle);
				Debug.Log(zAngle);

				if(  (yAngle <= 0) && (zAngle <= 270) && (zAngle >= 245))  // (yAngle >= -15) &&
				{
					rightPaddle.transform.Rotate(yRot/1000, 0, zRot/1000, Space.Self);

					Debug.Log(rightPaddle.transform.eulerAngles.y);
				}
					
				oldPos = rightWrist.transform.position.z;
			
	        }

			else {
				oldPos = rightWrist.transform.position.z;
			}
		}

		
	}



    void OnTriggerEnter(Collider other) 
    {
                 
		if (other.gameObject.name == "Contact Fingerbone" && gameObject.name == "HemiZone_L")  //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??    -------------->          IMPLENT TRIGGER WITH ATTACHMENT HANDS Objects
        {
			openLeft.SetActive(false);
            animatedLeft.SetActive(true);
            leftAnimator.SetBool("startGrab", true);


        }

		if (other.gameObject.name == "Contact Fingerbone" && gameObject.name == "HemiZone_R") //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??
        {
			openRight.SetActive(false);
			animatedRight.SetActive(true);
			rightAnimator.SetBool("startGrab", true); 
			auxR = true;
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
