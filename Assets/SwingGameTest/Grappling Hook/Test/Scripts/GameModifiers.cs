using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameModifiers : MonoBehaviour
{
    public GameObject ModifierPanel;
    public Toggle BouncyToggle;
    public Toggle FrictionToggle;
    public Toggle GravityToggle;

    public PhysicsMaterial2D playerMaterial;
    public Collider2D playerCollider;
    private Vector2 originalGravity; // Store the original gravity
    private void Start()
    {
        ModifierPanel.SetActive(false);
        // Store the original gravity at startup
        originalGravity = Physics2D.gravity;

        if (PlayerPrefs.GetInt("Bouncy", 0) == 1)
        {
            Bouncy(true);
            BouncyToggle.isOn = true;
            Debug.Log("Bouncy is ON");
        }
        else
        {
            Bouncy(false);
            BouncyToggle.isOn = false;
            Debug.Log("Bouncy is OFF");
        }
        if (PlayerPrefs.GetInt("Friction", 0) == 1)
        {
            Frictionless(true);
            FrictionToggle.isOn = true;
            Debug.Log("Frictionless is ON");
        }
        else
        {
            Frictionless(false);
            FrictionToggle.isOn = false;
            Debug.Log("Frictionless is OFF");
        }

        if (PlayerPrefs.GetInt("Gravity", 0) == 1)
        {
            LowGravity(true);
            GravityToggle.isOn = true;
            Debug.Log("Low Gravity is ON");
        }
        else
        {
            LowGravity(false);
            GravityToggle.isOn = false;
            Debug.Log("Low Gravity is OFF");
        }
    }

    public void Bouncy(bool toggle)
    {
        if(toggle)
        {
            playerMaterial.bounciness = 1f;
            playerMaterial.friction = 0f;
            PlayerPrefs.SetInt("Bouncy", 1);
            TemporarilyDisablePlayerCollider();
        }
        else
        {
            playerMaterial.bounciness = 0f;
            playerMaterial.friction = 1f;
            PlayerPrefs.SetInt("Bouncy", 0);
            TemporarilyDisablePlayerCollider();
        }
    }
    public void Frictionless(bool toggle)
    {
        if (toggle)
        {
            playerMaterial.friction = 0f;
            PlayerPrefs.SetInt("Friction", 1);
            TemporarilyDisablePlayerCollider();
        }
        else
        {
            playerMaterial.friction = 1f;
            PlayerPrefs.SetInt("Friction", 0);
            TemporarilyDisablePlayerCollider();
        }
    }
    public void LowGravity(bool toggle)
    {
        if (toggle)
        {
            Physics2D.gravity = originalGravity * 0.5f; // Halve the gravity
            Debug.Log("gravity is set to " + Physics2D.gravity);
            PlayerPrefs.SetInt("Gravity", 1);
            TemporarilyDisablePlayerCollider();
            
        }
        else
        {
            Physics2D.gravity = originalGravity; // Restore original gravity
            PlayerPrefs.SetInt("Gravity", 0);
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
