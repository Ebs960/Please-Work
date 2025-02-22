using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CivilizationData : MonoBehaviour
{
    public Civilization baseCiv;
    public int gold;
    public int food;
    public int sciencePerTurn;
    public int culturePerTurn;
    public int personalTurnCount;

    public List<ProductionOption> availableUnitProductionOptions = new List<ProductionOption>();
    public List<ProductionOption> availableBuildingProductionOptions = new List<ProductionOption>();
    public int cityCount;

    public Technology currentTech;
    public int researchCost;
    public float currentResearchProgress;

    public List<Technology> availableTechnologies = new List<Technology>();

    public new string name
    {
        get { return baseCiv != null ? baseCiv.civName : "Unknown Civilization"; }
    }

    public int turnNumber
    {
        get { return personalTurnCount; }
    }
}