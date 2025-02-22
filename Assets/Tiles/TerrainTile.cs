using UnityEngine;

[System.Serializable]
public class TerrainTile : MonoBehaviour
{
    [Header("Terrain Properties")]
    public float moisture;
    public float temperature;
    public bool isRiver;
    public bool IsWaterTile;
    public float resourceDensity;
    public bool isSurface;
    public bool isMantle;
    public bool isSky; // Add this flag for the Sky layer
    public bool isCore;
    public bool isOrbit;
    public bool isMoon; // Add this with your other boolean flags

    // New property for lava (used in mantle/lava flow generation)
    public bool isLava = false;

    public string biomeType; // Add this new property

    [Header("Resources")]
    public ResourceData resource;
    public bool resourceRevealed = true; // Set to false if it needs tech to reveal

    [Header("Base Yields")]
    public int baseFood;
    public int baseProduction;
    public int baseGold;
    public int baseScience;
    public int baseCulture;

    // Add civilization ownership
    public CivilizationData owner;
    public Color originalColor;
    private Renderer tileRenderer;

    void Awake()
    {
        // Add BoxCollider if it doesn't exist
        if (GetComponent<BoxCollider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            // Adjust the collider size to match your tile size
            collider.size = new Vector3(1f, 0.1f, 1f); // Thin box for the surface
            collider.center = new Vector3(0f, 0f, 0f); // Center the collider
        }

        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            originalColor = tileRenderer.material.color;
        }

        // Set base yields based on terrain type
        SetBaseYields();
    }

    private void SetBaseYields()
    {
        if (isSurface)
        {
            if (IsWaterTile)
            {
                baseFood = 1;
                baseGold = 1;
            }
            else // Plains
            {
                baseFood = 2;
                baseProduction = 1;
            }

            if (isRiver)
            {
                baseGold += 1;
            }
        }
    }

    public (int food, int production, int gold, int science, int culture) GetTotalYields()
    {
        int totalFood = baseFood;
        int totalProduction = baseProduction;
        int totalGold = baseGold;
        int totalScience = baseScience;
        int totalCulture = baseCulture;

        if (resource != null && resourceRevealed)
        {
            totalFood += resource.foodYield;
            totalProduction += resource.productionYield;
            totalGold += resource.goldYield;
            totalScience += resource.scienceYield;
            totalCulture += resource.cultureYield;
        }

        return (totalFood, totalProduction, totalGold, totalScience, totalCulture);
    }

    public void SetOwner(CivilizationData newOwner)
    {
        owner = newOwner;
        if (tileRenderer != null && owner != null && owner.baseCiv != null)
        {
            tileRenderer.material.color = owner.baseCiv.civColor;
        }
        else
        {
            tileRenderer.material.color = originalColor;
        }
    }

    public string GetTerrainDescription()
    {
        string baseInfo = $"Biome: {biomeType}";

        // Add layer info
        string layer = isSurface ? "Surface" :
                      isMantle ? "Mantle" :
                      isSky ? "Sky" :
                      isCore ? "Core" :
                      isOrbit ? "Orbit" : "Unknown";
        baseInfo += $", Layer: {layer}";

        // Add type info
        string type = IsWaterTile ? "Water" : "Land";
        if (isRiver) type = "River";
        baseInfo += $", Type: {type}";

        // Add resource info if present
        if (resource != null && resourceRevealed)
        {
            baseInfo += $", Resource: {resource.resourceName}";
        }

        // Add yields on new line using GetTotalYields()
        var yields = GetTotalYields();
        string resourceInfo = $"Food: {yields.food}, Production: {yields.production}, Gold: {yields.gold}";
        if (yields.science > 0 || yields.culture > 0)
        {
            resourceInfo += $", Science: {yields.science}, Culture: {yields.culture}";
        }

        return $"{baseInfo}\n{resourceInfo}";
    }

    public string GetResourceDescription()
    {
        if (resource != null && resourceRevealed)
        {
            return $"{resource.resourceName}\n{resource.description}";
        }
        return "None";
    }

    public string GetYieldsDescription()
    {
        var yields = GetTotalYields();
        return $"Food: {yields.food}\n" +
               $"Production: {yields.production}\n" +
               $"Gold: {yields.gold}\n" +
               $"Science: {yields.science}\n" +
               $"Culture: {yields.culture}";
    }
}

// Example of a simple struct to represent tile yields.
public struct TileYields
{
    public int food;
    public int production;
    public int gold;
    public int science;
    public int culture;
}