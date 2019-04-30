using UnityEngine;

public class PCamera : MonoBehaviour
{

    public Transform player;

    public float cameraSmoothingSpeed = 0.125f;
    public float dynamicCameraSmoothingSpeed;
    public Vector3 offset;

    void Start()
    {
        dynamicCameraSmoothingSpeed = cameraSmoothingSpeed;
    }

    private void LateUpdate()
    {

        Vector3 currentPos = player.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, currentPos, dynamicCameraSmoothingSpeed);
        transform.position = smoothedPos;
    }

    private void Update()
    {
        if (player.GetComponent<Blink>().cameraLag == true)
        {
            dynamicCameraSmoothingSpeed = 0f;
            player.GetComponent<Blink>().cameraLag = false;
        }

        if (dynamicCameraSmoothingSpeed < cameraSmoothingSpeed)
        {
            dynamicCameraSmoothingSpeed += ((Time.deltaTime) / 16);
        }

    }


}
