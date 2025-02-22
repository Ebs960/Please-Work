using UnityEngine;

[CreateAssetMenu(menuName = "CivGame/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public int productionCost;
    public string description;

    // Extra yield bonuses provided by the building.
    public int extraFood;
    public int extraGold;
    public int extraScience;
    public int extraCulture;

    // Icon representing the building in the UI.
    public Sprite icon;
}