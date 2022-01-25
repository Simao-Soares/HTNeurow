using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    Rigidbody rb;
    //public GameManager gm; //ControlMethod: 1 -> BCI   -1 -> HT
	

    public float rotAngle;  //angle of rotation with 1 press
    public float spinningTime; //time it takes takes to rotate

    public float forwardForce;

    [HideInInspector] public bool selfCorrection;


    [HideInInspector] public bool cooldownActivated;
  
    float timeStarted;
    float timeSinceStarted;

    [HideInInspector] public bool turnLeft = false;
    [HideInInspector] public bool turnRight = false;

    public GameObject leftPaddle;
    public GameObject rightPaddle;

    public Animator L_rowAnimator; 
    public Animator R_rowAnimator;

    Quaternion rotateB;
    [HideInInspector] public Quaternion rotateA;

    [HideInInspector] public bool auxTurn = false;

    //public int countl = 0;
    //public int countr = 0;


    //-------------------------
    [HideInInspector] public bool debugAux = false;
    //-------------------------


    // Start is called before the first frame update
    void Start(){

        rb = GetComponent<Rigidbody>(); 
        //rb.AddForce(0, 0, forwardForce, ForceMode.Impulse); //sketchy

        L_rowAnimator.SetFloat("Time", 0.7f/spinningTime ); //0.7 was trial and error
        R_rowAnimator.SetFloat("Time", 0.7f/spinningTime );

        selfCorrection = false;

    }

    private void FixedUpdate(){

		int cm = GameManager.ControlMethod;

        var boatPos = transform.position;
        var frontOfBoat = boatPos + 5 * transform.forward;
        var boatOrientation = frontOfBoat - boatPos;


        //if (auxTurn) StartCoroutine(Turning(1f, rotateB));
        //if(boatOrientation!=rotateB)
        



        //checks input + if cooldown is over + selected control method corresponds to the input 
        if ((Input.GetKey(KeyCode.RightArrow) && cooldownActivated == false && cm == 1)||
            (turnLeft == true && cooldownActivated == false && cm == -1))                                 
        {
            if(GameManager.invertTurn) Turn(true); //turn to the left side=true
            else Turn(false);

            cooldownActivated = true;
            turnLeft = false;
			if(cm == 1) R_rowAnimator.SetBool("Turning", true);
        }

        else if ((Input.GetKey(KeyCode.LeftArrow) && cooldownActivated == false && cm == 1) ||
                 (turnRight == true && cooldownActivated == false && cm == -1))  
        {
            if (GameManager.invertTurn) Turn(false); //turn to the right side=false
            else Turn(true);

            cooldownActivated = true;
            turnRight = false;
			if(cm == 1) L_rowAnimator.SetBool("Turning", true);
        }

        if(cooldownActivated) RunCooldownTimer();


        //Change movement direction
        rb.velocity = Vector3.zero;
        if(!selfCorrection) rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
    }



    public void Turn(bool side){
        cooldownActivated = true;
        timeStarted = Time.time;
        rotateA.eulerAngles = transform.rotation.eulerAngles;         

        if(side){
            //countl++;
            //rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
           
            rotateB = Quaternion.Euler(0, rotateA.eulerAngles.y - rotAngle, 0); //(rotAngle * (countr - countl))

            //auxTurn = true;

            

        }
        else if(side==false){
            //countr++;
            //rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);

            rotateB = Quaternion.Euler(0, rotateA.eulerAngles.y + rotAngle, 0);

            //auxTurn = true;

            


        }
    }


    IEnumerator Turning(float speed, Quaternion rotation)
    {
        Quaternion current = transform.rotation;
        transform.localRotation = Quaternion.Lerp(current, rotation, speed * Time.deltaTime);
        yield return null;
    }


    void RunCooldownTimer(){  //runs until rotation is complete 
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / spinningTime;

        transform.rotation = Quaternion.Lerp(rotateA, rotateB, percentageComplete);

        if (percentageComplete >= 1)
        {
            cooldownActivated = false;
            L_rowAnimator.SetBool("Turning", false);
            R_rowAnimator.SetBool("Turning", false);
        }
    }


}
