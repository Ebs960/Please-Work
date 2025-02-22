using UnityEngine;

[CreateAssetMenu(fileName = "Settler", menuName = "CivGame/Units/Settler")]
public class SettlerData : UnitData
{
    void OnEnable()
    {
        unitName = "Settler";
        baseHealth = 5;
        baseAttack = 0;
        baseDefense = 1;
        baseMoveSpeed = 5f;
        baseRange = 1;
        unitClass = UnitClass.Settler;
        movementPoints = 2;
        attackPoints = 0;
        productionCost = 30;
        description = "Settlers can found new cities for your civilization.";

        // Movement capabilities
        canMoveOnSurface = true;
        canMoveOnWater = false;
        canMoveOnRivers = false;
        canMoveInSky = false;
        canMoveInMantle = false;
        canMoveInCore = false;
        canMoveInOrbit = false;
    }
} 