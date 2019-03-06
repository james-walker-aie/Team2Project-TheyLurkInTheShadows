using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public GameObject ball;
    private RaycastHit hit;
    public Transform cam;
    public Vector3 offset;
    float pokeForce;
    
    void Start()
    {

    }

    void Update()
    {

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            ball.transform.position = hit.point;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {      
            transform.position = ball.transform.position + Vector3.up;
        }
    }
}
