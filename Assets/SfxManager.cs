using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [Header("Audio Files")]
    public AudioClip interactSound;


    private AudioSource defaultSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one audio manager!");
            return;
        }
        
        Instance = this;

        defaultSource = GetComponent<AudioSource>();
        if (defaultSource == null)
        {
            Debug.LogError("No default audiosource set for SfxManager");
        }
    }

    public void PlayAudio(AudioClip clip, AudioSource source = null)
    {
        
        (source ?? defaultSource).PlayOneShot(clip);
    }
}
