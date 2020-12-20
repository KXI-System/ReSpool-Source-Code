using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public AudioClip[] Noises;
    public AudioSource Source;

    public float minPitch;
    public float maxPitch;

    private void Awake()
    {
        if (Source == null) { Source = GetComponent<AudioSource>(); }
    }

    public void PlayNoise(int noiseID)
    {
        Source.pitch = 1f;
        Source.PlayOneShot(Noises[noiseID]);
    }

    public void PlayNoiseWithRandPitch(int noiseID)
    {
        Source.pitch = Random.Range(minPitch, maxPitch);
        Source.PlayOneShot(Noises[noiseID]);
    }
}
