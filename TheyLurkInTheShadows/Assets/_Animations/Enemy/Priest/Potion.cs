using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    public GameObject RB1;
    public GameObject LB2;
    public GameObject RP1;
    public GameObject RP2;
    public GameObject LP1;

    void Grab1()
    {
        RB1.SetActive(false);
        RP1.SetActive(true);
        
    }

    void PutDown1()
    {
        RP1.SetActive(false);
        RB1.SetActive(true);
    }

    void Grap2()
    {
        LB2.SetActive(false);
        LP1.SetActive(true);
        
    }

    void Swap()
    {
        RP2.SetActive(true);
        LP1.SetActive(false);
    }

    void Swap2()
    {
        RP2.SetActive(false);
        LP1.SetActive(true);
    }

    void PutDown2()
    {
        LP1.SetActive(false);
        LB2.SetActive(true);
    }
}
