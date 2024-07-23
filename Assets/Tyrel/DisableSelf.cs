using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    public void DisablingMyself()
    {
        this.gameObject.SetActive(false);
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

}
