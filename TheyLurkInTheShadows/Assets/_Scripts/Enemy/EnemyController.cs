using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public bool Hidden;
    public float health = 100;
    public Vector3 posTarget = Vector3.zero;
    Vector3 playerLastPos;
    Transform clsPoint = null;
    public GameObject alertSphere;
    public GameObject sight;
    public GameObject fr_Object;
    GameObject player;
    public List<Transform> hidingSpots = new List<Transform>();
    public List<Transform> patrolSpots = new List<Transform>();
    public List<Transform> nearByEnemies = new List<Transform>();
    public List<GameObject> spottedDeadEnemies = new List<GameObject>();
    NavMeshAgent nav;
    public int searchInt;
    public int patrolInt = 0;
    public float guardTime = 5;
    public bool contin;
    float timer;
    bool timerReset = true;
    public bool searchingSpot;
    public bool playerInSight;
    public bool inGuardSpot;
    public bool patrolling;
    public bool traveling;
    public bool Guard;
    public int rotateSpeed = 20;
    public bool firstResponder;
    public bool alerted;


    public enum State
    {
        Guard,
        Patrol,
        Alert,
        investigating,
        Searching,
        Combat,
        Aggro,
        Chase,
        Dead

    }
    public State state;
    public State uClass { get { return state; } }


    // Use this for initialization
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update () {

        if(health <= 0)
        {
            state = State.Dead;
        }

        if(state == State.Guard || state == State.Patrol)
        {
            if (playerInSight)
                state = State.Alert;

            nav.stoppingDistance = 0;
            transform.LookAt(null);
        }
        else
        {
            nav.stoppingDistance = 2;
        }

        

        if(state == State.Aggro)
        {
            nav.speed = 4.5f;
        }
        else
        {
            nav.speed = 3.5f;
        }

        if (playerInSight)
        {
            transform.LookAt(player.transform);
        }





        switch (state)
        {
            case State.Guard:
                //stand still
                Debug.Log("GUARDING");

                //ChangeRotation();
                Debug.Log("guard point rotation:  " + patrolSpots[patrolInt].rotation.eulerAngles.y);
                if(guardTime > 0 || Guard)
                {
                    Quaternion guardR = Quaternion.Euler(transform.rotation.x, patrolSpots[patrolInt].rotation.eulerAngles.y, transform.rotation.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, guardR, rotateSpeed * Time.deltaTime);
                }
                

                if (!Guard)
                {
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
                
                if (!inGuardSpot && !patrolling)
                {
                    float lowestDist = 10000f;
                    float nextDist;
                    
                    for (int i = 0; i < patrolSpots.Count; i++)
                    {
                        
                        if (clsPoint == null)
                        {
                            clsPoint = patrolSpots[0];
                            lowestDist = Vector3.Distance(patrolSpots[0].transform.position , transform.position);
                        }
                        nextDist = Vector3.Distance(patrolSpots[i].transform.position, transform.position);
                        if (nextDist < lowestDist)
                        {
                            lowestDist = nextDist;
                            clsPoint = patrolSpots[i];
                        }
                    }
                    Debug.Log(clsPoint);
                    
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
                    else if( !traveling)
                    {
                        nav.SetDestination(patrolSpots[patrolInt].transform.position);
                        posTarget = patrolSpots[patrolInt].transform.position;
                    }
                }
                float gdis = Vector3.Distance(posTarget, gameObject.transform.position);
                //Debug.Log("gdis: " + gdis);
                if (gdis < 2)
                {
                    Debug.Log("Patrolling");
                    traveling = false;
                    timerReset = true;
                    state = State.Guard;
                }
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
                        Debug.Log("nearby enemies: " + nearByEnemies.Count);
                        if (nearByEnemies.Count > 0)
                        {
                            bool fr = true;
                            for (int k = 0; k < nearByEnemies.Count; k++)
                            {
                                if (nearByEnemies[k].GetComponent<EnemyController>().firstResponder == true)
                                {
                                    fr = false;
                                }
                            }

                            firstResponder = fr;
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
                    if(timer <= 0 && !playerInSight)
                    {
                        searchInt = 0;
                        timerReset = true;
                        state = State.Searching;
                    }
                    
                }

                break;
            case State.Searching:
                //search near by hiding spots
                Debug.Log("Searching");
                transform.LookAt(null);
                if (playerInSight)
                {
                    state = State.Chase;
                }

                if (alertSphere.active == false)
                {
                    alertSphere.SetActive(true);
                }
                if(hidingSpots.Count != 0)
                {
                    Debug.Log("Search2");
                    nav.SetDestination(hidingSpots[searchInt].position);
                    float sDis = Vector3.Distance(hidingSpots[searchInt].position, transform.position);
                    if(sDis < 2)
                    {
                        
                        if(timerReset == true)
                        {
                            timer = 5f;
                            timerReset = false;
                        }
                        searchingSpot = true;
                        timer -= Time.deltaTime;
                        if(timer <= 0)
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
                        if(sDis < 3)
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
                Debug.Log("Chase");
                if (playerInSight || alerted)
                {
                    
                    playerLastPos = player.transform.position;
                    nav.SetDestination(player.transform.position);
                    float pDis = Vector3.Distance(player.transform.position, transform.position);
                    if (pDis < 7)
                        state = State.Aggro;
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
                
                float aDis = Vector3.Distance(player.transform.position, transform.position);

                if (aDis < 4)
                {
                    state = State.Combat;
                }
                else if (aDis > 4 && aDis < 7)
                {
                    
                    nav.SetDestination(player.transform.position);
                }
                else if (aDis >= 7)
                {
                    state = State.Chase;
                }


                break;

            case State.Combat:
                //fight
                Debug.Log("Combat");
                
                float cDis = Vector3.Distance(player.transform.position, transform.position);

                if (cDis > 4)
                {
                    
                    state = State.Aggro;
                }
                    


                break;
            
            
            case State.Dead:
                //dead
                sight.SetActive(false);
                GetComponent<BoxCollider>().isTrigger = true;
                

                break;
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bush")
        {
            if(state == State.Dead)
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

    

}
