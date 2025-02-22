using UnityEngine;

public class CavemanUnit : Unit
{
    private int extraAttackFromTech = 0;

    protected override void Start()
    {
        base.Start();
        CheckTechnologyBonuses();
    }

    private void CheckTechnologyBonuses()
    {
        if (owner != null)
        {
            // Check for Sharp Rocks technology
            foreach (Technology tech in owner.availableTechnologies)
            {
                if (tech.techName == "Sharp Rocks")
                {
                    extraAttackFromTech = 2;
                    break;
                }
            }
        }
    }

    public int GetAttack()
    {
        return unitData.baseAttack + extraAttackFromTech;
    }
} 