using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Animator animator; // Animator component to play the animation
    [SerializeField] private string breakAnimationName = "Break"; // Name of the break animation
    [SerializeField] private float destroyDelay = 0.5f; // Time to wait after the animation ends before destroying the object

    private bool isBreaking = false; // Prevent multiple triggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player") && !isBreaking)
        {
            isBreaking = true; // Prevent multiple triggers
            PlayBreakAnimation();
        }
    }

    private void PlayBreakAnimation()
    {
        if (animator != null)
        {
            // Play the break animation
            animator.Play(breakAnimationName);

            // Get the length of the animation
            float animationLength = GetAnimationClipLength(breakAnimationName);

            // Start the coroutine to destroy the object after the animation ends
            StartCoroutine(DestroyAfterAnimation(animationLength + destroyDelay));
        }
        else
        {
            Debug.LogWarning("Animator is not assigned to the BreakableObject.");
            Destroy(gameObject, destroyDelay); // Fallback: destroy the object after the delay
        }
    }

    private float GetAnimationClipLength(string animationName)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationName)
                {
                    return clip.length;
                }
            }
        }

        Debug.LogWarning($"Animation '{animationName}' not found in the Animator.");
        return 0f; // Default to 0 if the animation is not found
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
