using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [SerializeField]
    private AudioClip[] clips;

    float timer;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void Step()
    {
        if (timer < 0)
        {
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);

            timer = 0.1f;
        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
