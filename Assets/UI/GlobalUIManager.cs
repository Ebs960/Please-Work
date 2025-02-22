using UnityEngine;
using TMPro;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance;

    [Header("Top Civilization UI Elements")]
    public TextMeshProUGUI civilizationNameText;
    public TextMeshProUGUI turnNumberText;
    public TextMeshProUGUI resourceText;

    // Reference to the panel containing all the global UI elements
    public GameObject globalUIPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCivilizationUI(CivilizationData civData)
    {
        if (civData != null)
        {
            civilizationNameText.text = "Civ: " + civData.name;
            turnNumberText.text = "Turn: " + civData.personalTurnCount.ToString();
            resourceText.text = "Science/Turn: " + civData.sciencePerTurn +
                "\nCulture/Turn: " + civData.culturePerTurn +
                "\nFood: " + civData.food +
                "\nGold: " + civData.gold;
        }
        else
        {
            civilizationNameText.text = "Civ:";
            turnNumberText.text = "Turn:";
            resourceText.text = "Science/Turn: \nCulture/Turn: \nFood: \nGold:";
        }
    }

    // Call this to hide the global UI
    public void HideGlobalUI()
    {
        if (globalUIPanel != null)
        {
            globalUIPanel.SetActive(false);
        }
    }

    // Call this to show the global UI
    public void ShowGlobalUI()
    {
        if (globalUIPanel != null)
        {
            globalUIPanel.SetActive(true);
        }
    }
}