using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestController : MonoBehaviour
{

    public bool playerInSight;
    float timer;
    int lastTask = 3;
    string tsk;
    NavMeshAgent nav;
    public Transform[] guardPoints;
    bool taskComplete = true;
    bool startTimer = false;
    bool hasTask = false;
    string task;
    Animator anim;
    bool fail;
    public float health = 100;
    public GameObject alertSphere;
    public List<Transform> nearByEnemies = new List<Transform>();
    public Transform runTo;
    AudioSource AS;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector3 lastPos;
    Vector3 nPos;
    GameObject player;
    public GameObject backStab;
    public GameObject Sight;
    public GameObject Blood;
    public bool hit;
    public AudioClip Dead;
    public AudioClip Hit;
    public ParticleSystem p_fail;

    public enum State
    {
        Tasks,
        AlertTime,
        Run,
        Dead

    }
    public State state;
    public State uClass { get { return state; } }

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            state = State.Dead;


        Vector3 worldDeltaPosition = nPos - transform.position;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy).normalized;

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        velocity = smoothDeltaPosition;


        anim.SetFloat("Movement", velocity.y);
        anim.SetFloat("Horizontal", velocity.x);

        if (playerInSight)
        {
            if(state != State.Run && state != State.AlertTime && state != State.Dead)
            {
                timer = 3;
                state = State.AlertTime;
            }
        }

        if (hit)
        {
            Blood.GetComponent<ParticleSystem>().Play();
            AS.clip = Hit;
            AS.Play();
            hit = false;
        }

        switch (state)
        {
            case State.Tasks:

                IfMoving(lastPos);
                if (!hasTask || taskComplete)
                {
                    //Debug.Log("1");
                    task = ChooseTask();
                }
                if (task == null)
                    return;
                else
                    DoTask(task);

                if (startTimer)
                {
                    timer -= Time.deltaTime;
                }
                
                break;
            case State.AlertTime:
                Rotation();
                nav.isStopped = true;
                timer -= Time.deltaTime;
                if (timer <= 0)
                    state = State.Run;
                ResetAnim();

                break;

            case State.Run:
                nav.isStopped = false;
                AlertNearbyEnemies();
                nav.speed = 3;
                nav.SetDestination(runTo.transform.position);
                ResetAnim();
                anim.SetBool("Running", true);

                break;
            case State.Dead:

                ResetAnim();
                AS.clip = Dead;
                AS.Play();
                anim.SetBool("Dead", true);
                nav.enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                backStab.SetActive(false);
                Sight.SetActive(false);
                GetComponent<PriestController>().enabled = false;
                break;
        }
        
        lastPos = transform.position;
    }

    void Rotation()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    string ChooseTask()
    {
        int Rand = Random.Range(0, 2);
        startTimer = false;
        
        if (Rand == lastTask)
            return null;
        
        
        lastTask = Rand;
        taskComplete = false;
        if (Rand == 0)
        {
            hasTask = true;
            timer = 8;
            return "Write"; 
        }
        else
        if(Rand == 1)
        {
            hasTask = true;
            timer = 12;
            return "Chem";
        }
        

        return null;
    }

    void DoTask(string task)
    {
        switch (task)
        {
            case "Write":
                nav.SetDestination(guardPoints[0].transform.position);
                nPos = guardPoints[0].transform.position;
                float dist = Vector3.Distance(guardPoints[0].transform.position, transform.position);
                //Debug.Log("1: " + dist);
                if (dist < 1f)
                {
                    Rotation(guardPoints[0].transform);
                    if(transform.rotation.eulerAngles == guardPoints[0].transform.rotation.eulerAngles)
                    {
                        startTimer = true;
                        ResetAnim();
                        if (!fail)
                            anim.SetBool("Writing", true);
                        else
                            anim.SetBool("Sad", true);
                    }
                    
                }
                if (startTimer)
                {
                    if (timer <= 0)
                    {
                        ResetAnim();
                        taskComplete = true;
                        startTimer = false;
                    }
                        
                }

                break;
            case "Chem":
                nav.SetDestination(guardPoints[1].transform.position);
                nPos = guardPoints[1].transform.position;
                float dis = Vector3.Distance(guardPoints[1].transform.position, transform.position);
                //Debug.Log("2: " + dis);
                if (dis < 1f)
                {
                    
                    startTimer = true;
                    Rotation(guardPoints[1].transform);
                    ResetAnim();
                    anim.SetBool("Chem", true);
                }
                if (startTimer)
                {
                    if (timer <= 0)
                    {
                        float rnd = Random.Range(0f, 1f);

                        if(rnd < .55f)
                        {
                            //fail
                            fail = true;
                        }
                        else
                        {
                            //Work

                            p_fail.Play();
                            fail = false;
                        }
                        ResetAnim();
                        taskComplete = true;
                        startTimer = false;
                    }
                }
                        

                break;
          
        }
    }

    void Rotation(Transform guardPoint)
    {
        Quaternion guardR = Quaternion.Euler(transform.rotation.x, guardPoint.rotation.eulerAngles.y, transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, guardR, 3 * Time.deltaTime);
    }

    void ResetAnim()
    {
        anim.SetBool("Walking",false);
        anim.SetBool("Running", false);
        anim.SetBool("Sad", false);
        anim.SetBool("Chem", false);
        anim.SetBool("Writing", false);
    }

    void AlertNearbyEnemies()
    {

        if (!alertSphere.activeSelf)
        {
            alertSphere.SetActive(true);
        }

        if(nearByEnemies.Count > 0)
        {
            for (int i = 0; i < nearByEnemies.Count; i++)
            {

                if (nearByEnemies[i].GetComponent<EnemyController>().state != EnemyController.State.Combat && !nearByEnemies[i].GetComponent<EnemyController>().alerted)
                {

                    nearByEnemies[i].GetComponent<EnemyController>().alerted = true;
                    if (nearByEnemies[i].GetComponent<EnemyController>().state == EnemyController.State.Patrol || nearByEnemies[i].GetComponent<EnemyController>().state == EnemyController.State.Guard ||
                        nearByEnemies[i].GetComponent<EnemyController>().state == EnemyController.State.Searching || nearByEnemies[i].GetComponent<EnemyController>().state == EnemyController.State.AlertTime)
                    {

                        nearByEnemies[i].GetComponent<EnemyController>().state = EnemyController.State.Alert;
                    }
                }
            }
        }
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
            anim.SetBool("Walking", false);
        }
        else
        {  
             anim.SetBool("Walking", true);
        }
    }

    void FootR()
    {
        //
    }

    void FootL()
    {
        //
    }
}
