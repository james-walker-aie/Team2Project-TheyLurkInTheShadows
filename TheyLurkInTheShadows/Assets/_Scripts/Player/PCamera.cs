using UnityEngine;

public class PCamera : MonoBehaviour
{

    public Transform player;

    public float cameraSmoothingSpeed = 0.125f;
    public Vector3 offset;


    private void LateUpdate()
    {

        Vector3 currentPos = player.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, currentPos, cameraSmoothingSpeed);
        transform.position = smoothedPos;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) == true)
        {

        }
    }


}
