using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLAT : MonoBehaviour
{
    public float visibility;

    public List<Transform> lights = new List<Transform>();

    // Update is called once per frame
    private void Update()
    {
        if(lights.Count > 0)
        {
            Transform clsLight = null;
            float dis = 0;

            for(int i = 0; i< lights.Count; i++)
            {
                if(clsLight == null)
                {
                    clsLight = lights[i];
                    dis = Vector3.Distance(clsLight.transform.position, this.transform.position);
                }
                else
                {
                    float dist2 = Vector3.Distance(lights[i].transform.position, this.transform.position);
                    if(dist2 < dis)
                    {
                        clsLight = lights[i];
                        dis = dist2;
                    }
                }  
            }

            float disToLight = Vector3.Distance(clsLight.transform.position, transform.position);
            Debug.Log(disToLight);
            float vis = visibility;
            visibility = 100 / disToLight * 4f;
        }
        else
        {
            visibility = 10;
        }
    }
}
