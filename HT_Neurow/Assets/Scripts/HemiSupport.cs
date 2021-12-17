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

    public float maxReach = 0.2f; //maximum delta z  

	private float initPos;




    private float testDistance;
    private float delta = 0;
    private float deltaRot = 0;
    private bool forward;






    private void Start()
	{
		initPos = rightWrist.transform.position.z; //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement
        
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



        // ------------------------------ Override with arrowKeys ---------------------------------
        testDistance = rightWrist.transform.position.z + 0.2155255f;

        if (Input.GetKey(KeyCode.UpArrow) && testDistance <= maxReach) 
        {
            rightWrist.transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow) && rightWrist.transform.position.z >= -0.2155255)
        {
            rightWrist.transform.Translate(Vector3.back * 0.5f * Time.deltaTime);
        }
        //-----------------------------------------------------------------------------------------

        
        RotRightPaddle(initPos, delta, forward);
        //oldPos = rightWrist.transform.position.z;

    }

    void RotRightPaddle(float oldPos, float delta, bool forward)
    {
        // -------------------- 4 phases of rowing movement: --------------------
        //  phase 1 (y:  0  -> -15) (z: -90  -> -105)
        //  phase 2 (y: -15 ->  0 ) (z: -105 -> -120)
        //  phase 3 (y:  0  -> -15) (z: -120 -> -105)
        //  phase 4 (y: -15 ->  0 ) (z: -105 ->  -90)

        //phases 1 and 2 with forward wrist motion  => forward = true
        //phases 3 and 4 with backward wrist motion => forward = false

        float currentPos = rightWrist.transform.position.z;
        Quaternion target;

        delta = (currentPos - initPos);
        deltaRot = (2*delta / maxReach) * 15f;                                                      //dont know why i need the 2* but i'll take it 

        if (delta == 0) forward = true;

        else if (delta >= maxReach) {
            forward = false;
            
        } 

        //phase 1
        else if (delta > 0 && delta < maxReach/2 && forward)
        {
            target = Quaternion.Euler(0, -deltaRot, -90-deltaRot);
            rightPaddle.transform.rotation = Quaternion.Slerp(rightPaddle.transform.rotation, target, 500f * Time.deltaTime);
        }

        //phase 2
        else if ( delta > maxReach/2 && forward)
        {
            target = Quaternion.Euler(0, -30+deltaRot, -90-deltaRot);
            rightPaddle.transform.rotation = Quaternion.Slerp(rightPaddle.transform.rotation, target, 500f * Time.deltaTime);
        }

        //phase 3
        else if (delta < maxReach/2 && !forward)
        {
            target = Quaternion.Euler(0, deltaRot, -90+deltaRot);
            rightPaddle.transform.rotation = Quaternion.Slerp(rightPaddle.transform.rotation, target, 500f * Time.deltaTime);
            Debug.Log("mekieee");
        }

        //phase 4
        else if (delta > maxReach/2 && !forward)
        {
            target = Quaternion.Euler(0, 30-deltaRot, -90+deltaRot);
            rightPaddle.transform.rotation = Quaternion.Slerp(rightPaddle.transform.rotation, target, 1f * Time.deltaTime);
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
