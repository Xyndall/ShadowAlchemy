using UnityEngine;

public class PlayerTriggerTrack : MonoBehaviour
{
    public bool playerInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "TriggerZone")
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TriggerZone")
            playerInside = false;
    }

}
