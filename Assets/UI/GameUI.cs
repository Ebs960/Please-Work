using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("Panels")]
    public GameObject cityPanel;
    public GameObject unitPanel;

    [Header("City UI")]
    public TextMeshProUGUI cityNameText;
    public TextMeshProUGUI cityResourcesText;
    public TextMeshProUGUI cityPopulationText;

    [Header("Unit UI")]
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI unitOwnerText;
    public TextMeshProUGUI unitDescriptionText;
    public TextMeshProUGUI unitStatsText;
    public Button moveButton;
    public Button attackButton;

    void Awake()
    {
        Instance = this;
        HideAllPanels();
    }

    public void ShowCityPanel(City city)
    {
        HideAllPanels();
        if (city != null && cityPanel != null)
        {
            cityPanel.SetActive(true);

            // Hide the global UI panel while the city panel is active.
            if (GlobalUIManager.Instance != null)
            {
                GlobalUIManager.Instance.HideGlobalUI();
            }
        }
    }

    public void ShowUnitPanel(Unit unit)
    {
        HideAllPanels();
        if (unit != null && unitPanel != null)
        {
            unitPanel.SetActive(true);
            unitNameText.text = unit.unitData.unitName;
            unitOwnerText.text = $"Owner: {unit.owner?.baseCiv?.civName ?? "None"}";
            unitDescriptionText.text = unit.unitData.description;
            unitStatsText.text = $"Movement: {unit.currentMovementPoints}/{unit.unitData.movementPoints}\n" +
                                $"Attack: {unit.unitData.baseAttack}\n" +
                                $"Defense: {unit.unitData.baseDefense}";

            bool isSettler = unit is SettlerUnit;
            if (moveButton != null) moveButton.gameObject.SetActive(true);
            if (attackButton != null) attackButton.gameObject.SetActive(!isSettler);
        }
    }

    public void HideAllPanels()
    {
        if (cityPanel != null) cityPanel.SetActive(false);
        if (unitPanel != null) unitPanel.SetActive(false);
    }
}