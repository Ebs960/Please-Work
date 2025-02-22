using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanel : BasePanel
{
    [Header("Unit Info")]
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI ownerText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI movementText;

    [Header("Actions")]
    public Button moveButton;
    public Button attackButton;
    public Button foundCityButton;

    private Unit currentUnit;

    protected override void Awake()
    {
        base.Awake();
        SetupButtons();
    }

    void SetupButtons()
    {
        if (moveButton != null)
            moveButton.onClick.AddListener(() => Debug.Log("Move mode activated"));
        if (attackButton != null)
            attackButton.onClick.AddListener(() => Debug.Log("Attack mode activated"));
        if (foundCityButton != null)
            foundCityButton.onClick.AddListener(() => Debug.Log("Found city mode activated"));
    }

    public void SetUnit(Unit unit)
    {
        currentUnit = unit;
        UpdateUI();
        
        // Show/hide buttons based on unit type
        if (foundCityButton != null)
            foundCityButton.gameObject.SetActive(unit is SettlerUnit);
    }

    public void UpdateUI()
    {
        if (currentUnit == null) return;

        unitNameText.text = currentUnit.unitData.unitName;
        ownerText.text = $"Owner: {currentUnit.owner?.baseCiv?.civName ?? "None"}";
        
        statsText.text = $"Attack: {currentUnit.unitData.baseAttack}\n" +
                        $"Defense: {currentUnit.unitData.baseDefense}\n" +
                        $"Health: {currentUnit.unitData.baseHealth}";
                        
        movementText.text = $"Movement: {currentUnit.currentMovementPoints}/{currentUnit.unitData.movementPoints}";
    }
} 