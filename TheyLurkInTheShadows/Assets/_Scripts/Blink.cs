using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blink : MonoBehaviour
{

    public GameObject ball;
    public GameObject player;
    private RaycastHit hit;
    public Transform cam;
    public Vector3 offset;
    private NavMeshHit Hit;
    public Transform target;
    private bool invalid = false;
    float pokeForce;
    public int AreaMaskCheck = 0;

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

        invalid = NavMesh.Raycast(transform.position + Vector3.up, ball.transform.position, out Hit, AreaMaskCheck);
        Debug.DrawLine(transform.position + Vector3.up, ball.transform.position, invalid ? Color.red : Color.green);

        if (invalid)
            Debug.DrawRay(Hit.position, Vector3.up, Color.red);


        if (Input.GetKeyDown(KeyCode.F) && !(invalid))
        {
            transform.position = ball.transform.position + Vector3.up;
        }
    }
}
