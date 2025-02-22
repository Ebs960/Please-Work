using UnityEngine;

public enum ProductionType
{
    Unit,
    Building
}

[System.Serializable]
public class ProductionOption
{
    public string productionName;
    public int productionCost;

    // For unit production, these fields are used.
    public GameObject prefab;
    public UnitData unitData;

    // For building production, you can assign building-specific data.
    public BuildingData buildingData;

    public Sprite icon;
    public ProductionType productionType;
}

