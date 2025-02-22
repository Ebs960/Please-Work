using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CityPanel : BasePanel
{
    [Header("City Info")]
    public TextMeshProUGUI cityNameText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI resourcesText;

    [Header("Production")]
    public Transform productionQueueContent;
    public Button addProductionButton;
    public TextMeshProUGUI currentProductionText;
    public Slider productionProgressSlider;
    public Transform unitOptionsContainer;
    public Transform buildingOptionsContainer;
    public Button productionButtonPrefab;

    private City currentCity;

    protected override void Awake()
    {
        base.Awake();
        if (addProductionButton != null)
            addProductionButton.onClick.AddListener(OpenProductionMenu);
    }

    public void SetCity(City city)
    {
        currentCity = city;
        if (currentCity == null)
        {
            Debug.LogError("SetCity called with a null City reference.");
            return;
        }
        Debug.Log("Setting city: " + currentCity.cityName);
        UpdateUI();
        PopulateProductionOptions();
    }

    public void UpdateUI()
    {
        if (currentCity == null) return;

        cityNameText.text = $"{currentCity.cityName}";
        populationText.text = $"Population: {currentCity.population}";

        resourcesText.text = $"Food: {currentCity.foodPerTurn}\n" +
                             $"Production: {currentCity.productionPerTurn}\n" +
                             $"Gold: {currentCity.goldPerTurn}\n" +
                             $"Science: {currentCity.sciencePerTurn}\n" +
                             $"Culture: {currentCity.culturePerTurn}";

        UpdateProductionUI();
        Debug.Log("UI updated for city: " + currentCity.cityName);
    }

    void UpdateProductionUI()
    {
        if (currentCity.currentProductionOption != null)
        {
            currentProductionText.text = $"Producing: {currentCity.currentProductionOption.productionName}";
            productionProgressSlider.maxValue = currentCity.currentProductionOption.productionCost;
            productionProgressSlider.value = currentCity.currentProductionProgress;
        }
        else
        {
            currentProductionText.text = "No production selected";
            productionProgressSlider.value = 0;
        }

        foreach (Transform child in productionQueueContent)
        {
            Destroy(child.gameObject);
        }

        foreach (ProductionOption option in currentCity.availableProductions)
        {
            GameObject productionItem = new GameObject(option.productionName);
            productionItem.transform.SetParent(productionQueueContent, false);

            TextMeshProUGUI productionText = productionItem.AddComponent<TextMeshProUGUI>();
            productionText.text = option.productionName;
            productionText.fontSize = 2;
            productionText.alignment = TextAlignmentOptions.Center;
        }
    }

    void OpenProductionMenu()
    {
        Debug.Log("Opening production menu...");
    }

    public void PopulateProductionOptions()
    {
        foreach (Transform child in unitOptionsContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in buildingOptionsContainer)
        {
            Destroy(child.gameObject);
        }

        if (currentCity == null || currentCity.owner == null)
        {
            Debug.LogError("City or its owner (CivilizationData) is null in CityPanel.");
            return;
        }

        foreach (var option in currentCity.owner.availableUnitProductionOptions)
        {
            Button btn = Instantiate(productionButtonPrefab, unitOptionsContainer);
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = option.productionName;
            }
            btn.onClick.AddListener(() =>
            {
                currentCity.SetProduction(option);
                UpdateUI();
            });
        }

        foreach (var option in currentCity.owner.availableBuildingProductionOptions)
        {
            Button btn = Instantiate(productionButtonPrefab, buildingOptionsContainer);
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = option.productionName;
            }
            btn.onClick.AddListener(() =>
            {
                currentCity.SetProduction(option);
                UpdateUI();
            });
        }
    }

    public void CloseCityUI()
    {
        gameObject.SetActive(false);
        if (GlobalUIManager.Instance != null)
        {
            GlobalUIManager.Instance.ShowGlobalUI();
        }
    }
}