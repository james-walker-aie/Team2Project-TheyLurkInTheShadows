using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (other.GetComponent<TestLAT>().lights.Count == 0)
            {
                other.GetComponent<TestLAT>().lights.Add(gameObject.transform);
            }
            else
            {
                for (int i = 0; i < other.GetComponent<TestLAT>().lights.Count; i++)
                {
                    if (other.GetComponent<TestLAT>().lights[i] != gameObject.transform)
                    {
                        
                        other.GetComponent<TestLAT>().lights.Add(gameObject.transform);
                        
                    }
                }
            }

        }

        for (int j = 0; j < other.GetComponent<TestLAT>().lights.Count; j++)
        {
            for (int k = 0; k < other.GetComponent<TestLAT>().lights.Count; k++)
            {
                if (k != j)
                {
                    if (other.GetComponent<TestLAT>().lights[j] == other.GetComponent<TestLAT>().lights[k])
                    {
                        other.GetComponent<TestLAT>().lights.Remove(gameObject.GetComponent<TestLAT>().lights[j]);
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < other.GetComponent<TestLAT>().lights.Count; i++)
            {
                if(other.GetComponent<TestLAT>().lights[i] == gameObject.transform)
                {
                    other.GetComponent<TestLAT>().lights.Remove(this.gameObject.transform);
                }
            }

        }
    }
}
