using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTrail : MonoBehaviour
{
    public GameObject enemy;
    Vector3 lastPos;
    bool trail = true;
    float timer = 0;
    private Vector3 velocity = Vector3.zero;
    // Update is called once per frame


    private void Update()
    {
       
        transform.position = Vector3.SmoothDamp(transform.position, enemy.transform.position, ref velocity, 0.6f);
    }
}
