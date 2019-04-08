using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPosition : MonoBehaviour
{
    private CheckpointManager checkMan;

    // Start is called before the first frame update
    void Start()
    {
        checkMan = GameObject.FindGameObjectWithTag("CheckMan").GetComponent<CheckpointManager>();
        transform.position = checkMan.checkpointPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
