using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CivilizationManager : MonoBehaviour
{
    public static CivilizationManager Instance;

    public List<Civilization> activeCivilizations = new List<Civilization>();
    public List<CivilizationData> civilizationDataInstances = new List<CivilizationData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CivilizationManager initialized");
            InitializeCivilizations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeCivilizations()
    {
        Debug.Log($"Initializing {activeCivilizations.Count} civilizations");
        foreach (Civilization civ in activeCivilizations)
        {
            if (civ != null)
            {
                GameObject civObject = new GameObject(civ.civName);
                CivilizationData civData = civObject.AddComponent<CivilizationData>();
                civData.baseCiv = civ;
                civData.gold = civ.bonusGold;
                civData.food = civ.bonusFood;
                civData.sciencePerTurn = civ.bonusScience;
                civData.culturePerTurn = civ.bonusCulture;

                // Initialize available unit and building data
                civData.availableUnitProductionOptions.AddRange(civ.availableUnits.Select(unitData => new ProductionOption
                {
                    productionName = unitData.unitName,
                    productionCost = unitData.productionCost,
                    prefab = unitData.unitPrefab,
                    unitData = unitData,
                    productionType = ProductionType.Unit,
                    icon = unitData.icon // Assuming UnitData has an icon field
                }));
                civData.availableBuildingProductionOptions.AddRange(civ.availableBuildings.Select(buildingData => new ProductionOption
                {
                    productionName = buildingData.buildingName,
                    productionCost = buildingData.productionCost,
                    buildingData = buildingData,
                    productionType = ProductionType.Building,
                    icon = buildingData.icon // Assuming BuildingData has an icon field
                }));
                civilizationDataInstances.Add(civData);
                Debug.Log($"Initialized civilization: {civ.civName}");
            }
        }
    }

    public CivilizationData GetCivilizationData(Civilization civ)
    {
        return civilizationDataInstances.Find(cd => cd.baseCiv == civ);
    }

    public void AddResources(CivilizationData civData, float gold, float food, float science, float culture)
    {
        if (civData != null)
        {
            civData.gold += (int)gold;
            civData.food += (int)food;
            civData.sciencePerTurn = (int)science;
            civData.culturePerTurn = (int)culture;
        }
    }
}