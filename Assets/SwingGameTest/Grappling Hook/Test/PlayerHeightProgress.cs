using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeightProgress : MonoBehaviour
{
    public Slider heightSlider;
    public Transform player;          // Reference to the player's Transform component
    public float maxHeight = 100f;    // Maximum height player is aiming to reach
    public TextMeshProUGUI currentHeightText;    // Text component for displaying current height
    public TextMeshProUGUI maxHeightText;        // Text component for displaying max height

    private void Start()
    {
        // Set slider min and max values
        heightSlider.minValue = 0;
        heightSlider.maxValue = maxHeight;

        // Set the max height text once, as it doesn't change
        maxHeightText.text = $"{Mathf.FloorToInt(maxHeight)} m";
    }

    private void Update()
    {
        // Update slider with the player's current Y height
        heightSlider.value = player.position.y;

        // Update the current height text without decimal places
        currentHeightText.text = $"{Mathf.FloorToInt(player.position.y)} m";
        //currentHeightText.text = $"Height: {Mathf.FloorToInt(player.position.y)} m";
    }
}
