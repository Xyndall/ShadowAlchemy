using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    // Customizable clamp values
    public Vector3 minPosition = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    public Vector3 maxPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        // Clamp the position
        newPos.x = Mathf.Clamp(newPos.x, minPosition.x, maxPosition.x);
        newPos.y = Mathf.Clamp(newPos.y, minPosition.y, maxPosition.y);
        newPos.z = Mathf.Clamp(newPos.z, minPosition.z, maxPosition.z);

        transform.localPosition = newPos;
    }
}
