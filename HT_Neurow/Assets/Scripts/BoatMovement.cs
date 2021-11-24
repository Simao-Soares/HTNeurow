using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
	Rigidbody rb;
	public int ControlMethod = 1; // 1 -> BCI   -1 -> HT

  public float rotAngle;  //angle of rotation with 1 press
  public float spinningTime; //time it takes takes to rotate

  public float forwardForce; 


  bool cooldownActivated;
  
  float timeStarted;
  float timeSinceStarted;

  public bool turnLeft = false;
  public bool turnRight = false;



   

  Quaternion rotateB;
  Quaternion rotateA;

  int countl = 0;
  int countr = 0;


    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody>(); 
      rb.AddForce(0, 0, forwardForce, ForceMode.Impulse); //sketchy
    }

    private void Update() {
      //checks input + if cooldown is over + selected ontrol method corresponds to the input 
      if ((Input.GetKey(KeyCode.LeftArrow) && cooldownActivated == false && ControlMethod == 1)||(turnLeft == true && cooldownActivated == false && ControlMethod == -1))
        {
          Turn(true); //turn to the left side=true
          cooldownActivated = true;
          turnLeft = false;
        }
      else if ((Input.GetKey(KeyCode.RightArrow) && cooldownActivated == false && ControlMethod == 1)||(turnRight == true && cooldownActivated == false && ControlMethod == -1))
        {
          Turn(false); //turn to the right side=false
          cooldownActivated = true;
          turnRight = false;
        }
        if(cooldownActivated) RunCooldownTimer();

      //Change movement direction
      rb.velocity=Vector3.zero;
      rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse); 
		}



    public void Turn(bool side)
    {
      cooldownActivated = true;
      timeStarted = Time.time;
      

      Transform thisTransform = gameObject.transform;

      rotateA.eulerAngles = this.transform.rotation.eulerAngles;

      //Debug.Log(rotateA.eulerAngles);

      if(side){
        countl++;
        rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
        //Debug.Log(rotateB.eulerAngles);
      }
      else if(side==false) {
        countr++;
        rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
        //Debug.Log(rotateB.eulerAngles);
      }
    }

    void RunCooldownTimer()
    {
      float timeSinceStarted = Time.time - timeStarted;
      float percentageComplete = timeSinceStarted / spinningTime;

      transform.rotation = Quaternion.Lerp(rotateA, rotateB, percentageComplete);

      if (percentageComplete >= 1)
      {
          cooldownActivated = false;
      }
    }
}
