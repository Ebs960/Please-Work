using UnityEngine;
using UnityEngine.UI;

public class UnitUIController : MonoBehaviour
{
    // Reference to the Unit panel GameObject.
    public GameObject unitPanel;

    // Reference to the X button on the Unit UI.
    public Button closeButton;

    void Awake()
    {
        if (closeButton != null)
        {
            // When the close button is clicked, the panel will close.
            closeButton.onClick.AddListener(CloseUnitUI);
        }
    }

    void Update()
    {
        // Optionally, also close the UI when the user presses the X key.
        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseUnitUI();
        }
    }

    // Disables the Unit UI panel.
    public void CloseUnitUI()
    {
        if (unitPanel != null)
        {
            unitPanel.SetActive(false);
        }
    }
} 