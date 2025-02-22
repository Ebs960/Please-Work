using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Civilization", menuName = "Civilization")]
public class Civilization : ScriptableObject
{
    [Header("Basic Info")]
    public string civName;
    public Color civColor;

    [Header("Visuals")]
    public GameObject leaderPrefab;
    public GameObject cityPrefab;
    public GameObject unitPrefab;

    [Header("Bonus Attributes")]
    public int bonusGold;
    public int bonusProduction;
    public int bonusScience;
    public int bonusCulture;
    public int bonusFood;

    [Header("City Naming")]
    public string[] cityNames;

    [Header("Units")]
    public List<UnitData> availableUnits = new List<UnitData>();

    [Header("Buildings")]
    public List<BuildingData> availableBuildings = new List<BuildingData>();
}