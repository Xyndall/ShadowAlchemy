using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject uiElement;  // Drag your UI element (e.g., Panel) here in the Inspector
    private bool isUIVisible = false;

    public void Toggle()
    {
        isUIVisible = !isUIVisible;  // Toggle the state
        uiElement.SetActive(isUIVisible);  // Show or hide the UI element based on the state
    }
}
