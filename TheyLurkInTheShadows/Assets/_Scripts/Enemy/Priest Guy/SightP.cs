using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightP : MonoBehaviour
{
    public GameObject self;
    public List<Transform> inSight = new List<Transform>();
    public GameObject sightSinc;
    public Material green;
    PriestController states;
    public LayerMask layerMask = ~(1 << 11);
    public LayerMask layerMask2 = ~(1 << 11);
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        if(inSight.Count <= 0)
        {
            GetComponentInParent<PriestController>().playerInSight = false;
        }
        Debug.Log("1");
        if (inSight.Count > 0 && GetComponentInParent<PriestController>().state != PriestController.State.Dead)
        {
            Debug.Log("2");
            for (int i = 0; i < inSight.Count; i++)
            {
                Debug.Log("3");
                RaycastHit hit;
                LayerMask LM = layerMask;
                
                LM = layerMask;
                
                

                if (Physics.Raycast(this.sightSinc.transform.position, inSight[i].GetComponentInChildren<Transform>().position - this.sightSinc.transform.position, out hit, Mathf.Infinity, LM))
                {

                    Debug.Log("4");
                    Debug.DrawRay(this.sightSinc.transform.position, (inSight[i].GetComponentInChildren<Transform>().position - this.sightSinc.transform.position) * hit.distance, Color.red);
                    
                    
                    switch (hit.collider.GetComponent<Transform>().tag)
                    {
                        
                        case "Player":

                            if (hit.collider.tag == "Player")
                            {
                                if (hit.collider.GetComponent<PController>().hidden == false)
                                {

                                    GetComponentInParent<PriestController>().playerInSight = true;
                                }

                            }
                            break;
                            
                        default:
                            
                            GetComponentInParent<PriestController>().playerInSight = false;
                            Debug.Log("Boooooo");
                            break; 

                    }

                }
                else
                {
                    Debug.Log("no hit");
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
                    
                gameObject.GetComponentInParent<PriestController>().playerInSight = false;
                if (inSight.Contains(other.transform.GetComponent<PController>().sightSync))
                {

                    inSight.Remove(other.transform.GetComponent<PController>().sightSync);
                }
            }


            

            
        }
    }

    


}
