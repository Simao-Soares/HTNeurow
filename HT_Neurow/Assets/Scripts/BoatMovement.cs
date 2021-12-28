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


    bool cooldownActivated;
  
    float timeStarted;
    float timeSinceStarted;

    [HideInInspector] public bool turnLeft = false;
    [HideInInspector] public bool turnRight = false;

    public GameObject leftPaddle;
    public GameObject rightPaddle;

    public Animator L_rowAnimator; 
    public Animator R_rowAnimator;

    Quaternion rotateB;
    Quaternion rotateA;

    int countl = 0;
    int countr = 0;


    // Start is called before the first frame update
    void Start(){

        rb = GetComponent<Rigidbody>(); 
        rb.AddForce(0, 0, forwardForce, ForceMode.Impulse); //sketchy

        L_rowAnimator.SetFloat("Time", 0.7f/spinningTime ); //0.7 was trial and error
        R_rowAnimator.SetFloat("Time", 0.7f/spinningTime );

    }

    private void Update(){

		int cm = GameManager.ControlMethod; 

        //checks input + if cooldown is over + selected control method corresponds to the input 
        if ((Input.GetKey(KeyCode.LeftArrow) && cooldownActivated == false && cm == 1) ||(turnLeft == true && cooldownActivated == false && cm == -1)){
            Turn(true); //turn to the left side=true
            cooldownActivated = true;
            turnLeft = false;
			if(cm == 1) R_rowAnimator.SetBool("Turning", true);
        }

        else if ((Input.GetKey(KeyCode.RightArrow) && cooldownActivated == false && cm == 1) ||(turnRight == true && cooldownActivated == false && cm == -1)){
            Turn(false); //turn to the right side=false
            cooldownActivated = true;
            turnRight = false;
			if(cm == 1) L_rowAnimator.SetBool("Turning", true);
        }

        if(cooldownActivated) RunCooldownTimer(GameManager.ControlMethod);

        //Change movement direction
        rb.velocity=Vector3.zero;
        rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse); 
    }



    public void Turn(bool side){
        cooldownActivated = true;
        timeStarted = Time.time;
        rotateA.eulerAngles = this.transform.rotation.eulerAngles;

        //Debug.Log(rotateA.eulerAngles);

        if(side){
            countl++;
            rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
            //Debug.Log(rotateB.eulerAngles);
        }
        else if(side==false){
            countr++;
            rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
            //Debug.Log(rotateB.eulerAngles);
        }
    }


    void RunCooldownTimer(int auxControl){  //runs until rotation is complete and 
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
