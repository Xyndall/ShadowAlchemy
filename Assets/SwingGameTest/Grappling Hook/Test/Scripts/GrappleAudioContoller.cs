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

    
    public bool hasPlayedRetractSound = false; // State flag

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
        if (!hasPlayedRetractSound)
        {
            PlaySound(retractSound);
            hasPlayedRetractSound = true; // Prevents repeated triggers
            
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop(); // Stop any currently playing sound
            audioSource.clip = clip; // Set the new clip
            audioSource.Play(); // Play the new sound immediately
            audioSource.time = 0f; // Ensure the clip starts at the beginning
        }
    }


}
