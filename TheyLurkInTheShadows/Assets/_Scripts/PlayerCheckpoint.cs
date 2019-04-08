using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private CheckpointManager checkMan;

    void Start()
    {
        checkMan = GameObject.FindGameObjectWithTag("CheckMan").GetComponent<CheckpointManager>();
    }

    public void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            checkMan.checkpointPos = transform.position;
        }

    }
}
