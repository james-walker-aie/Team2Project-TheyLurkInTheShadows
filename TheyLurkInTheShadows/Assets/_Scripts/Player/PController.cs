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
    public float attackTimer = 2f;
    public float health = 100;

    public bool hidden;
    public bool isBlocking;
    private bool isWalking;
    private bool isRunning;
    private bool canBackstab;

    public List<Transform> lights = new List<Transform>();

    private Rigidbody playerRB;

    private Vector3 playerMovementInput;
    private Vector3 playerVelocity;
    public Vector3 playerToMouse;

    public Animator playerAnimator;

    public GameObject attackCollider;
    public GameObject backstabCollider;

    public Transform sightSync;

    private float secretTimer = 1;
    private float secretCounter;
    private float secretPlayTime = 0;
    public AudioClip secretSound;
    private AudioSource audioSource;
    public GameObject secretLights;
    private float secretSpinSpeed = 10f;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        playerMovementInput = Quaternion.Euler(0, 45, 0) * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerVelocity = playerMovementInput * movementSpeed;


        playerAnimator.SetFloat("MovementBlendX", ((Quaternion.Euler(0, 135, 0) * playerRB.rotation) * new Vector3(-Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))).normalized.x);
        playerAnimator.SetFloat("MovementBlendY", ((Quaternion.Euler(0, 315, 0) * playerRB.rotation) * new Vector3(-Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))).normalized.z);

        PlayerRotation();

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
            playerAnimator.SetBool("IsAttacking", true);

            if (attackTimer > 0)
            {
                attackCollider.SetActive(true);
            }


        }

        if (Input.GetKeyDown(KeyCode.E) == true && attackTimer < 0)
        {

            if (canBackstab == true)
            {
                attackTimer = 2.5f;
                playerAnimator.SetBool("IsBackstabbing", true);

                if (attackTimer > 0)
                {
                    backstabCollider.SetActive(true);
                }

            }

        }

        if (attackTimer < 0)
        {
            playerAnimator.applyRootMotion = false;
            playerAnimator.SetBool("IsAttacking", false);
            playerAnimator.SetBool("IsBackstabbing", false);

        }

        if (attackTimer < 0)
        {
            attackCollider.SetActive(false);
            backstabCollider.SetActive(false);
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

        #region SECRET

        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            secretTimer = 1;
            secretCounter += 1;

            if (secretCounter >= 5)
            {
                secretPlayTime = 30;
                secretCounter = 0;
                playerAnimator.SetLayerWeight(2, 2);
                audioSource.PlayOneShot(secretSound);
                secretLights.SetActive(true);
            }

        }

        if (secretTimer < 0)
        {
            secretCounter = 0;
        }

        secretTimer -= Time.deltaTime;
        secretPlayTime -= Time.deltaTime;

        if (secretPlayTime < 0)
        {
            playerAnimator.SetLayerWeight(2, 0);
            audioSource.Stop();
            secretLights.SetActive(false);
        }
        #endregion

        secretLights.transform.Rotate(Vector3.up, secretSpinSpeed * Time.deltaTime);


    }

    private void FixedUpdate()
    {
        playerVelocity.y = playerRB.velocity.y;
        playerRB.velocity = playerVelocity;

    }

    void PlayerRotation()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit))
        {
            playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRB.MoveRotation(newRotation);

        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bush")
        {
            hidden = true;
        }

        if (other.gameObject.tag == "BackStab")
        {
            canBackstab = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BackStab")
        {
            hidden = false;
        }

        if (other.gameObject.tag == "Enemy")
        {
            canBackstab = false;
        }
    }
}
