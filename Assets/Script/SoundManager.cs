using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public List<AudioSource> audios;

    void Awake()
    {
        AudioSource audioSource = audios[Random.Range(0, audios.Count)];

        audioSource.Play();
    }
}