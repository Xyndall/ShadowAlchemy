using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHit.Instance.SetTeleportPosition(transform.position);
            Debug.Log("Checkpoint set at: " + transform.position);
        }
    }
}
