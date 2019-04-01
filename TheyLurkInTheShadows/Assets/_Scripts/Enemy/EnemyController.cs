using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool hit;

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
    public float timer;

    [Header("Int")]
    public int searchInt;
    public int patrolInt = 0;
    public int rotateSpeed = 20;
    public int AttackGroup = 0;
    public int num;
    int lastAttack;

    [Header("GameObjects")]
    public GameObject alertSphere;
    public GameObject sight;
    public GameObject sightSync;
    public GameObject fr_Object;
    public GameObject Blood;
    public GameObject Special;
    public GameObject Smash;
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

    
    [SerializeField] Image playerHealthIcon;
    [SerializeField] bool isPlayer = false;
    float playerCurrentHealth;
    float playerStartingHealth = 100f;

    public enum State
    {
        Guard,
        Patrol,
        AlertTime,
        Alert,
        investigating,
        Searching,
        Combat,
        BeingAttacked,
        Attack,
        Block,
        Blocked,
        Roll,
        Aggro,
        Chase,
        LastPos,
        Hit,
        Dead

    }
    public State state;
    public State uClass { get { return state; } }

    public enum Type
    {
        Basic,
        Heavy,
        Ninja,
        Ranged

    }
    public Type Class;
    public Type rClass { get { return Class; } }


    // Use this for initialization
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        navObs = GetComponent<NavMeshObstacle>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        c_ctrl = GameObject.FindGameObjectWithTag("EC_ctrl");

        playerCurrentHealth = playerStartingHealth;
        PlayerHealthUI();
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (health <= 0)
        {
            state = State.Dead;
        }

        if (distance > 2)
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
            if (Class == Type.Basic)
            {
                nav.speed = 3.5f;
            }
            else
            if (Class == Type.Heavy)
            {
                nav.speed = 3.5f / 2;
            }

        }

        if (playerInSight)
        {
            Rotation();
        }


        /*if(nav.velocity.sqrMagnitude > 0 && state != State.Guard && state != State.Combat)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }*/
        IfMoving(lastPos);

        Vector3 worldDeltaPosition = posTarget - transform.position;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy).normalized;

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        velocity = smoothDeltaPosition;



        anim.SetFloat("Movement", velocity.y);
        anim.SetFloat("Horizontal", velocity.x);

        if (state != State.Combat)
        {
            anim.SetFloat("Idle", 0);
        }
        else
        {
            anim.SetFloat("Idle", 1);
        }

        if (alerted && playerInSight)
            alerted = false;

        if (alerted)
        {
            if (distance > 17)
            {
                alerted = false;
            }
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
                    if (patrolSpots.Count >= 0)
                    {
                        Quaternion guardR = Quaternion.Euler(transform.rotation.x, patrolSpots[patrolInt].rotation.eulerAngles.y, transform.rotation.z);
                        transform.rotation = Quaternion.Slerp(transform.rotation, guardR, rotateSpeed * Time.deltaTime);
                    }

                }


                if (!Guard)
                {
                    if (Class == Type.Basic)
                    {
                        nav.speed = 3f;
                    }
                    else
                    if (Class == Type.Heavy)
                    {
                        nav.speed = 3 / 1.2f;
                    }

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
                if (Class == Type.Basic)
                {
                    nav.speed = 3f;
                }
                else
                if (Class == Type.Heavy)
                {
                    nav.speed = 3 / 1.2f;
                }
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

                if (playerInSight)
                {

                    //detect if moved//
                    bool x1 = false;
                    bool z1 = false;
                    float xf1 = 0;
                    float zf1 = 0;

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



                    Rotation();
                    if (timerReset == true)
                    {

                        timer = 100 / player.GetComponent<PController>().visibility * 0.5f;
                        timerReset = false;
                    }
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {

                        timerReset = true;
                        state = State.Alert;
                    }
                }
                else
                {

                    timerReset = true;
                    state = State.Patrol;
                }

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

                        AlertNearbyEnemies();
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
                if (searchInt > hidingSpots.Count)
                {


                    state = State.Patrol;
                }
                transform.LookAt(null);
                IfMoving(lastPos);
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

                    if (searchInt < hidingSpots.Count)
                    {
                        nav.SetDestination(hidingSpots[searchInt].position);
                    }
                    else
                    {
                        state = State.Patrol;
                    }


                    float sDis = Vector3.Distance(hidingSpots[searchInt].position, transform.position);

                    if (sDis < 3)
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
                            if (searchInt <= hidingSpots.Count)
                            {

                                searchInt++;
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
                lastPos = this.transform.position;
                break;

            case State.LastPos:
                nav.enabled = true;
                if (alerted || playerInSight)
                {
                    state = State.Chase;
                }
                nav.stoppingDistance = 0;
                nav.SetDestination(pos);
                float LPdis = Vector3.Distance(pos, this.transform.position);
                if (LPdis <= 1)
                    state = State.Searching;

                break;

            case State.Chase:
                //follow player, if lose sight go to last known point and search
                run = 1;
                Debug.Log("Chase");
                AlertNearbyEnemies();
                if (playerInSight || alerted)
                {
                    Rotation();

                    nav.SetDestination(player.transform.position);

                    if (distance < 10)
                        state = State.Combat;
                }
                else
                if (distance > 10)
                {
                    lastPos = player.transform.position;
                    pos = lastPos;
                    state = State.LastPos;
                }

                break;

            case State.Aggro:
                //seen player, move for combat

                Debug.Log("Aggro");
                AlertNearbyEnemies();
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
                AlertNearbyEnemies();
                nav.stoppingDistance = 3;
                Rotation();
                Raycasts();

                //set destination to pos (pos is determined through Raycasts())//
                ChoosePos();
                nav.SetDestination(pos);
                //Debug.Log("Pos: " + pos);
                /////////////////////////////////////////////////////////////////

                IfMoving(lastPos);

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

            case State.BeingAttacked:

                float chance;
                chance = Random.Range(0f, 1f);
                if (chance < 0.4f)
                {
                    timer = .5f;
                    state = State.Block;
                }
                else
                if (chance > 0.4f && chance < 0.7f)
                {
                    timer = 1;
                    state = State.Roll;
                }
                else
                {
                    timer = 1;
                    state = State.Hit;
                }

                break;

            case State.Attack:
                Debug.Log("Attack");
                //Debug.Log("YAAAAAAA");
                attacking = true;
                canAttack = false;
                nav.enabled = false;
                anim.applyRootMotion = true;
                Attack();


                timer -= Time.deltaTime;
                if (timer <= 0 && timer > -5)
                {
                    nav.enabled = true;
                    anim.applyRootMotion = false;
                    state = State.Combat;
                }
                break;


            case State.Block:
                ResetAnimationBools();
                anim.SetBool("Block", true);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    nav.enabled = true;
                    ResetAnimationBools();
                    state = State.Combat;
                }


                break;
            case State.Blocked:

                ResetAnimationBools();
                anim.SetBool("Blocked", true);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    nav.enabled = true;
                    ResetAnimationBools();
                    state = State.Combat;
                }

                break;
            case State.Roll:
                Rotation();
                nav.enabled = false;
                anim.applyRootMotion = true;
                ResetAnimationBools();
                anim.SetBool("Roll", true);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    nav.enabled = true;
                    anim.applyRootMotion = false;
                    ResetAnimationBools();
                    state = State.Combat;
                }

                break;

            case State.Hit:
                timer -= Time.deltaTime;
                nav.enabled = false;
                //anim.applyRootMotion = true;
                if (anim.GetBool("Hit") == false)
                {
                    Blood.GetComponent<ParticleSystem>().Play();
                    health -= 20;
                }


                ResetAnimationBools();
                if (timer > 0)
                {
                    if (anim.GetBool("Hit") == false)
                    {

                        gameObject.GetComponent<Rigidbody>().mass = 1;
                        gameObject.GetComponent<Rigidbody>().drag = 1;
                        //gameObject.GetComponent<Rigidbody>().angularDrag = 0;
                        gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 20f * Time.deltaTime);
                        //transform.Translate(transform.forward * 40 * Time.deltaTime);
                    }
                    anim.SetBool("Hit", true);
                }

                if (timer <= 0)
                {
                    anim.SetBool("Hit", false);
                    nav.enabled = true;
                    gameObject.GetComponent<Rigidbody>().mass = 100;
                    gameObject.GetComponent<Rigidbody>().drag = 12;
                    gameObject.GetComponent<Rigidbody>().angularDrag = 4;
                    nav.enabled = true;
                    state = State.Combat;
                }


                break;

            case State.Dead:
                //dead
                sight.SetActive(false);
                ResetAnimationBools();
                anim.SetBool("Dead", true);
                nav.enabled = false;
                GetComponent<CapsuleCollider>().isTrigger = true;
                GetComponent<Rigidbody>().isKinematic = true;
                if (c_ctrl.GetComponent<EnemyCombatCtrl>().enemies.Contains(this.transform))
                {
                    c_ctrl.GetComponent<EnemyCombatCtrl>().enemies.Remove(this.transform);
                }
                /*
                if (c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup1.Contains(this.transform))
                {
                    c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup1.Remove(this.transform);
                }
                if (c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup2.Contains(this.transform))
                {
                    c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup2.Remove(this.transform);
                }*/
                if (AttackGroup == 1)
                {

                    c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup1.Remove(this.transform);
                }
                else
                {

                    c_ctrl.GetComponent<EnemyCombatCtrl>().AttackGroup2.Remove(this.transform);
                }
                GetComponent<EnemyController>().enabled = false;


                break;
        }

        lastPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bush")
        {
            Debug.Log("Enter");
            if (state == State.Dead)
            {
                Hidden = true;
            }
            else
            {
                if (state == State.Searching)
                {
                    ResetAnimationBools();
                    anim.SetBool("Searching", true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bush")
        {
            Debug.Log("Stay");
            if (state == State.Dead)
            {
                Hidden = true;
            }
            else
            if (state == State.Searching)
            {
                ResetAnimationBools();
                anim.SetBool("Searching", true);
            }
            Debug.Log(other.name);
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bush")
        {
            Debug.Log("Exit");
            if (state == State.Dead)
            {
                Hidden = false;
            }
            else
            {
                anim.SetBool("Searching", false);
            }

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
        anim.SetBool("Hit", false);
        anim.SetBool("Block", false);
        anim.SetBool("Lunge", false);
        anim.SetBool("Roll", false);
        anim.SetBool("Blocked", false);
        anim.SetBool("Searching", false);
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
        if (Physics.Raycast(sp, transform.forward, out hitFoward, d + .05f, layerMask))
        {

            if (hitFoward.collider.tag == "Player")
            {
                hitP = true;

                F = true;
                //nav.stoppingDistance = 3f;

                //pos = player.transform.position;


            }
            else
            if (hitFoward.collider.tag == "Enemy")
            {
                hitP = false;
                F = true;
                //nav.stoppingDistance = 0;


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
        if (Physics.Raycast(sp + ((transform.forward + -transform.right) * 2), Vector3.down * (d * 2), out hitL, d, layerMask))
        {


            mPosL = hitL.point;


        }
        Debug.DrawRay(sp + ((transform.forward + -transform.right) * 2), Vector3.down * (d * 2), Color.blue);
        //right
        if (Physics.Raycast(sp + ((transform.forward + transform.right)), Vector3.down, out hitR, d, layerMask))
        {

            mPosR = hitR.point;

        }
        Debug.DrawRay(sp + ((transform.forward + transform.right) * 2), Vector3.down * (d * 2), Color.yellow);

        //Back
        if (Physics.Raycast(sp + -transform.forward, Vector3.down, out hitB, d, layerMask))
        {

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

    void AlertNearbyEnemies()
    {
        if (!alertSphere.activeSelf)
        {
            alertSphere.SetActive(true);
        }
        for (int i = 0; i < nearByEnemies.Count; i++)
        {
            if (!nearByEnemies[i].GetComponent<EnemyController>().playerInSight && !alerted)
            {
                nearByEnemies[i].GetComponent<EnemyController>().alerted = true;
                if (nearByEnemies[i].GetComponent<EnemyController>().state == State.Patrol || nearByEnemies[i].GetComponent<EnemyController>().state == State.Guard || nearByEnemies[i].GetComponent<EnemyController>().state == State.Searching)
                {
                    nearByEnemies[i].GetComponent<EnemyController>().state = State.Alert;
                }
            }
        }
    }

    void Hit()
    {
        if (hit)
        {
            if (player.GetComponent<PController>().isBlocking != true)
            {
               player.GetComponent<PController>().health -= 10;

                playerCurrentHealth = player.GetComponent<PController>().health;
                PlayerHealthUI();

                if (GetComponentInChildren<HitColliders>().Heavy2)
                {
                    Debug.Log("Hit");
                    Instantiate(Special, Smash.transform.position, this.transform.rotation);

                    GetComponentInChildren<HitColliders>().Heavy2 = false;
                    hit = false;
                }
            }
            else
            {
                timer = .8f;
                state = State.Blocked;
            }


        }

    }

    public void PlayerHealthUI()
    {
        if (isPlayer)
        {
            playerHealthIcon.fillAmount = playerCurrentHealth / playerStartingHealth;
        }
        
    }


    void Attack()
    {
        if (distance <= 4)
        {
            num = Random.Range(0, 4);
            if (num == lastAttack)
            {
                switch (num)
                {
                    case 0:
                        num = 1;
                        break;
                    case 1:
                        num = 2;
                        break;
                    case 2:
                        num = 3;
                        break;
                    case 3:
                        num = 0;
                        break;
                }

            }
        }
        else
        if (Class == Type.Heavy && distance > 4)
        {
            num = 1;
            GetComponentInChildren<HitColliders>().Heavy2 = true;
        }


        switch (Class)
        {
            case Type.Basic:
                if (timer <= 0)
                {

                    switch (num)
                    {
                        case 0:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);

                            anim.SetBool("Attack1", true);
                            timer = .8f;
                            lastAttack = 0;
                            break;
                        case 1:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Attack2", true);
                            timer = .8f;
                            lastAttack = 1;
                            break;
                        case 2:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Combo1", true);
                            timer = 2.5f;
                            lastAttack = 2;
                            break;
                        case 3:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Combo2", true);
                            timer = 2.4f;
                            lastAttack = 3;
                            break;
                    }
                }
                break;
            case Type.Heavy:

                if (timer <= 0)
                {

                    switch (num)
                    {
                        case 0:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Attack1", true);
                            timer = .8f;
                            lastAttack = 0;
                            break;
                        case 1:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Attack2", true);
                            GetComponentInChildren<HitColliders>().Heavy2 = true;
                            leap = false;
                            timer = 2.3f;
                            lastAttack = 1;
                            break;
                        case 2:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Combo1", true);
                            timer = 2.5f;
                            lastAttack = 2;
                            break;
                        case 3:
                            ResetAnimationBools();
                            if (leap)
                                anim.SetBool("Lunge", true);
                            anim.SetBool("Combo2", true);
                            timer = 2.4f;
                            lastAttack = 3;
                            break;
                    }
                }

                break;
        }

        lastPos = transform.position;
    }



    void IfMoving(Vector3 lastPos)
    {
        //detect if moved//
        bool x = false;
        bool z = false;
        float xf = 0;
        float zf = 0;

        if (transform.position.x > lastPos.x)
        {
            xf = transform.position.x - lastPos.x;
            if (xf > 0.001)
                x = true;
        }
        else
        {
            xf = lastPos.x - transform.position.x;
            if (xf > 0.001)
                x = true;
        }


        if (transform.position.z > lastPos.z)
        {
            zf = transform.position.z - lastPos.z;
            if (zf > 0.001)
                z = true;
        }
        else
        {
            zf = lastPos.z - transform.position.z;
            if (zf > 0.001)
                z = true;
        }
        //////////////////////////


        //set animation to combat movement//
        if (!x && !z)
        {
            if (state == State.Combat)
            {
                ResetAnimationBools();
                anim.SetBool("cMove", false);
                anim.SetFloat("Idle", 1);
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetFloat("Idle", 0);
            }

        }
        else
        {
            if (state == State.Combat)
            {
                ResetAnimationBools();
                if (distance < 5)
                {
                    anim.SetBool("cMove", true);
                }
                else
                {
                    anim.SetBool("Moving", true);
                }
            }
            else
            {
                anim.SetBool("Moving", true);
            }

        }


        ////////////////////////////////////////////////////


    }
}

    

