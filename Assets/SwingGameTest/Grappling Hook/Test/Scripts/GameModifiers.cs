using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameModifiers : MonoBehaviour
{
    public Toggle BouncyToggle;

    public PhysicsMaterial2D playerMaterial;
    public Collider2D playerCollider;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Bouncy", 0) == 1)
        {
            Bouncy(true);
        }
        else
        {
            Bouncy(false);
        }
    }

    public void Bouncy(bool toggle)
    {
        if(toggle)
        {
            playerMaterial.bounciness = 1f;
            PlayerPrefs.SetInt("Bouncy", 1);
            TemporarilyDisablePlayerCollider();
        }
        else
        {
            playerMaterial.bounciness = 0f;
            PlayerPrefs.SetInt("Bouncy", 0);
            TemporarilyDisablePlayerCollider();
        }
    }

    public void TemporarilyDisablePlayerCollider()
    {
        StartCoroutine(DisableColliderTemporarily());
    }

    private IEnumerator DisableColliderTemporarily()
    {
        
        if (playerCollider != null)
        {
            playerCollider.enabled = false; // Disable the collider
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds
            playerCollider.enabled = true; // Re-enable the collider
        }
        else
        {
            Debug.LogWarning("Player or Collider2D not found!");
        }
    }

}
