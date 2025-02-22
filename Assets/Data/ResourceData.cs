using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "CivGame/Resource")]
public class ResourceData : ScriptableObject
{
    [Header("Basic Info")]
    public string resourceName;
    public Sprite resourceIcon;
    [TextArea]
    public string description;

    [Header("Base Yields")]
    public int foodYield;
    public int productionYield;
    public int goldYield;
    public int scienceYield;
    public int cultureYield;

    [Header("Requirements")]
    public Technology requiredTechnology; // Technology needed to utilize this resource
    public UnitData unlockedUnit; // Unit that this resource enables (like Iron -> Swordsman)
} 