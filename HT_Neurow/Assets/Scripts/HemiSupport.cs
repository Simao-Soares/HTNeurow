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
    }



    void OnTriggerEnter(Collider other) 
    {
                               //no idea if this is it     
        if (gameObject.name == "Interaction Hand (Left)") 
        {
            openLeft.SetActive(false);   
            animatedLeft.SetActive(true);
            leftAnimator.SetBool("startGrab", true); 
            animatedLeft.SetActive(false);
            openLeft.SetActive(false);
            closedLeft.SetActive(true);


        }
                                //no idea if this is it
        if (gameObject.name == "Interaction Hand (Right)") 
        {

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
