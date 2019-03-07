using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeathTimer : MonoBehaviour
{

    public float deathTimer;

    void Start()
    {
        gameObject.transform.parent = null;
        Destroy(gameObject, deathTimer);
    }


}
