using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Blink : MonoBehaviour
{
    public GameObject ball;
    public GameObject player;
    private RaycastHit hit;
    public Transform cam;
    public Vector3 offset;
    private NavMeshHit Hit;
    public Transform target;
    public bool isBlinking;
    public bool invalid = false;
    float pokeForce;
    public int AreaMaskCheck = 0;
    public bool cameraLag = false;

    public Animator postProcessingAnim;

    private bool blinkableSurface;

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

            if (hit.collider.tag == "Blinkable")
            {
                blinkableSurface = true;      
            }
            else
            {
                blinkableSurface = false;
            }

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
                cameraLag = true;
                transform.position = ball.transform.position + Vector3.up;
                isBlinking = true;
                currentTime = 0;
                GetComponent<BlinkParticles>().BlinkParticleTrigger();
                postProcessingAnim.SetTrigger("Blink");
                
            }

            //isBlinking = false;
        }

        if (Input.GetKeyDown(KeyCode.F) && blinkableSurface == true)
        {
            if (distance <= MaxRange && currentTime >= timerDelay)
            {
                cameraLag = true;
                transform.position = ball.transform.position + Vector3.up;
                isBlinking = true;
                currentTime = 0;
                GetComponent<BlinkParticles>().BlinkParticleTrigger();
                postProcessingAnim.SetTrigger("Blink");

            }
        }

        //to do: a max range that can be changed in the editor, make the teleport ignore the AI sight cones, add in a cooldown rate for teleporting.
        //isBlinking = false;
    }

    void UpdateUI()
    {
        playerBlinkIcon.fillAmount = currentTime / timerDelay;
    }

}
