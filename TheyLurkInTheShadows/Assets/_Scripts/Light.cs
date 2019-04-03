using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (other.GetComponent<PController>().lights.Count == 0)
            {
                other.GetComponent<PController>().lights.Add(gameObject.transform);
            }
            else
            {
                for (int i = 0; i < other.GetComponent<PController>().lights.Count; i++)
                {
                    if (other.GetComponent<PController>().lights[i] != gameObject.transform)
                    {
                        
                        other.GetComponent<PController>().lights.Add(gameObject.transform);
                        
                    }
                }
            }

        }

        for (int j = 0; j < other.GetComponent<PController>().lights.Count; j++)
        {
            for (int k = 0; k < other.GetComponent<PController>().lights.Count; k++)
            {
                if (k != j)
                {
                    if (other.GetComponent<PController>().lights[j] == other.GetComponent<PController>().lights[k])
                    {
                        other.GetComponent<PController>().lights.Remove(gameObject.GetComponent<PController>().lights[j]);
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < other.GetComponent<PController>().lights.Count; i++)
            {
                if(other.GetComponent<PController>().lights[i] == gameObject.transform)
                {
                    other.GetComponent<PController>().lights.Remove(this.gameObject.transform);
                }
            }

        }
    }
}
