using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint; // Set this in the Inspector to the desired location
    [SerializeField] private GameObject carrierPrefab; // The object that carries the player
    [SerializeField] private float grabSpeed = 8f; // Speed at which the carrier moves to grab the player
    [SerializeField] private float transportSpeed = 5f; // Speed at which the carrier moves to the teleport point
    [SerializeField] private Transform spawnPoint; // Transform for spawning the carrier
    [SerializeField] private Vector2 flyAwayDirection = new Vector2(1f, 1f); // Direction for carrier to fly away
    [SerializeField] private float flyAwayDistance = 10f; // Distance for carrier to fly away before destruction

    private bool hasReturnedToTeleportPoint = true; // Tracks if the player has returned to the teleport point

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasReturnedToTeleportPoint) // Ensure the player has the correct tag and can trigger
        {
            hasReturnedToTeleportPoint = false; // Prevent re-triggering
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            NewGrappleTest grappleScript = other.GetComponent<NewGrappleTest>();

            // Spawn the carrier
            GameObject carrier = Instantiate(carrierPrefab, spawnPoint.position, Quaternion.identity);
            StartCoroutine(MoveCarrierToPlayer(carrier.transform, other.transform, rb, grappleScript));
        }
    }

    private IEnumerator MoveCarrierToPlayer(Transform carrier, Transform player, Rigidbody2D rb, NewGrappleTest grappleScript)
    {
        // Move carrier to the player
        while (Vector2.Distance(carrier.position, player.position) > 0.1f)
        {
            carrier.position = Vector2.MoveTowards(carrier.position, player.position, grabSpeed * Time.deltaTime);
            yield return null;
        }

        // Disable player movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        if (grappleScript != null) grappleScript.enabled = false;

        // Attach player to the carrier
        player.SetParent(carrier);

        // Move carrier (with player) to teleport point
        while (Vector2.Distance(carrier.position, teleportPoint.position) > 0.1f)
        {
            carrier.position = Vector2.MoveTowards(carrier.position, teleportPoint.position, transportSpeed * Time.deltaTime);
            yield return null;
        }

        // Detach player and reset settings
        player.SetParent(null);
        if (rb != null) rb.isKinematic = false;
        if (grappleScript != null) grappleScript.enabled = true;

        // Mark the player as having returned to the teleport point
        hasReturnedToTeleportPoint = true;

        // Fly away and destroy carrier
        Vector2 flyAwayTarget = (Vector2)carrier.position + (flyAwayDirection.normalized * flyAwayDistance);
        while (Vector2.Distance(carrier.position, flyAwayTarget) > 0.1f)
        {
            carrier.position = Vector2.MoveTowards(carrier.position, flyAwayTarget, grabSpeed * Time.deltaTime); // Use grabSpeed for fly away
            yield return null;
        }

        Destroy(carrier.gameObject);
    }
}
