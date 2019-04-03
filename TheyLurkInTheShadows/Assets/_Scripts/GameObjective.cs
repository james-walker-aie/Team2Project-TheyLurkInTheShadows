using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    public bool ObjectHasBeenCollected = false;
    public GameObject Door;

    public void Start()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
        gameObject.transform.Rotate(0, 15 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            ObjectHasBeenCollected = true;
            Door.GetComponent<Collider>().enabled = false;
            Door.SetActive(false);
            gameObject.SetActive(false);           
        }
    }
}
