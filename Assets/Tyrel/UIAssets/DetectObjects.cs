using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{
    public ShadowInteract ShadowInteract;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShadowObject"))
        {
            Debug.Log("Shadow object is in trigger");
            ShadowInteract.shadowObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ShadowObject"))
        {
            Debug.Log("Shadow object is in trigger");
            ShadowInteract.shadowObject = null;
        }
    }

}
