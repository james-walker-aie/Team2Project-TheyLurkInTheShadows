using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public GameObject self;
    public List<Transform> inSight = new List<Transform>();
    public GameObject sightSinc;
    public Material green;
    EnemyController states;
    public LayerMask layerMask = ~(1 << 11);
    public LayerMask layerMask2 = ~(1 << 11);
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        if(inSight.Count <= 0)
        {
            GetComponentInParent<EnemyController>().playerInSight = false;
        }

        if (inSight.Count > 0 && GetComponentInParent<EnemyController>().state != EnemyController.State.Dead)
        {
            
            for (int i = 0; i < inSight.Count; i++)
            {
                
                RaycastHit hit;
                LayerMask LM = layerMask;
                if(GetComponentInParent<EnemyController>().state != EnemyController.State.Guard && GetComponentInParent<EnemyController>().state != EnemyController.State.Patrol)
                {
                     LM = layerMask2;
                }
                else
                {
                     LM = layerMask;
                }
                

                if (Physics.Raycast(this.sightSinc.transform.position, inSight[i].GetComponentInChildren<Transform>().position - this.sightSinc.transform.position, out hit, Mathf.Infinity, LM))
                {
                    

                    Debug.DrawRay(this.sightSinc.transform.position, (inSight[i].GetComponentInChildren<Transform>().position - this.sightSinc.transform.position) * hit.distance, Color.red);
                    
                    
                    switch (hit.collider.GetComponent<Transform>().tag)
                    {
                        
                        case "Player":

                            if (hit.collider.tag == "Player")
                            {
                                if (hit.collider.GetComponent<PController>().hidden == false)
                                {
                                    
                                    GetComponentInParent<EnemyController>().playerInSight = true;
                                }
                                else
                                {
                                    if(GetComponentInParent<EnemyController>().state == EnemyController.State.Searching && GetComponentInParent<EnemyController>().searchingSpot)
                                    {
                                        
                                        GetComponentInParent<EnemyController>().playerInSight = true;
                                        GetComponentInParent<EnemyController>().state = EnemyController.State.Chase;


                                    }
                                    else
                                    {
                                        //Debug.Log("Player Hidden");
                                    }
                                    
                                }

                            }
                            break;

                        case "Enemy":
                            GameObject hitEnemy = hit.transform.gameObject;
                            
                            if (hit.collider.tag == "Enemy")
                            {

                                //Debug.Log("seen enemy1");
                                if (hit.transform.GetComponent<EnemyController>().Hidden == false)
                                {
                                    //Debug.Log("seen enemy2");
                                    if (hitEnemy!= null)
                                    {
                                        //Debug.Log("seen enemy3");
                                        if (hitEnemy.GetComponent<EnemyController>().state == EnemyController.State.Dead )
                                        {
                                            //Debug.Log("seen enemy4");
                                            if(GetComponentInParent<EnemyController>().spottedDeadEnemies.Count <= 0)
                                            {
                                                //Debug.Log("seen enemy5");
                                                if(GetComponentInParent<EnemyController>().state == EnemyController.State.Guard || GetComponentInParent<EnemyController>().state == EnemyController.State.Patrol) 
                                                    GetComponentInParent<EnemyController>().state = EnemyController.State.Alert;

                                                GetComponentInParent<EnemyController>().posTarget = hit.collider.transform.position;
                                                GetComponentInParent<EnemyController>().spottedDeadEnemies.Add(hitEnemy);
                                            }
                                            else
                                            {
                                                //Debug.Log("seen enemy6");
                                                int check = 0;
                                                for (int k = 0; k < GetComponentInParent<EnemyController>().spottedDeadEnemies.Count; k++)
                                                {
                                                    if (GetComponentInParent<EnemyController>().spottedDeadEnemies[k] != hitEnemy)
                                                    {
                                                        check += 1;
                                                    }
                                                }
                                                if(check == GetComponentInParent<EnemyController>().spottedDeadEnemies.Count)
                                                {
                                                    //Debug.Log("seen enemy7");
                                                    if (GetComponentInParent<EnemyController>().state == EnemyController.State.Guard || GetComponentInParent<EnemyController>().state == EnemyController.State.Patrol)
                                                        GetComponentInParent<EnemyController>().state = EnemyController.State.Alert;

                                                    GetComponentInParent<EnemyController>().posTarget = hit.collider.transform.position;
                                                    GetComponentInParent<EnemyController>().spottedDeadEnemies.Add(hitEnemy);
                                                }
                                                //Debug.Log("seen enemy8");
                                            }
                                            
                                            
                                        }
                                    }
                                    
                                }
                                

                            }

                            break;
                        default:
                            
                            GetComponentInParent<EnemyController>().playerInSight = false;
                            Debug.Log("defualt" + hit.collider.name);
                            break;

                        

                    }

                    

                }
                else
                {
                    //Debug.DrawRay(this.sightSinc.transform.position, (inSight[i].GetComponentInChildren<Transform>().position - this.sightSinc.transform.position) * 100, Color.red);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {

            
            if(other.tag == "Player")
            {
                
                if (!inSight.Contains(other.transform.GetComponent<PController>().sightSync))
                {
                    inSight.Add(other.transform.GetComponent<PController>().sightSync);
                }

            }
            else
            if(other.tag == "Enemy")
            {
                if (!inSight.Contains(other.GetComponent<EnemyController>().sightSync.transform))
                {
                    inSight.Add(other.GetComponent<EnemyController>().sightSync.transform);
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" || other.tag == "Enemy")
        {
            
            if (other.tag == "Player")
            {
                    
                gameObject.GetComponentInParent<EnemyController>().playerInSight = false;
                if (inSight.Contains(other.transform.GetComponent<PController>().sightSync))
                {

                    inSight.Remove(other.transform.GetComponent<PController>().sightSync);
                }
            }


            

            if (other.tag == "Enemy")
            {
                
                if (inSight.Contains(other.GetComponent<EnemyController>().sightSync.transform))
                {
                    
                    inSight.Remove(other.GetComponent<EnemyController>().sightSync.transform);
                }
            }
        }
    }

    


}
