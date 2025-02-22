using UnityEngine;

[CreateAssetMenu(menuName = "CivGame/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Basic Info")]
    public string unitName;
    public int productionCost;
    public GameObject unitPrefab;
    [TextArea]
    public string description;

    [Header("Base Stats")]
    public int baseHealth = 10;
    public int baseAttack = 1;
    public int baseDefense = 0;
    public float baseMoveSpeed = 5f;
    public int baseRange = 1;
    public UnitClass unitClass = UnitClass.Combat;

    [Header("Movement")]
    public int movementPoints = 2;
    public int attackPoints = 1;

    [Header("Movement Capabilities")]
    public bool canMoveOnSurface = true;
    public bool canMoveOnWater = false;
    public bool canMoveOnRivers = false;
    public bool canMoveInSky = false;
    public bool canMoveInMantle = false;
    public bool canMoveInCore = false;
    public bool canMoveInOrbit = false;

    // Icon representing the unit in the UI.
    public Sprite icon;
}

public enum UnitClass
{
    Worker,
    Settler,
    Combat,
    Animal
}