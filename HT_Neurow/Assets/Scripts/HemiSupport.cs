using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemiSupport : MonoBehaviour
{
    public Animator animator;
    public GameObject animatedModel; //hand model with animator
    public GameObject handModel; //opened hand model with hand binder


    private bool rightSide;


    private bool auxTrack = false; //signals beggining of wrist tracking

	public GameObject wrist;
	public GameObject paddle;

	

    public float maxReach = 0.2f; //maximum delta z  

	private float initPos;
    private float oldPos;



    private float testDistance;
    private float delta = 0;
    private float deltaRot = 0;
    private bool forward = true;
    private bool forwardAux = true;






    private void Start()
	{
		//initPos = wrist.transform.position.z; //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement
        //oldPos = initPos;
        if (gameObject.name == "HemiZone_L") rightSide = false;
        else if (gameObject.name == "HemiZone_R") rightSide = true;

    }



    private void Update()
    {
//        //---------------------  Test animations without the hand collisions: ---------------------
//
//        if (Input.GetKey("g"))
//        {
//            auxTrack = true;
//            TestingGraspAnim();
//        }
//
//
//
//        // ----------------------- Override wrist movement with arrowKeys --------------------------
//
//        testDistance = wrist.transform.localPosition.z - initPos;
//
//        if (Input.GetKey(KeyCode.UpArrow) && testDistance < maxReach) 
//        {
//            wrist.transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
//        }
//
//        if (Input.GetKey(KeyCode.DownArrow) && wrist.transform.localPosition.z > initPos) 
//        {
//            wrist.transform.Translate(Vector3.back * 0.5f * Time.deltaTime);
//        }
//        //------------------------------------------------------------------------------------------

        

		if (auxTrack){	// && oldPos >= initPos && oldPos <= initPos + maxReach) {   //-----------------------------------------> this is why it doesnt do the second part of the rowing movement 

			if( oldPos >= initPos && oldPos <= initPos + maxReach){
	            RotPaddle(oldPos, delta, forward, forwardAux);
			}
			oldPos = wrist.transform.position.z;
        }

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
        float currentRotY = paddle.transform.localEulerAngles.y;
        float currentRotZ = paddle.transform.localEulerAngles.z;
        
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
		if (forward && forwardAux && currentPos <= initPos + maxReach/2) //&& forwardAux
        {
            if(rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
            Debug.Log("p1");
        }

        //phase 2
		else if (forward && forwardAux && currentPos > initPos + maxReach/2) //&& forwardAux
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY+(1.5f*deltaRot), currentRotZ-deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
           	Debug.Log("p2");
        }
        
        //phase 3
		else if (!forward && forwardAux && currentPos >= initPos + maxReach/2) //&& !forwardAux
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
            Debug.Log("p3");
        }

        //phase 4
		else if (!forward && forwardAux && currentPos < initPos + maxReach/2) //&& !forwardAux
        {
            if (rightSide) target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ - deltaRot);
            else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

            paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
            Debug.Log("p4");

        }
    }




    void OnTriggerEnter(Collider other) 
    {
                 
		if (other.gameObject.name == "Contact Fingerbone")  //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??    -------------->          IMPLENT TRIGGER WITH ATTACHMENT HANDS Objects
        {
			handModel.SetActive(false);            //works as long as patient doesnt leave LeapMotion tacking area
            animatedModel.SetActive(true);
            animator.SetBool("startGrab", true);

			initPos = wrist.transform.position.z; //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement
			oldPos = initPos;
			//Debug.Log(initPos + maxReach);

            auxTrack = true;

			gameObject.GetComponent<SphereCollider>().enabled = false;  //prevent subsequent collisions
			gameObject.GetComponent<MeshRenderer>().enabled = false;



        }
    }



    void TestingGraspAnim()
    {
        animatedModel.SetActive(true);
        animator.SetBool("startGrab", true);
    }
}
