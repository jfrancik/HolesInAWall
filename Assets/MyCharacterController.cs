using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour {

    public float turnSpeed = 120;
    public float walkSpeed = 5;
    public float jumpSpeed = 10;
    public float gravity = -20;

    CharacterController characterController;
    Vector3 move = Vector3.zero;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        float forward = Input.GetAxis("Vertical");
        float stride = Input.GetAxis("Horizontal");
        float turn =  Input.GetAxis("Mouse X");
        float tilt =  Input.GetAxis("Mouse Y");
        float jump = Input.GetAxis("Jump");

        if (characterController.isGrounded)
            move = (transform.forward * forward + transform.right * stride) * walkSpeed + transform.up * jump * jumpSpeed;
        else
            move += transform.up * gravity * Time.deltaTime;

        characterController.Move(move * Time.deltaTime);

        transform.Rotate(new Vector3(0, turn * turnSpeed * Time.deltaTime, 0));
        transform.GetChild(0).Rotate(new Vector3(tilt * turnSpeed * Time.deltaTime, 0, 0));
    }
}
