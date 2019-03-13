using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombatTest : MonoBehaviour
{

    GameObject player;
    
    NavMeshAgent nav;
    public float distance;
    public float d = 2f;
    Vector3 pos;
    Vector3 mPosL;
    Vector3 mPosR;
    Vector3 mPosLs;
    Vector3 mPosRs;
    Vector3 mPosB;
    bool F;
    bool FL;
    bool FR;
    bool B;
    public bool hitP = false;
    public bool strafe = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        pos = player.transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        //fight
        distance = Vector3.Distance(player.transform.position, transform.position);
        Rotation();
        Raycasts();
        //Debug.Log("Distance: " + distance);
        if(distance < 2)
        {
            if (!B)
            {
                nav.stoppingDistance = 0;
                pos = mPosB;
            }
                
        }
        else
        {
            if (!hitP)
            {
                nav.stoppingDistance = 0;
                if (F)
                {
                    if (FR && FL)
                    {
                        pos = mPosR;
                    }
                    else
                    if (!FR && FL)
                    {
                        pos = mPosL;
                    }
                    else
                    if (FR && !FL)
                    {
                        pos = mPosR;
                    }


                }
                else
                {
                    nav.stoppingDistance = 3;
                    pos = player.transform.position;
                }
            }

        }
        

        nav.SetDestination(pos);

    }

    void Rotation()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    void Raycasts()
    {

        //Detect Enemies/////
        RaycastHit hitFoward;
        RaycastHit hitFRight;
        RaycastHit hitFLeft;
        RaycastHit hitBack;

        //Debug.Log(Vector3.forward);
        Vector3 sp = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        //forward
        if (Physics.Raycast(sp, transform.forward, out hitFoward, d+.05f))
        {
            //Debug.Log("aaaaaa");
            if (hitFoward.collider.tag == "Player")
            {
                hitP = true;
                if (!strafe)
                {
                    F = true;
                    //nav.stoppingDistance = 3f;
                    //Debug.Log("yyyyyyy" + gameObject.name);
                    //pos = player.transform.position;
                }
                
            }else 
            if (hitFoward.collider.tag == "Enemy")
            {
                hitP = false;
                F = true;
                //nav.stoppingDistance = 0;
                //Debug.Log("GGGG");
                
                //pos = mPosR;
            }

        }
        else
        {
            //nav.stoppingDistance = 0;
            F = false;
            hitP = false;
            //pos = player.transform.position;
        }
        Debug.DrawRay(sp, transform.forward * (d + .05f), Color.red);

        //Back//

        if (Physics.Raycast(sp, -transform.forward, out hitBack, d /2))
        {
            
            if (hitBack.collider.tag == "Enemy")
            {
                B = true;
                
            }

        }
        else
        {
            B = false;
            if(distance < 3)
            {
               // pos = mPosB;
            }
        }
        Debug.DrawRay(sp, -transform.forward * (d/2), Color.blue);
        //forwardleft

        if (Physics.Raycast(sp, ((transform.forward + -transform.right) ), out hitFLeft, d))
        {
            if (hitFLeft.collider.tag == "Enemy" && !hitP)
            {
                //nav.stoppingDistance = 0;
                //pos = mPosR;
                FL = true;
            }
            else
            if (hitFLeft.collider.tag == "Enemy" && hitP)
            {
                //nav.stoppingDistance = 0;
                //pos = mPosR/2;
                FL = true;
            }

        }
        else
        {
            //pos = player.transform.position;
            FL = false;
        }
        Debug.DrawRay(sp, ((transform.forward + -transform.right) ) * d, Color.blue);
        //forwardright
        if (Physics.Raycast(sp, ((transform.forward + transform.right)), out hitFRight, d))
        {
            if (hitFRight.collider.tag == "Enemy" && !hitP)
            {
                //nav.stoppingDistance = 0;
                //pos = mPosL;
                FR = true;
            }
            else
            if (hitFRight.collider.tag == "Enemy" && hitP)
            {
                //nav.stoppingDistance = 0;
                //pos = mPosL / 2;
                FR = true;
            }

        }
        else
        {
            //pos = player.transform.position;
            FR = false;
        }
        Debug.DrawRay(sp, ((transform.forward + transform.right)) * d, Color.yellow);

        
        //move points

        RaycastHit hitL;
        RaycastHit hitR;
        RaycastHit hitB;

        //Left
        if (Physics.Raycast(sp+((transform.forward + -transform.right)*2),Vector3.down * (d*2), out hitL, d,LayerMask.GetMask("Ground")))
        {
            //Debug.Log("dddddddd");
            
            mPosL = hitL.point;
            
            
        }
        Debug.DrawRay(sp + ((transform.forward + -transform.right) * 2), Vector3.down * (d * 2), Color.blue);
        //right
        if (Physics.Raycast(sp + ((transform.forward + transform.right) ),Vector3.down , out hitR, d, LayerMask.GetMask("Ground")))
        {
            //Debug.Log("Rrrrrrrr");
            mPosR = hitR.point;
            
        }
        Debug.DrawRay(sp + ((transform.forward + transform.right) * 2), Vector3.down * (d * 2), Color.yellow);

        //Back
        if (Physics.Raycast(sp + -transform.forward, Vector3.down, out hitB, d, LayerMask.GetMask("Ground")))
        {
            //Debug.Log("Rrrrrrrr");
            mPosB = hitB.point;

        }
        Debug.DrawRay(sp + -transform.forward, Vector3.down * (d), Color.yellow);

    }


}
   
    
    
