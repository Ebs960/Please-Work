// 2/20/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class City : MonoBehaviour
{
    [Header("City Info")]
    public string cityName;
    public CivilizationData owner;
    public int population = 1;

    [Header("Base Resource Yields (Per Population)")]
    public int baseFoodPerPop = 2;
    public int baseProductionPerPop = 1;
    public int baseGoldPerPop = 1;
    public int baseSciencePerPop = 1;
    public int baseCulturePerPop = 1;

    [Header("Resources Per Turn")]
    public int foodPerTurn;
    public int productionPerTurn;
    public int goldPerTurn;
    public int sciencePerTurn;
    public int culturePerTurn;

    [Header("Completed Buildings")]
    public List<ProductionOption> completedBuildings = new List<ProductionOption>();

    [Header("Production")]
    public ProductionOption currentProductionOption;
    public int currentProductionProgress = 0;
    public List<ProductionOption> availableProductions = new List<ProductionOption>();

    [Header("UI & Floating Label")]
    public TextMeshPro nameLabel;
    public GameObject labelPrefab;

    void Start()
    {
        if (labelPrefab != null && nameLabel == null)
        {
            GameObject label = Instantiate(labelPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            label.transform.SetParent(transform);
            nameLabel = label.GetComponent<TextMeshPro>();
        }
        UpdateCityLabel();
        CalculateResourceGeneration();
        InitializeProductionOptions();
    }

    void Update()
    {
        if (nameLabel != null && Camera.main != null)
        {
            nameLabel.transform.rotation = Camera.main.transform.rotation;
        }
    }

    void UpdateCityLabel()
    {
        if (nameLabel != null)
        {
            nameLabel.text = $"{cityName}\n{(owner != null ? owner.name : "No Owner")}";
        }
    }

    public void CalculateResourceGeneration()
    {
        foodPerTurn = population * baseFoodPerPop;
        productionPerTurn = population * baseProductionPerPop;
        goldPerTurn = population * baseGoldPerPop;
        sciencePerTurn = population * baseSciencePerPop;
        culturePerTurn = population * baseCulturePerPop;

        foreach (ProductionOption building in completedBuildings)
        {
            if (building.buildingData != null)
            {
                foodPerTurn += building.buildingData.extraFood;
                goldPerTurn += building.buildingData.extraGold;
                sciencePerTurn += building.buildingData.extraScience;
                culturePerTurn += building.buildingData.extraCulture;
            }
        }
    }

    public void GenerateResources()
    {
        CalculateResourceGeneration();
        if (owner != null)
        {
            owner.food += foodPerTurn;
            owner.gold += goldPerTurn;
            owner.sciencePerTurn += sciencePerTurn;
            owner.culturePerTurn += culturePerTurn;
        }
    }

    public void SetProduction(ProductionOption production)
    {
        currentProductionOption = production;
        currentProductionProgress = 0;
    }

    public int GetProductionPerTurn()
    {
        return productionPerTurn;
    }

    public void CompleteProduction()
    {
        if (currentProductionOption != null)
        {
            if (currentProductionOption.productionType == ProductionType.Unit)
            {
                if (currentProductionOption.prefab != null)
                {
                    Vector3 spawnPos = FindValidSpawnPosition();
                    GameObject producedInstance = Instantiate(currentProductionOption.prefab, spawnPos, Quaternion.identity);
                    Unit unitComponent = producedInstance.GetComponent<Unit>();
                    if (unitComponent != null)
                    {
                        unitComponent.owner = owner;
                    }
                }
                else
                {
                    Debug.LogError("Production option prefab is null for unit: " + currentProductionOption.productionName);
                }
            }
            else if (currentProductionOption.productionType == ProductionType.Building)
            {
                completedBuildings.Add(currentProductionOption);
                CalculateResourceGeneration();
            }
        }
        currentProductionProgress = 0;
        currentProductionOption = null;
    }

    public void UpdateProduction()
    {
        if (currentProductionOption != null)
        {
            currentProductionProgress += GetProductionPerTurn();
            if (currentProductionProgress >= currentProductionOption.productionCost)
            {
                CompleteProduction();
            }
        }
    }

    private Vector3 FindValidSpawnPosition()
    {
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right,
            new Vector3(1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 0, -1)
        };

        foreach (Vector3 dir in directions)
        {
            Vector3 candidate = transform.position + dir;
            Collider[] colliders = Physics.OverlapSphere(candidate, 0.3f);
            if (colliders.Length == 0)
                return candidate;
        }
        return transform.position;
    }

    private void InitializeProductionOptions()
    {
        if (owner != null && owner.baseCiv != null)
        {
            availableProductions.Clear();
            availableProductions.AddRange(owner.baseCiv.availableUnits.Select(unitData => new ProductionOption
            {
                productionName = unitData.unitName,
                productionCost = unitData.productionCost,
                prefab = unitData.unitPrefab,
                unitData = unitData,
                productionType = ProductionType.Unit,
                icon = unitData.icon // Assuming UnitData has an icon field
            }));

            availableProductions.AddRange(owner.baseCiv.availableBuildings.Select(buildingData => new ProductionOption
            {
                productionName = buildingData.buildingName,
                productionCost = buildingData.productionCost,
                buildingData = buildingData,
                productionType = ProductionType.Building,
                icon = buildingData.icon // Assuming BuildingData has an icon field
            }));
        }
    }
}