using UnityEngine;

[CreateAssetMenu(fileName = "Caveman", menuName = "CivGame/Units/Caveman")]
public class CavemanData : UnitData
{
    void OnEnable()
    {
        unitName = "Caveman";
        baseHealth = 10;
        baseAttack = 3;
        baseDefense = 1;
        baseMoveSpeed = 5f;
        baseRange = 1;
        unitClass = UnitClass.Combat;
        movementPoints = 2;
        attackPoints = 1;
        productionCost = 20;
        description = "Basic early-game warrior unit. Gets stronger with technological advances.";
    }
} 