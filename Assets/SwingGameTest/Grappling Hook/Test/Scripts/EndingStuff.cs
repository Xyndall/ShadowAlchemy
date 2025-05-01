using System.Collections;
using UnityEngine;

public class EndingStuff : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.EndGame();
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy this GameObject
        Destroy(gameObject);
    }

}
