using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrow : MonoBehaviour
{
    public Sprite red;
    public Sprite yellow;
    public Sprite blue;
    // Update is called once per frame
    void Update()
    {
        if(GetComponentInParent<EnemyController>().state == EnemyController.State.Combat)
        {
            GetComponent<SpriteRenderer>().sprite = red;
        }
        else
        if (GetComponentInParent<EnemyController>().state == EnemyController.State.AlertTime || GetComponentInParent<EnemyController>().state == EnemyController.State.Alert)
        {
            GetComponent<SpriteRenderer>().sprite = yellow;
        }
        else
        if(GetComponentInParent<EnemyController>().state == EnemyController.State.Searching)
        {
            GetComponent<SpriteRenderer>().sprite = blue;
        }
        else
        if (GetComponentInParent<EnemyController>().state == EnemyController.State.Patrol || GetComponentInParent<EnemyController>().state == EnemyController.State.Guard)
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }

    }
}
