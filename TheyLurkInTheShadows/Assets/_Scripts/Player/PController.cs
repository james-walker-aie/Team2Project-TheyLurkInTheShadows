using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PController : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;
    public float movementSpeed;
    public float visibility;

    public bool hidden;
    private bool isWalking;
    private bool isRunning;

    public List<Transform> lights = new List<Transform>();

    public float attackTimer = 1f;

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


        playerMovementInput = Quaternion.Euler(0, 45, 0) * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerVelocity = playerMovementInput * movementSpeed;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.LookRotation(playerMovementInput);
        }



        if (Input.GetKey(KeyCode.W) == true || Input.GetKey(KeyCode.A) == true || Input.GetKey(KeyCode.S) == true || Input.GetKey(KeyCode.D) == true)
        {
            if (Input.GetKey(KeyCode.LeftShift) == true)
            {
                isRunning = true;
                isWalking = false;

            }
            else
            {
                isWalking = true;
                isRunning = false;
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }

        if (isWalking == true)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (isRunning == true)
        {
            playerAnimator.SetBool("IsRunning", true);
            movementSpeed = runSpeed;
        }
        else
        {
            playerAnimator.SetBool("IsRunning", false);
            movementSpeed = walkSpeed;
        }



        attackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) == true && attackTimer < 0)
        {
            attackTimer = 1f;
            //playerAnimator.applyRootMotion = true;
            playerAnimator.SetBool("IsPunching", true);


        }

        if (Input.GetKeyDown(KeyCode.Mouse1) == true && attackTimer < 0)
        {
            attackTimer = 1f;
            //playerAnimator.applyRootMotion = true;
            playerAnimator.SetBool("IsKicking", true);


        }

        if (attackTimer < 0)
        {
            playerAnimator.applyRootMotion = false;
            playerAnimator.SetBool("IsPunching", false);
            playerAnimator.SetBool("IsKicking", false);

        }

        if (lights.Count > 0)
        {
            Transform clsLight = null;
            float dis = 0;

            for (int i = 0; i < lights.Count; i++)
            {
                if (clsLight == null)
                {
                    clsLight = lights[i];
                    dis = Vector3.Distance(clsLight.transform.position, this.transform.position);
                }
                else
                {
                    float dist2 = Vector3.Distance(lights[i].transform.position, this.transform.position);
                    if (dist2 < dis)
                    {
                        clsLight = lights[i];
                        dis = dist2;
                    }
                }
            }

            float disToLight = Vector3.Distance(clsLight.transform.position, transform.position);
            Debug.Log(disToLight);
            float vis = visibility;
            visibility = 100 / disToLight * 4f;
        }
        else
        {
            visibility = 10;
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
