using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    [Header("Bools")]
    public bool Hidden;
    public bool searchingSpot;
    public bool playerInSight;
    public bool inGuardSpot;
    public bool patrolling;
    public bool traveling;
    public bool Guard;
    public bool firstResponder;
    public bool alerted;
    public bool contin;
    public bool canAttack;
    bool cMove = false;
    bool timerReset = true;
    bool attacking;
    bool F;
    bool FL;
    bool FR;
    bool B;
    bool leap;
    public bool hitP = false;

    [Header("Floats")]
    public float health = 100;
    public float guardTime = 5;
    public float run;
    public float distance;
    float d = 2f;
    float timer;

    [Header("Int")]
    public int searchInt;
    public int patrolInt = 0;
    public int rotateSpeed = 20;
    public int AttackGroup = 0;
    public int num;

    [Header("GameObjects")]
    public GameObject alertSphere;
    public GameObject sight;
    public GameObject sightSync;
    public GameObject fr_Object;
    public GameObject Blood;
    GameObject c_ctrl;
    GameObject player;

    [Header("Vector3")]
    public Vector3 posTarget = Vector3.zero;
    Vector3 playerLastPos;
    Vector3 pos;
    Vector3 mPosL;
    Vector3 mPosR;
    Vector3 mPosLs;
    Vector3 mPosRs;
    Vector3 mPosB;
    Vector3 lastPos;

    [Header("NavMesh")]
    NavMeshAgent nav;
    NavMeshObstacle navObs;

    [Header("Lists")]
    public List<Transform> hidingSpots = new List<Transform>();
    public List<Transform> patrolSpots = new List<Transform>();
    public List<Transform> nearByEnemies = new List<Transform>();
    public List<GameObject> spottedDeadEnemies = new List<GameObject>();

    [Header("Transforms")]
    Transform clsPoint = null;

    [Header("Animator")]
    Animator anim;

    [Header("Vector2")]
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    public LayerMask layerMask = ~(1 << 11);



    public enum State
    {
        Guard,
        Patrol,
        AlertTime,
        Alert,
        investigating,
        Searching,
        Combat,
        Attack,
        Aggro,
        Chase,
        Hit,
        Dead

    }
    public State state;
    public State uClass { get { return state; } }


    // Use this for initialization
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        navObs = GetComponent<NavMeshObstacle>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        c_ctrl = GameObject.FindGameObjectWithTag("EC_ctrl");

    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (health <= 0)
        {
            state = State.Dead;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            state = State.Hit;
            if(timer <= 0)
            {
                timer = .5f;
            }
            
        }

        if(distance > 2)
        {
            leap = true;
        }
        else
        {
            leap = false;
        }

        if (leap)
        {
            anim.SetBool("Lunge", true);
        }
        else
        {
            anim.SetBool("Lunge", false);
        }
            

        if (state == State.Guard || state == State.Patrol)
        {
            if (playerInSight && !alerted)
            {
                timerReset = true;
                state = State.AlertTime;

            }


            nav.stoppingDistance = 0;
            transform.LookAt(null);
        }
        else
        {
            nav.stoppingDistance = 2;
        }

        if (state == State.Chase)
        {
            nav.speed = 3.5f;
        }

        if (playerInSight)
        {
            Rotation();
        }

        /*if (firstResponder)
        {
            if (!playerInSight)
            {
                firstResponder = false;
            }
        }*/

        if (fr_Object != null)
        {
            if (fr_Object.GetComponent<EnemyController>().playerInSight == false)
            {
                alerted = false;
            }
        }

        if(nav.velocity.sqrMagnitude > 0 && state != State.Guard && state != State.Combat)
        {
            anim.SetBool("Moving", true);
        }

        Vector3 worldDeltaPosition = posTarget - transform.position;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        velocity = smoothDeltaPosition;

        

        anim.SetFloat("Movement", velocity.y);
        anim.SetFloat("Horizontal", velocity.x);

        if(state != State.Combat)
        {
            anim.SetFloat("Idle", 0);
        }
        else
        {
            anim.SetFloat("Idle", 1);
        }

        switch (state)
        {
            case State.Guard:
                //stand still
                //Debug.Log("GUARDING");
                //ResetAnimationBools();
                //anim.SetBool("Guard", true);
                //ChangeRotation();
                
                if (guardTime > 0 || Guard)
                {
                    if(patrolSpots.Count >= 0 )
                    {
                        Quaternion guardR = Quaternion.Euler(transform.rotation.x, patrolSpots[patrolInt].rotation.eulerAngles.y, transform.rotation.z);
                        transform.rotation = Quaternion.Slerp(transform.rotation, guardR, rotateSpeed * Time.deltaTime);
                    }
                    
                }


                if (!Guard)
                {
                    nav.speed = 3f;
                    if (posTarget == Vector3.zero && patrolSpots.Count > 0)
                    {

                        state = State.Patrol;
                    }
                    if (timerReset == true)
                    {
                        timer = guardTime;
                        timerReset = false;
                    }

                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {

                        if (inGuardSpot)
                        {
                            if (patrolInt == 1)
                            {
                                patrolInt = 2;
                            }
                            else
                            {
                                patrolInt++;
                            }
                        }


                        state = State.Patrol;

                    }
                }


                break;
            case State.Patrol:
                //go from point to point
                run = 0;
                nav.speed = 3f;
                if (!inGuardSpot && !patrolling)
                {
                    float lowestDist = 10000f;
                    float nextDist;

                    for (int i = 0; i < patrolSpots.Count; i++)
                    {

                        if (clsPoint == null)
                        {
                            clsPoint = patrolSpots[0];
                            lowestDist = Vector3.Distance(patrolSpots[0].transform.position, transform.position);
                        }
                        nextDist = Vector3.Distance(patrolSpots[i].transform.position, transform.position);
                        if (nextDist < lowestDist)
                        {
                            lowestDist = nextDist;
                            clsPoint = patrolSpots[i];
                        }
                    }
                    

                    nav.SetDestination(clsPoint.transform.position);
                    posTarget = clsPoint.transform.position;
                }
                else
                {

                    patrolling = true;
                    if (patrolInt == patrolSpots.Count && !traveling)
                    {
                        traveling = true;
                        nav.SetDestination(patrolSpots[0].transform.position);
                        posTarget = patrolSpots[0].transform.position;
                        patrolInt = 0;
                    }
                    else if (!traveling)
                    {
                        nav.SetDestination(patrolSpots[patrolInt].transform.position);
                        posTarget = patrolSpots[patrolInt].transform.position;
                    }
                }
                float gdis = Vector3.Distance(posTarget, gameObject.transform.position);
                //Debug.Log("gdis: " + gdis);
                if (gdis < 2)
                {
                    //Debug.Log("Patrolling");
                    traveling = false;
                    timerReset = true;
                    state = State.Guard;
                }
                break;

            case State.AlertTime:
                int timesrun = 0;
                Debug.Log("Alerttime 1");
                if (playerInSight)
                {
                    
                    //detect if moved//
                    bool x1 = false;
                    bool z1 = false;
                    float xf1 = 0;
                    float zf1= 0;

                    if (transform.position.x > lastPos.x)
                    {
                        xf1 = transform.position.x - lastPos.x;
                        if (xf1 > 0.01)
                            x1 = true;
                    }
                    else
                    {
                        xf1 = lastPos.x - transform.position.x;
                        if (xf1 > 0.01)
                            x1 = true;
                    }


                    if (transform.position.z > lastPos.z)
                    {
                        zf1 = transform.position.z - lastPos.z;
                        if (zf1 > 0.01)
                            z1 = true;
                    }
                    else
                    {
                        zf1 = lastPos.z - transform.position.z;
                        if (zf1 > 0.01)
                            z1 = true;
                    }
                    //////////////////////////
                    if (!x1 && !z1)
                    {
                        anim.SetFloat("Idle", 0);
                        ResetAnimationBools();
                    }
                    else
                    {
                        ResetAnimationBools();
                        anim.SetBool("Moving", true);
                    }
                    
                    
                    Debug.Log("Alerttime 2");
                    Rotation();
                    if (timerReset == true)
                    {
                        Debug.Log("Alerttime 3");
                        timer = 100 / player.GetComponent<PController>().visibility * 0.5f;
                        timerReset = false;
                    }
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        Debug.Log("Alerttime 4");
                        timerReset = true;
                        state = State.Alert;
                    }
                }
                else
                {
                    Debug.Log("Alerttime 5");
                    timerReset = true;
                    state = State.Patrol;
                }
                Debug.Log("Alerttime 6");
                timesrun++;
                lastPos = transform.position;
                break;

            case State.Alert:
                //seen player, dead body, or alerted by other enemy
                if (alerted)
                {
                    state = State.Chase;
                }

                Debug.Log("Alert");
                alertSphere.SetActive(true);
                if (playerInSight)
                {
                    if (alertSphere.GetComponent<AlertSphere>().completed)
                    {
                        Debug.Log("nearby enemies1");
                        if (nearByEnemies.Count > 0)
                        {
                            Debug.Log("nearby enemies2");
                            bool fr = true;
                            for (int k = 0; k < nearByEnemies.Count; k++)
                            {
                                Debug.Log("nearby enemies3");
                                if (nearByEnemies[k].GetComponent<EnemyController>().firstResponder == true)
                                {
                                    Debug.Log("nearby enemies4");
                                    fr = false;
                                }
                                Debug.Log("nearby enemies5");
                            }
                            Debug.Log("nearby enemies6");
                            Debug.Log(fr);
                            firstResponder = fr;
                            Debug.Log(firstResponder);
                        }

                        if (firstResponder)
                        {
                            for (int j = 0; j < nearByEnemies.Count; j++)
                            {
                                if (nearByEnemies[j].GetComponent<EnemyController>().state == State.Searching || nearByEnemies[j].GetComponent<EnemyController>().state == State.Guard || nearByEnemies[j].GetComponent<EnemyController>().state == State.Patrol)
                                {
                                    if (fr_Object == null)
                                    {
                                        fr_Object = this.gameObject;
                                    }

                                    if (!nearByEnemies[j].GetComponent<EnemyController>().alerted)
                                    {
                                        nearByEnemies[j].GetComponent<EnemyController>().alerted = true;
                                    }

                                    nearByEnemies[j].GetComponent<EnemyController>().state = State.Alert;
                                }
                            }
                        }

                        state = State.Chase;
                    }



                }

                nav.SetDestination(posTarget);
                float dis = Vector3.Distance(posTarget, transform.position);
                if (dis < 4)
                {
                    if (!timerReset)
                    {
                        timerReset = true;
                        timer = 3;
                    }

                    timer -= Time.deltaTime;
                    if (timer <= 0 && !playerInSight && !alerted)
                    {
                        searchInt = 0;
                        timerReset = true;
                        state = State.Searching;
                    }

                }

                break;
            case State.Searching:
                //search near by hiding spots
                run = .5f;
                //Debug.Log("Searching");
                transform.LookAt(null);
                if (playerInSight)
                {
                    state = State.Chase;
                }

                if (alertSphere.active == false)
                {
                    alertSphere.SetActive(true);
                }
                if (hidingSpots.Count != 0)
                {
                    //Debug.Log("Search2");
                    nav.SetDestination(hidingSpots[searchInt].position);
                    float sDis = Vector3.Distance(hidingSpots[searchInt].position, transform.position);
                    if (sDis < 2)
                    {

                        if (timerReset == true)
                        {
                            timer = 5f;
                            timerReset = false;
                        }
                        searchingSpot = true;
                        timer -= Time.deltaTime;
                        if (timer <= 0)
                        {
                            if (searchInt != hidingSpots.Count - 1)
                            {

                                searchInt += 1;
                            }
                            else
                            {
                                state = State.Patrol;

                            }
                            timerReset = true;

                        }

                    }
                    else
                    {
                        searchingSpot = false;
                    }

                }
                else
                {

                    if (playerLastPos != Vector3.zero)
                    {
                        nav.SetDestination(playerLastPos);
                        float sDis = Vector3.Distance(playerLastPos, transform.position);
                        if (sDis < 3)
                        {
                            if (timerReset == true)
                            {
                                timer = 5f;
                                timerReset = false;

                                timer -= Time.deltaTime;
                                if (timer <= 0)
                                {
                                    state = State.Patrol;
                                }
                            }

                        }
                    }
                }

                break;

            case State.Chase:
                //follow player, if lose sight go to last known point and search
                run = 1;
                Debug.Log("Chase");
                if (playerInSight || alerted)
                {
                    Rotation();
                    playerLastPos = player.transform.position;
                    nav.SetDestination(player.transform.position);
                    float pDis = Vector3.Distance(player.transform.position, transform.position);
                    if (pDis < 10)
                        state = State.Combat;
                }
                else
                {
                    float lsDis = Vector3.Distance(playerLastPos, transform.position);
                    nav.SetDestination(playerLastPos);
                    if (lsDis < 4)
                        state = State.Searching;
                }

                break;

            case State.Aggro:
                //seen player, move for combat

                Debug.Log("Aggro");
                if (!playerInSight)
                {
                    state = State.Searching;
                }
                nav.SetDestination(player.transform.position);
                float aDis = Vector3.Distance(player.transform.position, transform.position);

                if (aDis < 7f)
                {
                    timerReset = true;
                    state = State.Combat;
                }

                else if (aDis >= 10)
                {
                    state = State.Chase;
                }


                break;

            case State.Combat:
                //fight
                //Debug.Log("nav: "+nav.velocity);
                
                nav.stoppingDistance = 3;
                Rotation();
                Raycasts();

                //set destination to pos (pos is determined through Raycasts())//
                ChoosePos();
                nav.SetDestination(pos);
                //Debug.Log("Pos: " + pos);
                /////////////////////////////////////////////////////////////////

                

                //detect if moved//
                bool x = false;
                bool z = false;
                float xf = 0;
                float zf = 0;

                if (transform.position.x > lastPos.x)
                {
                    xf = transform.position.x - lastPos.x;
                    if (xf > 0.01)
                        x = true;
                }
                else
                {
                    xf = lastPos.x - transform.position.x;
                    if (xf > 0.01)
                        x = true;
                }


                if (transform.position.z > lastPos.z)
                {
                    zf = transform.position.z - lastPos.z;
                    if (zf > 0.01)
                        z = true;
                }
                else
                {
                    zf = lastPos.z - transform.position.z;
                    if (zf > 0.01)
                        z = true;
                }
                //////////////////////////

                //set animation to combat movement//
                if (!x && !z)
                {
                    ResetAnimationBools();
                    anim.SetBool("cMove", false);
                    anim.SetFloat("Idle", 1);
                }
                else
                {
                    ResetAnimationBools();
                    if(distance < 5)
                    {
                        anim.SetBool("cMove", true);
                    }
                    else
                    {
                        anim.SetBool("Moving", true);
                    }
                    
                }
                

                ////////////////////////////////////////////////////

                
                
                //Go back to aggro state//
                if (distance > 10 && !alerted)
                {
                    nav.stoppingDistance = 0;
                    anim.SetFloat("Idle", 0);
                    state = State.Chase;
                }
                //////////////////////////

                //add enemy to enemy combat enemy list//
                if (hitP)
                {
                    if (!c_ctrl.GetComponent<EnemyCombatCtrl>().enemies.Contains(this.transform))
                        c_ctrl.GetComponent<EnemyCombatCtrl>().enemies.Add(this.transform);
                }
                
                ////////////////////////////////////////

                
                //Changes state to attack//
                if (canAttack)
                {
                    timer = -5;
                    state = State.Attack;
                }
                ////////////////////////////

                lastPos = this.transform.position;

                break;
            case State.Attack:
                Debug.Log("Attack");
                //Debug.Log("YAAAAAAA");
                attacking = true;
                canAttack = false;
                nav.enabled = false;
                anim.applyRootMotion = true;
                num = Random.Range(0, 4);
                
                if(timer <= 0)
                {
                    Debug.Log("Attack2");
                    switch (num)
                    {
                        case 0:
                            ResetAnimationBools();
                            anim.SetBool("Attack1", true);
                            timer = .8f;
                            break;
                        case 1:
                            ResetAnimationBools();
                            anim.SetBool("Attack2", true);
                            timer = .8f;
                            break;
                        case 2:
                            ResetAnimationBools();
                            anim.SetBool("Combo1", true);
                            timer = 2.5f;
                            break;
                        case 3:
                            ResetAnimationBools();
                            anim.SetBool("Combo2", true);
                            timer = 2.4f;
                            break;
                    }
                    Debug.Log("Attack3");
                }
                Debug.Log("Attack4");
                timer -= Time.deltaTime;
                if(timer <= 0 && timer > -5)
                {
                    nav.enabled = true;
                    anim.applyRootMotion = false;
                    state = State.Combat;
                }
                break;

            case State.Hit:
                timer -= Time.deltaTime;
                nav.enabled = false;
                //anim.applyRootMotion = true;
                if(Input.GetKeyDown(KeyCode.Mouse0) &&  anim.GetBool("Hit") == false)
                {
                    Blood.GetComponent<ParticleSystem>().Play();
                    //gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 100*Time.deltaTime);
                    //transform.Translate(transform.forward * 40 * Time.deltaTime);
                }
                    

                ResetAnimationBools();
                if(timer > 0)
                    anim.SetBool("Hit", true);
                if(timer <= 0)
                {
                    anim.SetBool("Hit", false);
                    nav.enabled = true;
                    state = State.Combat;
                }
                    

                break;

            case State.Dead:
                //dead
                sight.SetActive(false);
                //GetComponent<CapsuleCollider>().isTrigger = true;


                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bush")
        {
            if (state == State.Dead)
                Hidden = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bush")
        {
            if (state == State.Dead)
                Hidden = false;
        }
    }

    void ResetAnimationBools()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Combo1", false);
        anim.SetBool("Combo2", false);
        anim.SetBool("Combat", false);
        anim.SetBool("Moving", false);
        anim.SetBool("Guard", false);
        anim.SetBool("cMove", false);
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

        Debug.Log(Vector3.forward);
        Vector3 sp = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        //forward
        if (Physics.Raycast(sp, transform.forward, out hitFoward, d + .05f,layerMask))
        {
            Debug.Log("aaaaaa");
            if (hitFoward.collider.tag == "Player")
            {
                hitP = true;
               
                F = true;
                //nav.stoppingDistance = 3f;
                Debug.Log("yyyyyyy" + gameObject.name);
                //pos = player.transform.position;
                

            }
            else
            if (hitFoward.collider.tag == "Enemy")
            {
                hitP = false;
                F = true;
                //nav.stoppingDistance = 0;
                Debug.Log("GGGG");

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

        if (Physics.Raycast(sp, -transform.forward, out hitBack, d / 2, layerMask))
        {

            if (hitBack.collider.tag == "Enemy")
            {
                B = true;

            }

        }
        else
        {
            B = false;
            if (distance < 3)
            {
                // pos = mPosB;
            }
        }
        Debug.DrawRay(sp, -transform.forward * (d / 2), Color.blue);
        //forwardleft

        if (Physics.Raycast(sp, ((transform.forward + -transform.right)), out hitFLeft, d, layerMask))
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
        Debug.DrawRay(sp, ((transform.forward + -transform.right)) * d, Color.blue);
        //forwardright
        if (Physics.Raycast(sp, ((transform.forward + transform.right)), out hitFRight, d, layerMask))
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
        if (Physics.Raycast(sp + ((transform.forward + -transform.right) * 2), Vector3.down * (d * 2), out hitL, d,layerMask))
        {
            Debug.Log("dddddddd");

            mPosL = hitL.point;


        }
        Debug.DrawRay(sp + ((transform.forward + -transform.right) * 2), Vector3.down * (d * 2), Color.blue);
        //right
        if (Physics.Raycast(sp + ((transform.forward + transform.right)), Vector3.down, out hitR, d, layerMask))
        {
            Debug.Log("Rrrrrrrr");
            mPosR = hitR.point;

        }
        Debug.DrawRay(sp + ((transform.forward + transform.right) * 2), Vector3.down * (d * 2), Color.yellow);

        //Back
        if (Physics.Raycast(sp + -transform.forward, Vector3.down, out hitB, d, layerMask))
        {
            //Debug.Log("Rrrrrrrr");
            mPosB = hitB.point;

        }
        Debug.DrawRay(sp + -transform.forward, Vector3.down * (d), Color.yellow);

    }

    void ChoosePos()
    {
        if (distance < 2)
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
                    nav.stoppingDistance = 2;
                    pos = player.transform.position;
                }
            }

        }
    }

   

}
