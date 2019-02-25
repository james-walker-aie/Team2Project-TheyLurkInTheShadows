using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PController : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;
    public float movementSpeed;


    public bool hidden;
    private bool isSprinting;
    public bool isMoving;

    private Rigidbody playerRB;

    private Vector3 playerMovementInput;
    private Vector3 playerVelocity;

    public Animator playerAnimator;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) == true)
        {
            movementSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) == true)
        {
            movementSpeed = walkSpeed;
        }   

        playerMovementInput = Quaternion.Euler(0, 45, 0) * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerVelocity = playerMovementInput * movementSpeed;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.LookRotation(playerMovementInput);
        }

        if (movementSpeed == walkSpeed)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (movementSpeed == runSpeed)
        {
            playerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            playerAnimator.SetBool("IsRunning", false);
        }

        if (playerRB.velocity.magnitude == 0)
        {
            playerAnimator.SetBool("IsWalking", false);
            playerAnimator.SetBool("IsRunning", false);
        }

    }

    private void FixedUpdate()
    {

        playerRB.velocity = playerVelocity;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bush")
        {
            hidden = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bush")
        {
            hidden = false;
        }
    }
}
