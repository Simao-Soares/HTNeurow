using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemiSupport : MonoBehaviour
{
    public Animator leftAnimator;
    public GameObject animatedLeft; //hand model with animator
    public GameObject openLeft; //opened hand model with hand binder
    public GameObject closedLeft; //opened hand model with hand binder

    public Animator rightAnimator;
    public GameObject animatedRight;
    public GameObject openRight; 
    public GameObject closedRight;



    bool rightSide;  //defines hemi side for rotations;
    bool aux = true; //signals beggining of wrist tracking

	public GameObject wrist;
	public GameObject paddle;

	

    public float maxReach = 0.2f; //maximum delta z  

	private float initPos;
    private float oldPos;



    private float testDistance;
    private float delta = 0;
    private float deltaRot = 0;
    private bool forward = true;
    bool forwardAux = true;






    private void Start()
	{
		initPos = wrist.transform.position.z; //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement
        oldPos = initPos;
        if (gameObject.name == "HemiZone_L") rightSide = false;
        else if (gameObject.name == "HemiZone_R") rightSide = true;

    }



    private void Update()
    {
        //---------------------  Test animations without the hand collisions: ---------------------

        if (Input.GetKey("a"))
        {
            TestingLeft();
        }
        if (Input.GetKey("d"))
        {
            TestingRight();
        }



        // ----------------------- Override wrist movement with arrowKeys --------------------------

        testDistance = wrist.transform.position.z - initPos;

        if (Input.GetKey(KeyCode.UpArrow) && testDistance < maxReach) 
        {
            wrist.transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow) && wrist.transform.position.z > initPos) 
        {
            wrist.transform.Translate(Vector3.back * 0.5f * Time.deltaTime);
        }
        //------------------------------------------------------------------------------------------

        
        RotPaddle(oldPos, delta, forward, forwardAux);
        oldPos = wrist.transform.position.z;

    }

    void RotPaddle(float oldPos, float delta, bool forward, bool forwardAux)
    {
        // -------------------- 4 phases of rowing movement: --------------------
        //  phase 1 (y:  0  -> -15) (z: -90  -> -105)
        //  phase 2 (y: -15 ->  0 ) (z: -105 -> -120)
        //  phase 3 (y:  0  -> -15) (z: -120 -> -105)
        //  phase 4 (y: -15 ->  0 ) (z: -105 ->  -90)

        //phases 1 and 2 with forward wrist motion  => forward = true
        //phases 3 and 4 with backward wrist motion => forward = false

        float currentPos = wrist.transform.position.z;
        float currentRotY = paddle.transform.eulerAngles.y;
        float currentRotZ = paddle.transform.eulerAngles.z;
        
        Quaternion target;

        delta = (currentPos - oldPos);
        deltaRot = (2*delta / maxReach) * 15f;  //dont know why i need the 2* but i'll take it


        if (currentPos >= initPos + maxReach) forwardAux = false;               //trying to implement the need to complete the whole movement!!!!!!!!!!!!
        else if (currentPos <= initPos) forwardAux = true;


        if (currentPos > oldPos)
        {
            forward = true;
            //Debug.Log("forward");
        }

        else if (currentPos < oldPos)
        {
            forward = false;
            //Debug.Log("backward");
        }

        //phase 1
        if (forward && currentPos <= initPos + maxReach/2)
        {
            if(rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.rotation = Quaternion.Slerp(paddle.transform.rotation, target, 500f * Time.deltaTime);
            Debug.Log("p1");
        }

        //phase 2
        else if (forward && currentPos > initPos + maxReach/2)
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY+(1.5f*deltaRot), currentRotZ-deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.rotation = Quaternion.Slerp(paddle.transform.rotation, target, 500f * Time.deltaTime);
            Debug.Log("p2");
        }
        
        //phase 3
        else if (!forward && currentPos >= initPos + maxReach/2)
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.rotation = Quaternion.Slerp(paddle.transform.rotation, target, 500f * Time.deltaTime);
            Debug.Log("p3");
        }

        //phase 4
        else if (!forward && currentPos < initPos + maxReach/2)
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.rotation = Quaternion.Slerp(paddle.transform.rotation, target, 500f * Time.deltaTime);
            Debug.Log("p4");

        }

        
        //Debug.Log("initpos -> " + initPos);
        //Debug.Log("halfway -> " + (initPos + maxReach/2));
        //Debug.Log("currentpos -> " + currentPos);

        //Debug.Log("RotY -> " + currentRotY);
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
			aux = true;
        }
    }



    void TestingRight()
    {
        animatedRight.SetActive(true);
        rightAnimator.SetBool("startGrab", true);
    }

    void TestingLeft()
    {
        animatedLeft.SetActive(true);
        leftAnimator.SetBool("startGrab", true);
    }
}
