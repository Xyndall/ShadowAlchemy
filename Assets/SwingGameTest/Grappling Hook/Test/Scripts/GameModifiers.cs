using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameModifiers : MonoBehaviour
{
    public GameObject ModifierPanel;
    public Toggle BouncyToggle;
    public Toggle FrictionToggle;

    public PhysicsMaterial2D playerMaterial;
    public Collider2D playerCollider;

    private void Start()
    {
        ModifierPanel.SetActive(false);
        if (PlayerPrefs.GetInt("Bouncy", 0) == 1)
        {
            Bouncy(true);
            BouncyToggle.isOn = true;
        }
        else
        {
            Bouncy(false);
            BouncyToggle.isOn = false;
        }
        if (PlayerPrefs.GetInt("Friction", 0) == 1)
        {
            Frictionless(true);
            FrictionToggle.isOn = true;
        }
        else
        {
            Frictionless(false);
            FrictionToggle.isOn = false;
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
