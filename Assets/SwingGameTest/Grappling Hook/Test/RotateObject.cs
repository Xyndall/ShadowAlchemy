using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationAmount = 10;
    private void FixedUpdate()
    {
        // Apply the rotation to the object
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
