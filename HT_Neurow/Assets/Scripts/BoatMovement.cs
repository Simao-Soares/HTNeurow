using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
	Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log("oi");
		rb = GetComponent<Rigidbody>(); 
		rb.AddForce(0, 0, 1.0f, ForceMode.Impulse);
    }

}
