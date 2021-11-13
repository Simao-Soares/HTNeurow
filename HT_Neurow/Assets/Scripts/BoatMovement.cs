using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
	Rigidbody rb;

  public float rotAngle;  //angle of rotation with 1 press
  public float spinningTime; //time it takes takes to rotate

  bool cooldownActivated;
  
  float timeStarted;
  float timeSinceStarted;

   

  Quaternion rotateB;
  Quaternion rotateA;

  int countl = 0;
  int countr = 0;


    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody>(); 
      rb.AddForce(0, 0, 1.0f, ForceMode.Impulse);
    }

    private void FixedUpdate() {
      if (Input.GetKeyDown(KeyCode.LeftArrow) && cooldownActivated == false)
      {
        Turn(true); //turn to the left side=true
        cooldownActivated = true;
      }
      else if (Input.GetKeyDown(KeyCode.RightArrow)&& cooldownActivated == false)
      {
        Turn(false); //turn to the right side=false
        cooldownActivated = true;
      }
      if(cooldownActivated) RunCooldownTimer();
    }

    public void Turn(bool side)
    {
      spinningTime = 1;
      cooldownActivated = true;
      timeStarted = Time.time;
      

      Transform thisTransform = gameObject.transform;

      rotateA.eulerAngles = this.transform.rotation.eulerAngles;

      Debug.Log(rotateA.eulerAngles);

      if(side){
        countl++;
        rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
        Debug.Log(rotateB.eulerAngles);
      }
      else if(side==false) {
        countr++;
        rotateB.eulerAngles = rotateA * new Vector3(0, rotAngle * (countr-countl), 0);
        Debug.Log(rotateB.eulerAngles);
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
