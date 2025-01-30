using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVelocityVolume : MonoBehaviour
{
    public Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D
    public float maxVelocity = 10f; // Maximum velocity for normalization
    public float minVolume = 0.2f; // Minimum volume of the audio source
    public float maxVolume = 1.0f; // Maximum volume of the audio source
    public float smoothingTime = 0.1f; // Time to smooth volume changes

    private AudioSource audioSource;
    private float targetVolume; // The desired volume based on velocity
    private float currentVolume; // The current volume of the audio source
    private float volumeVelocity; // Used by SmoothDamp for smoothing

    void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();

        // Validate references
        if (playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody2D is not assigned.");
        }

        // Initialize the current volume to the AudioSource's initial volume
        currentVolume = audioSource.volume;
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        // Get the player's velocity magnitude
        float velocityMagnitude = playerRigidbody.velocity.magnitude;

        // Normalize the velocity to a 0-1 range based on maxVelocity
        float normalizedVelocity = Mathf.Clamp01(velocityMagnitude / maxVelocity);

        // Calculate the target volume based on normalized velocity
        targetVolume = Mathf.Lerp(minVolume, maxVolume, normalizedVelocity);

        // Smoothly adjust the current volume towards the target volume
        currentVolume = Mathf.SmoothDamp(currentVolume, targetVolume, ref volumeVelocity, smoothingTime);

        // Apply the smoothed volume to the audio source
        audioSource.volume = currentVolume;
    }
}
