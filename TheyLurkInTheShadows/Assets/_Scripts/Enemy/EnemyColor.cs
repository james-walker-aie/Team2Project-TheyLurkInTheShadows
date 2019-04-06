using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    public Material red;
    public Material pink;
    public Material yellow;
    public Material blue;
    

    // Update is called once per frame
    void Update()
    {
        
        if(GetComponentInParent<EnemyController>().state != EnemyController.State.Dead)
        {
            if (GetComponentInParent<EnemyController>().state == EnemyController.State.Combat)
            {
                if (GetComponent<SkinnedMeshRenderer>())
                {
                    GetComponent<SkinnedMeshRenderer>().material = red;
                }
                else
                {
                    GetComponent<MeshRenderer>().material = red;
                }

            }
            else
            if (GetComponentInParent<EnemyController>().state == EnemyController.State.AlertTime || GetComponentInParent<EnemyController>().state == EnemyController.State.Alert)
            {
                if (GetComponent<SkinnedMeshRenderer>())
                {
                    GetComponent<SkinnedMeshRenderer>().material = pink;
                }
                else
                {
                    GetComponent<MeshRenderer>().material = pink;
                }

            }
            else
            if (GetComponentInParent<EnemyController>().state == EnemyController.State.Searching)
            {
                if (GetComponent<SkinnedMeshRenderer>())
                {
                    GetComponent<SkinnedMeshRenderer>().material = blue;
                }
                else
                {
                    GetComponent<MeshRenderer>().material = blue;
                }

            }
            else
            if (GetComponentInParent<EnemyController>().state == EnemyController.State.Patrol || GetComponentInParent<EnemyController>().state == EnemyController.State.Guard)
            {
                if (GetComponent<SkinnedMeshRenderer>())
                {
                    GetComponent<SkinnedMeshRenderer>().material = yellow;
                }
                else
                {
                    GetComponent<MeshRenderer>().material = yellow;
                }

            }
        }
        else
        {
            if (GetComponent<SkinnedMeshRenderer>())
            {
                GetComponent<SkinnedMeshRenderer>().material = yellow;
            }
            else
            {
                GetComponent<MeshRenderer>().material = yellow;
            }
        }
        
    }
}
