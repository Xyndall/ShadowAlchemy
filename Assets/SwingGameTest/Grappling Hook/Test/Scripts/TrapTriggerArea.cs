using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerArea : MonoBehaviour
{
    [Tooltip("Assign all ArrowTrap scripts to be controlled by this trigger.")]
    public List<ArrowTrap> linkedTraps;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var trap in linkedTraps)
                if (trap != null) trap.EnableTrap();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var trap in linkedTraps)
                if (trap != null) trap.DisableTrap();
        }
    }
}