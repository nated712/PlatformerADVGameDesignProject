using System;
using JetBrains.Annotations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //reference charactercontroller from inspector    
    public CharacterController controller;
    //movespeed
    [SerializeField] public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 6f;
    //stores transform of groundCheck object in inspector
    public Transform groundCheck;
    //size of sphere to check if grounded
    public float groundDistance = 0.4f;
    //store layer that we will count as ground
    public LayerMask groundMask;
    bool isGrounded;
    //for gravity calculation
    Vector3 velocity;



    // Update is called once per frame
    void Update()
    {
        //check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask.value);

        //reset velocity if grounded
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        //set up axis for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //jump logic
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //create vector storing movement based on where player is looking
        Vector3 move = transform.right * x + transform.forward * z;
        //Moves character controller with created vector
        controller.Move(move * speed * Time.deltaTime);

        //gravity handling
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
