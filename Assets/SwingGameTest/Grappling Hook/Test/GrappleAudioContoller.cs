using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAudioContoller : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip grappleSound;
    public AudioClip hitSound;
    public AudioClip retractSound;

    [Header("Audio Settings")]
    public AudioSource audioSource;

    // Methods to play different sounds
    public void PlayGrappleSound()
    {
        PlaySound(grappleSound);
    }

    public void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    public void PlayRetractSound()
    {
        PlaySound(retractSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }


}
