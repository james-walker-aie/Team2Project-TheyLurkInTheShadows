using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public GameObject ball;
    public GameObject player;
    private RaycastHit hit;
    public Transform cam;
    public Vector3 offset;
    private NavMeshHit Hit;
    public Transform target;
    public bool invalid = false;
    float pokeForce;
    public int AreaMaskCheck = 0;

    [SerializeField]
    public float timerDelay = 5f;
    [SerializeField]
    public float MaxRange = 15f;
    public float currentTime;
    public float distance;

    public LayerMask layerMask;

    [SerializeField] Image playerBlinkIcon;
    [SerializeField] bool isPlayer = false;

    void Start()
    {

    }

    void Update()
    {
        UpdateUI();
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity , layerMask))
        {
            ball.transform.position = hit.point;
        }

        invalid = NavMesh.Raycast(transform.position + Vector3.up, ball.transform.position, out Hit, AreaMaskCheck);
        Debug.DrawLine(transform.position + Vector3.up, ball.transform.position, invalid ? Color.red : Color.green);

        if (invalid)
            Debug.DrawRay(Hit.position, Vector3.up, Color.red);

        distance = Vector3.Distance(transform.position, ball.transform.position);

        currentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && !(invalid))
        {
            if (distance <= MaxRange && currentTime >= timerDelay)
            {
                transform.position = ball.transform.position + Vector3.up;
                currentTime = 0;
                GetComponent<BlinkParticles>().BlinkParticleTrigger();
            }


        }

        //to do: a max range that can be changed in the editor, make the teleport ignore the AI sight cones, add in a cooldown rate for teleporting.
    }

    void UpdateUI()
    {
        playerBlinkIcon.fillAmount = currentTime / timerDelay;
    }

}
