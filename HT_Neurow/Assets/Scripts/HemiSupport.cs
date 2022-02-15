using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemiSupport : MonoBehaviour
{
    public Animator animator;
    public GameObject animatedFemaleModel;
    public GameObject animatedMaleModel;
    public GameObject handModel; //opened hand model with hand binder


    private bool rightSide;


    private bool auxTrack = false; //signals beggining of wrist tracking

	public GameObject wrist;
    public GameObject wristAUX;
	public GameObject paddle;

	

    public float maxReach = 0.2f; //maximum delta z  

	private float initPos;
    private float oldPos;

    //Saves the max (or min positions, based on rowing phase)
    //so that rowing only occurs if the movement exceeds the max (min) position registered thus far
    private float maxMinAuxPos = 0f;

    private float minPos;
    private float maxPos;




    private float testDistance;
    private float delta = 0;
    private float deltaRot = 0;
    private bool forward = true;
    private bool forwardAux = true;

    //Determines if handModel should be hidden or not (post or pre collision)
    [HideInInspector] public bool hideHandAux = false;
    






    private void Start()
	{
        if (gameObject.name == "HemiZone_L") rightSide = false;
        else if (gameObject.name == "HemiZone_R") rightSide = true;

        animatedMaleModel.SetActive(false);
        //animatedFemaleModel.SetActive(false);

        forwardAux = true;

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

        wristAUX.transform.position = wrist.transform.position;
        Debug.Log(maxMinAuxPos);

        if(hideHandAux && handModel.activeSelf) handModel.SetActive(false);

        if (auxTrack){  // && oldPos >= initPos && oldPos <= initPos + maxReach) {   
            if (oldPos >= initPos && oldPos <= initPos + maxReach)
            {
                RotPaddle(oldPos, delta, forward, forwardAux);
            }
            else if (oldPos >= initPos + maxReach)
            {
                forwardAux = false;
                maxPos = initPos;
                maxMinAuxPos = initPos + maxReach;
            }
            else if (oldPos <= initPos)
            {
                forwardAux = true;
                minPos = initPos+maxReach;
                maxMinAuxPos = initPos;
            }
            //oldPos = wrist.transform.position.z;
            oldPos = CheckAxis();
        }

    }

    float CheckAxis()
    {
        if (GameManager.trackAxis == -1) return wristAUX.transform.localPosition.z;
        else if (GameManager.trackAxis == 1) return wristAUX.transform.localPosition.y;
        else if (GameManager.trackAxis == 0) return wristAUX.transform.localPosition.x;
        return -10;
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

        //float currentPos = wrist.transform.position.z;
        float currentPos = CheckAxis();
        float currentRotY = paddle.transform.localEulerAngles.y;
        float currentRotZ = paddle.transform.localEulerAngles.z;
        
        Quaternion target;

        delta = (currentPos - oldPos);
        deltaRot = (2*delta / maxReach) * 15f;  //dont know why i need the 2* but i'll take it


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



        if(forwardAux && currentPos > maxMinAuxPos)
        {
            //phase 1
            if (forward && currentPos <= initPos + maxReach / 2) 
            {
                if (rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
                else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

                paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
                Debug.Log("p1");
            }

            //phase 2
            else if (forward && currentPos > initPos + maxReach / 2 && currentPos > maxMinAuxPos) 
            {
                if (rightSide) target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ - deltaRot);
                else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

                paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
                Debug.Log("p2");   
            }
            maxMinAuxPos = currentPos;
        }

        if (!forwardAux && currentPos < maxMinAuxPos)
        {
            //phase 3
            if (!forward  && currentPos >= initPos + maxReach / 2) 
            {
                if (rightSide) target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ - deltaRot);
                else target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ + deltaRot);

                paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
                Debug.Log("p3");
            }

            //phase 4
            else if (!forward && currentPos < initPos + maxReach / 2) 
            {
                if (rightSide) target = Quaternion.Euler(-30f, currentRotY + (1.5f * deltaRot), currentRotZ - deltaRot);
                else target = Quaternion.Euler(-30f, currentRotY - (1.5f * deltaRot), currentRotZ + deltaRot);

                paddle.transform.localRotation = Quaternion.Slerp(paddle.transform.localRotation, target, 500f * Time.deltaTime);
                Debug.Log("p4");
            }

            maxMinAuxPos = currentPos;
        }
    }




    void OnTriggerEnter(Collider other) 
    {
                 
		if (other.gameObject.name == "Contact Fingerbone")  //HOW TO DISTINGUISH LEFT FROM RIGHT HAND??    -------------->          IMPLENT TRIGGER WITH ATTACHMENT HANDS Objects
        {
            hideHandAux = true;       

            if(GameManager.Gender == 1)
            {
                //animatedFemaleModel.SetActive(false);
                animatedMaleModel.SetActive(true);
            }
            else
            {
                //animatedFemaleModel.SetActive(true);
                animatedMaleModel.SetActive(false);
            }


            animator.SetBool("startGrab", true);

            //this will not be optimal, later maybe add a fixed starting position and the patient must reach that position to then start the movement
            initPos = CheckAxis();
            maxMinAuxPos = initPos;
			oldPos = initPos;

            minPos = initPos + maxReach;
            maxPos = initPos;

            //Debug.Log(initPos + maxReach);

            gameObject.GetComponent<SphereCollider>().enabled = false;  //prevent subsequent collisions
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            auxTrack = true;





        }
    }



    //void TestingGraspAnim()
    //{
    //    animatedModel.SetActive(true);
    //    animator.SetBool("startGrab", true);
    //}
}
