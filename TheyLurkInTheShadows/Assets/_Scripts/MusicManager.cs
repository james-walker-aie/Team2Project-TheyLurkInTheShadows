using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public AudioClip[] songList;
    private AudioSource audioSource;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetRandomSong();
        audioSource.Play();
    }

    void Update()
    {
        
    }

    private AudioClip GetRandomSong()
    {
        return songList[UnityEngine.Random.Range(0, songList.Length)];
    }
}
