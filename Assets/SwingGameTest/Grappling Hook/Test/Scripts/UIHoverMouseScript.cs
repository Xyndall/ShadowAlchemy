using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIHoverMouseScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Popup Settings")]
    [SerializeField] private GameObject popupPrefab; // Assign a popup prefab in the inspector
    [SerializeField] private string popupText = "Customizable info here!";

    private GameObject popupInstance;

    // Show popup at mouse position or near UI element
    private void ShowPopup()
    {
        if (popupInstance == null && popupPrefab != null)
        {
            popupInstance = Instantiate(popupPrefab, transform.root); // Place in canvas
            var textComp = popupInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
                textComp.text = popupText;

            // Optional: Position popup near this UI element
            RectTransform popupRect = popupInstance.GetComponent<RectTransform>();
            RectTransform thisRect = GetComponent<RectTransform>();
            if (popupRect != null && thisRect != null)
            {
                Vector3[] corners = new Vector3[4];
                thisRect.GetWorldCorners(corners);
                popupRect.position = corners[2]; // Top-right corner
            }
        }
    }

    private void HidePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPopup();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidePopup();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowPopup();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HidePopup();
    }
}
