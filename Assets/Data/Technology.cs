using UnityEngine;

[CreateAssetMenu(fileName = "New Technology", menuName = "CivGame/Technology")]
public class Technology : ScriptableObject
{
    public string techName;
    public TechnologyAge techAge;
    public int researchCost;
    public string description;
} 