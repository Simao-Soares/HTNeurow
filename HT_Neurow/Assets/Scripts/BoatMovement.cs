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

    public Animator rowAnim; // ver o video do mpto brackeys
    




    Quaternion rotateB;
    Quaternion rotateA;

    Quaternion paddleA;
    Quaternion paddleB;
    Quaternion paddleC;
    Quaternion paddleD;


    int countl = 0;
    int countr = 0;


    // Start is called before the first frame update
    void Start(){

        Debug.Log(GameManager.ControlMethod);
        rb = GetComponent<Rigidbody>(); 
        rb.AddForce(0, 0, forwardForce, ForceMode.Impulse); //sketchy

    }

    private void Update(){

        //checks input + if cooldown is over + selected ontrol method corresponds to the input 
        if ((Input.GetKey(KeyCode.LeftArrow) && cooldownActivated == false && GameManager.ControlMethod == 1) ||(turnLeft == true && cooldownActivated == false && GameManager.ControlMethod == -1)){
            Turn(true); //turn to the left side=true
            cooldownActivated = true;
            turnLeft = false;
            
            
        }

        else if ((Input.GetKey(KeyCode.RightArrow) && cooldownActivated == false && GameManager.ControlMethod == 1) ||(turnRight == true && cooldownActivated == false && GameManager.ControlMethod == -1)){
            Turn(false); //turn to the right side=false
            cooldownActivated = true;
            turnRight = false;
        }

        if(cooldownActivated) RunCooldownTimer(GameManager.ControlMethod);

        //Change movement direction
        rb.velocity=Vector3.zero;
        rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse); 
    }



    public void Turn(bool side){
        cooldownActivated = true;
        timeStarted = Time.time;
        //Transform thisTransform = gameObject.transform;
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
      
        float initY = 0;    //goes to -15 then 15 then back to 0
        float initZ = -65;  //goes to -65 and back

        //paddleA.eulerAngles = new Vector3(0, initY, initZ);
        //paddleB.eulerAngles = new Vector3(0, -15f, -77.5f);
        //paddleC.eulerAngles = new Vector3(0, 0, -90);
        //paddleD.eulerAngles = new Vector3(0, 15f, -77.5f);

        transform.rotation = Quaternion.Lerp(rotateA, rotateB, percentageComplete);

        //need to call this inside the if in Update and pass the side as an argument!!
        //leftPaddle.transform.rotation = Quaternion.Lerp(paddleA, paddleB, percentageComplete);
        //leftPaddle.transform.rotation = Quaternion.Lerp(paddleB, paddleC, percentageComplete);
        //leftPaddle.transform.rotation = Quaternion.Lerp(paddleC, paddleD, percentageComplete);
        //leftPaddle.transform.rotation = Quaternion.Lerp(paddleD, paddleA, percentageComplete);


        



        if (percentageComplete >= 1)
        {
            cooldownActivated = false;
        }
    }
}
