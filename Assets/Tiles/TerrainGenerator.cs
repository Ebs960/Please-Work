using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TerrainGenerator : MonoBehaviour
{
    //-------------------------------------------------------------------------
    // Surface Noise Settings (remains unchanged)
    //-------------------------------------------------------------------------
    public float heightNoiseScale = 0.1f;
    public float minElevation = 2f;
    public float maxElevation = 10f;
    public float heightBias = 1.0f;
    public float moistureNoiseScale = 0.1f;
    public float temperatureNoiseScale = 0.1f;
    public float minTemperature = 0f;
    public float maxTemperature = 1f;
    public float hotTemperatureThreshold = 0.7f;
    public float coldTemperatureThreshold = 0.3f;
    public float minMoisture = 0f;
    public float maxMoisture = 1f;
    public float wetMoistureThreshold = 0.7f;
    public float dryMoistureThreshold = 0.3f;
    public int numberOfRivers = 3;

    // Offsets for the surface noise
    private float noiseOffsetX;
    private float noiseOffsetZ;

    //-------------------------------------------------------------------------
    // Grid settings
    //-------------------------------------------------------------------------
    public int width = 30;
    public int depth = 30;
    public float tileSize = 1f;

    //-------------------------------------------------------------------------
    // Prefabs for different biomes / layers
    //-------------------------------------------------------------------------
    public GameObject waterPrefab;
    public GameObject riverPrefab;
    public GameObject plainsPrefab;
    public GameObject hillsPrefab;
    public GameObject mountainsPrefab;
    public GameObject desertPrefab;
    public GameObject tundraPrefab;
    public GameObject taigaPrefab;
    public GameObject tropicalPrefab;
    public GameObject aridPrefab;
    public GameObject alpinePrefab;
    public GameObject mantlePrefab;    // Mantle layer prefab
    public GameObject skyPrefab;       // Sky layer prefab
    public GameObject grasslandPrefab; // Grassland biome prefab
    public GameObject arcticPrefab;    // Arctic biome prefab
    public GameObject seasPrefab;      // Seas biome prefab
    public GameObject solidCorePrefab; // Core layer prefab
    public GameObject orbitPrefab;     // Orbit layer prefab
    public GameObject lavaPrefab;      // For Lava flows (a type of mantle terrain)

    //-------------------------------------------------------------------------
    // Tribe and Civilization Settings
    //-------------------------------------------------------------------------
    public GameObject tribePrefab;     // Prefab for tribe units
    public int numberOfTribes = 5;
    public float cityYOffset = 1.0f;
    public string[] tribalNames = new string[]
    {
        "Apache", "Sioux", "Iroquois", "Cheyenne", "Comanche", "Navajo",
        "Cherokee", "Seminole", "Blackfoot", "Mohawk"
    };
    public Vector2 populationRange = new Vector2(50, 300);
    public int defense = 5;
    public int production = 3;
    public int gold = 10;
    public int food = 10;
    public int science = 2;
    public int culture = 2;
    public int manpower = 5;

    public float hillHeightThreshold = 5f;
    public float mountainHeightThreshold = 8f;
    public float alpineHeightThreshold = 6f;

    public float seasHeightThreshold = -0.5f; // Below this, it's seas.
    public float waterHeightThreshold = 0f;   // Up to this, it's water.

    //-------------------------------------------------------------------------
    // Tile Arrays for Each Layer
    //-------------------------------------------------------------------------
    public GameObject[,] surfaceTiles;
    public GameObject[,] mantleTiles;
    public GameObject[,] skyTiles;
    public GameObject[,] coreTiles;
    public GameObject[,] orbitTiles;

    public TMP_Text floatingText; // For displaying tile info
    private GameObject selectedTile;

    [Header("Managers")]
    public CivilizationManager civilizationManager;

    [Header("Resources")]
    public List<ResourceData> availableResources;
    [Range(0, 100)]
    public float resourceSpawnChance = 15f;

    [Header("Tribe Settings")]
    public float minDistanceBetweenTribes = 10f;

    //-------------------------------------------------------------------------
    // NEW: Independent Layer Parameters
    //-------------------------------------------------------------------------
    // Mantle Layer
    public float mantleBaseHeight = -8f;
    public float mantleNoiseScale = 0.1f;
    public float mantleNoiseStrength = 1.0f;
    private float mantleNoiseOffsetX;
    private float mantleNoiseOffsetZ;

    // Core Layer
    public float coreBaseHeight = -16f;
    public float coreNoiseScale = 0.1f;
    public float coreNoiseStrength = 1.0f;
    private float coreNoiseOffsetX;
    private float coreNoiseOffsetZ;

    // Sky Layer – lowest sky block is y = 15
    public float skyBaseHeight = 15f;
    public float skyNoiseScale = 0.1f;
    public float skyNoiseStrength = 1.0f;
    private float skyNoiseOffsetX;
    private float skyNoiseOffsetZ;

    // Orbit Layer – lowest orbit block is y = 30
    public float orbitBaseHeight = 30f;
    public float orbitNoiseScale = 0.1f;
    public float orbitNoiseStrength = 1.0f;
    private float orbitNoiseOffsetX;
    private float orbitNoiseOffsetZ;

    // Moon Layer
    public float orbitLayerHeight = 30f;         // Orbit layer's flat Y value.
    public float moonHeightOffsetFromOrbit = 25f;  // Moon sits 25 blocks higher than orbit.
    public float moonNoiseScale = 0.1f;            // Adjust to change the "zoom" of the noise.
    public float moonNoiseStrength = 5f;           // Controls noise variation in elevation.
    public float moonCaveThreshold = 0.3f;         // Below this, Moon Caves.
    public float moonXOffset = 25f;                // Shifts the Moon to the left.
    public float moonNoiseOffsetX = 0f;            // Random offset for noise (optional).
    public float moonNoiseOffsetZ = 0f;            // Random offset for noise (optional).

    public GameObject moonDunesPrefab;   // Prefab for Moon Dunes.
    public GameObject moonCavesPrefab;   // Prefab for Moon Caves.
    public GameObject[,] moonTiles;      // Container for Moon layer tiles.

    //-------------------------------------------------------------------------
    // Initialization: Create independent noise offsets for each layer.
    //-------------------------------------------------------------------------
    void Start()
    {
        if (civilizationManager == null)
        {
            Debug.LogError("CivilizationManager not assigned!");
            return;
        }

        noiseOffsetX = Random.Range(0f, 1000f);
        noiseOffsetZ = Random.Range(0f, 1000f);

        mantleNoiseOffsetX = Random.Range(0f, 1000f);
        mantleNoiseOffsetZ = Random.Range(0f, 1000f);

        coreNoiseOffsetX = Random.Range(0f, 1000f);
        coreNoiseOffsetZ = Random.Range(0f, 1000f);

        skyNoiseOffsetX = Random.Range(0f, 1000f);
        skyNoiseOffsetZ = Random.Range(0f, 1000f);

        orbitNoiseOffsetX = Random.Range(0f, 1000f);
        orbitNoiseOffsetZ = Random.Range(0f, 1000f);

        // Initialize arrays
        surfaceTiles = new GameObject[width, depth];
        mantleTiles = new GameObject[width, depth];
        skyTiles = new GameObject[width, depth];
        coreTiles = new GameObject[width, depth];
        orbitTiles = new GameObject[width, depth];

        // Generate Layers Independently – each layer's height is determined by its base height plus its own noise variation.
        GenerateSurfaceTerrain();
        GenerateMantleTerrain();
        GenerateCoreTerrain();
        GenerateSkyTerrain();
        GenerateOrbitTerrain();

        GenerateRivers();
        GenerateLavaFlows();

        SpawnResources();
        SpawnTribes();
        SpawnInitialSettlers();

        GenerateMoonTerrain();
    }

    //-------------------------------------------------------------------------
    // Update – Handles Tile Selection and Displaying Info
    //-------------------------------------------------------------------------
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TerrainTile tile = hit.collider.gameObject.GetComponent<TerrainTile>();
            if (tile != null)
            {
                Vector2 tilePos = new Vector2(hit.collider.gameObject.transform.position.x,
                                              hit.collider.gameObject.transform.position.z);
                string biome = DetermineBiome(hit.collider.gameObject);
                string layer = tile.isSurface ? "Surface" :
                               tile.isMantle ? "Mantle" :
                               tile.isSky ? "Sky" :
                               tile.isCore ? "Core" :
                               tile.isOrbit ? "Orbit" : "Unknown";

                string type = DetermineType(hit.collider.gameObject);
                var yields = tile.GetTotalYields();
                string yieldInfo = $"Food: {yields.food}, Production: {yields.production}, Gold: {yields.gold}";
                if (yields.science > 0 || yields.culture > 0)
                {
                    yieldInfo += $", Science: {yields.science}, Culture: {yields.culture}";
                }

                floatingText.text = $"Position: ({tilePos.x}, {tilePos.y})\nBiome: {biome}\nLayer: {layer}\nType: {type}\n{yieldInfo}";

                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedTile != null)
                    {
                        ResetTileColor(selectedTile);
                    }
                    selectedTile = selectedTile == hit.collider.gameObject ? null : hit.collider.gameObject;
                    if (selectedTile != null)
                    {
                        HighlightTile(selectedTile);
                    }
                }
            }
        }
        else
        {
            floatingText.text = "";
        }
    }

    private void HighlightTile(GameObject tile)
    {
        TerrainTile tt = tile.GetComponent<TerrainTile>();
        if (tt.isSky)
            tile.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow);
        else
            tile.GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void ResetTileColor(GameObject tile)
    {
        TerrainTile tt = tile.GetComponent<TerrainTile>();
        if (tt.isSky)
            tile.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        else
            tile.GetComponent<Renderer>().material.color = Color.white;
    }

    private string DetermineBiome(GameObject tile)
    {
        TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
        if (terrainTile != null)
        {
            if (terrainTile.isOrbit) return "Orbit";
            if (terrainTile.isSky) return "Sky";
            if (terrainTile.isMantle) return "Mantle";
            if (terrainTile.isCore) return "Core";
            if (terrainTile.isSurface) return terrainTile.biomeType;
        }
        return "Unknown";
    }

    private string DetermineType(GameObject tile)
    {
        string tileName = tile.name.ToLower();
        if (tileName.Contains("river"))
            return "River";
        if (tileName.Contains("water") || tileName.Contains("seas"))
            return "Water";
        return "Land";
    }

    //-------------------------------------------------------------------------
    // Surface Generation (unchanged; uses its own noise)
    //-------------------------------------------------------------------------
    float GenerateHeight(int x, int z)
    {
        float xCoord = ((float)x + noiseOffsetX) / width * heightNoiseScale;
        float zCoord = ((float)z + noiseOffsetZ) / depth * heightNoiseScale;
        float perlin = Mathf.PerlinNoise(xCoord, zCoord);
        perlin = Mathf.Pow(perlin, heightBias);
        float h = Mathf.Lerp(minElevation, maxElevation, perlin);
        return Mathf.Round(h * 5f) / 5f;
    }

    void GenerateSurfaceTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float heightValue = GenerateHeight(x, z);
                float moistureValue = Mathf.Lerp(minMoisture, maxMoisture,
                    Mathf.PerlinNoise(((float)x + noiseOffsetX) / width * moistureNoiseScale, ((float)z + noiseOffsetZ) / depth * moistureNoiseScale));
                float temperatureValue = Mathf.Lerp(minTemperature, maxTemperature,
                    Mathf.PerlinNoise(((float)x + noiseOffsetX) / width * temperatureNoiseScale, ((float)z + noiseOffsetZ) / depth * temperatureNoiseScale));
                GameObject prefab = GetBiomePrefab(heightValue, moistureValue, temperatureValue);
                if (prefab != null)
                {
                    Vector3 pos = new Vector3(x * tileSize, heightValue, z * tileSize);
                    GameObject tile = Instantiate(prefab, pos, Quaternion.identity);
                    tile.name = $"Surface_Tile_{x}_{z}";
                    TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                    terrainTile.isSurface = true;

                    if (prefab == waterPrefab) terrainTile.biomeType = "Water";
                    else if (prefab == riverPrefab) terrainTile.biomeType = "River";
                    else if (prefab == plainsPrefab) terrainTile.biomeType = "Plains";
                    else if (prefab == hillsPrefab) terrainTile.biomeType = "Hills";
                    else if (prefab == mountainsPrefab) terrainTile.biomeType = "Mountains";
                    else if (prefab == desertPrefab) terrainTile.biomeType = "Desert";
                    else if (prefab == tundraPrefab) terrainTile.biomeType = "Tundra";
                    else if (prefab == taigaPrefab) terrainTile.biomeType = "Taiga";
                    else if (prefab == tropicalPrefab) terrainTile.biomeType = "Tropical";
                    else if (prefab == aridPrefab) terrainTile.biomeType = "Arid";
                    else if (prefab == alpinePrefab) terrainTile.biomeType = "Alpine";
                    else if (prefab == grasslandPrefab) terrainTile.biomeType = "Grassland";
                    else if (prefab == arcticPrefab) terrainTile.biomeType = "Arctic";
                    else if (prefab == seasPrefab) terrainTile.biomeType = "Seas";

                    surfaceTiles[x, z] = tile;
                    ApplyClimateEffects(tile, temperatureValue, moistureValue);
                }
            }
        }
    }

    //-------------------------------------------------------------------------
    // New Helper: Independently generate a layer's height.
    //-------------------------------------------------------------------------
    float GenerateLayerHeight(float baseHeight, float noiseScale, float noiseStrength, int x, int z, float offsetX, float offsetZ)
    {
        float noise = Mathf.PerlinNoise((x + offsetX) * noiseScale, (z + offsetZ) * noiseScale);
        return baseHeight + noise * noiseStrength;
    }

    //-------------------------------------------------------------------------
    // Generate Mantle Terrain Independently
    //-------------------------------------------------------------------------
    void GenerateMantleTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 pos = new Vector3(x * tileSize,
                    GenerateLayerHeight(mantleBaseHeight, mantleNoiseScale, mantleNoiseStrength, x, z, mantleNoiseOffsetX, mantleNoiseOffsetZ),
                    z * tileSize);
                GameObject tile = Instantiate(mantlePrefab, pos, Quaternion.identity);
                tile.name = $"Mantle_Tile_{x}_{z}";
                TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                terrainTile.isMantle = true;
                terrainTile.biomeType = "Mantle";
                mantleTiles[x, z] = tile;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Generate Core Terrain Independently
    //-------------------------------------------------------------------------
    void GenerateCoreTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 pos = new Vector3(x * tileSize,
                    GenerateLayerHeight(coreBaseHeight, coreNoiseScale, coreNoiseStrength, x, z, coreNoiseOffsetX, coreNoiseOffsetZ),
                    z * tileSize);
                GameObject tile = Instantiate(solidCorePrefab, pos, Quaternion.identity);
                tile.name = $"Core_Tile_{x}_{z}";
                TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                terrainTile.isCore = true;
                terrainTile.biomeType = "Core";
                coreTiles[x, z] = tile;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Generate Sky Terrain Independently – lowest sky block is at y = skyBaseHeight plus noise
    //-------------------------------------------------------------------------
    void GenerateSkyTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 pos = new Vector3(x * tileSize,
                    GenerateLayerHeight(skyBaseHeight, skyNoiseScale, skyNoiseStrength, x, z, skyNoiseOffsetX, skyNoiseOffsetZ),
                    z * tileSize);
                GameObject tile = Instantiate(skyPrefab, pos, Quaternion.identity);
                tile.name = $"Sky_Tile_{x}_{z}";
                TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                terrainTile.isSky = true;
                terrainTile.biomeType = "Sky";
                skyTiles[x, z] = tile;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Generate Orbit Terrain Independently – lowest orbit block is at y = orbitBaseHeight plus noise
    //-------------------------------------------------------------------------
    void GenerateOrbitTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 pos = new Vector3(x * tileSize,
                    GenerateLayerHeight(orbitBaseHeight, orbitNoiseScale, orbitNoiseStrength, x, z, orbitNoiseOffsetX, orbitNoiseOffsetZ),
                    z * tileSize);
                GameObject tile = Instantiate(orbitPrefab, pos, Quaternion.identity);
                tile.name = $"Orbit_Tile_{x}_{z}";
                TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                terrainTile.isOrbit = true;
                terrainTile.biomeType = "Orbit";
                orbitTiles[x, z] = tile;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Remaining functions: SpawnInitialSettlers(), SpawnResources(), SpawnTribes(), ApplyClimateEffects(), GetBiomePrefab()
    //-------------------------------------------------------------------------
    void SpawnInitialSettlers()
    {
        if (civilizationManager == null)
        {
            Debug.LogError("No CivilizationManager assigned to TerrainGenerator!");
            return;
        }

        Debug.Log($"Attempting to spawn settlers for {civilizationManager.civilizationDataInstances.Count} civilizations");
        foreach (CivilizationData civData in civilizationManager.civilizationDataInstances)
        {
            if (civData == null || civData.baseCiv == null)
            {
                Debug.LogError("Invalid CivilizationData!");
                continue;
            }
            List<Vector2Int> validTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (surfaceTiles[x, z] != null)
                    {
                        TerrainTile tile = surfaceTiles[x, z].GetComponent<TerrainTile>();
                        if (tile != null && tile.isSurface && !tile.IsWaterTile)
                        {
                            validTiles.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
            if (validTiles.Count > 0)
            {
                Vector2Int spawnTile = validTiles[Random.Range(0, validTiles.Count)];
                TerrainTile tile = surfaceTiles[spawnTile.x, spawnTile.y].GetComponent<TerrainTile>();
                Vector3 spawnPos = new Vector3(spawnTile.x * tileSize, tile.transform.position.y, spawnTile.y * tileSize);
                if (civData.baseCiv.unitPrefab != null)
                {
                    GameObject settlerObj = Instantiate(civData.baseCiv.unitPrefab, spawnPos, Quaternion.identity);
                    SettlerUnit settler = settlerObj.GetComponent<SettlerUnit>();
                    if (settler != null)
                    {
                        settler.owner = civData;
                        settlerObj.name = $"Settler_{civData.baseCiv.civName}";
                        Debug.Log($"Spawned settler for {civData.baseCiv.civName} at position {spawnPos}");
                    }
                }
            }
        }
    }

    void SpawnResources()
    {
        if (availableResources == null || availableResources.Count == 0)
            return;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (surfaceTiles[x, z] != null)
                {
                    TerrainTile tile = surfaceTiles[x, z].GetComponent<TerrainTile>();
                    if (tile != null && tile.isSurface && !tile.IsWaterTile)
                    {
                        if (Random.Range(0f, 100f) < resourceSpawnChance)
                        {
                            tile.resource = availableResources[Random.Range(0, availableResources.Count)];
                            Debug.Log($"Spawned {tile.resource.resourceName} at ({x},{z})");
                        }
                    }
                }
            }
        }
    }

    void SpawnTribes()
    {
        Debug.Log($"Starting tribe spawn process. Attempting to spawn {numberOfTribes} tribes.");
        List<Vector3> spawnedPositions = new List<Vector3>();
        for (int i = 0; i < numberOfTribes; i++)
        {
            List<Vector2Int> validTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (surfaceTiles[x, z] != null)
                    {
                        TerrainTile tile = surfaceTiles[x, z].GetComponent<TerrainTile>();
                        if (tile != null && tile.isSurface && !tile.IsWaterTile)
                        {
                            Vector3 pos = new Vector3(x * tileSize, tile.transform.position.y, z * tileSize);
                            bool tooClose = false;
                            foreach (Vector3 exPos in spawnedPositions)
                            {
                                if (Vector3.Distance(pos, exPos) < minDistanceBetweenTribes)
                                {
                                    tooClose = true;
                                    break;
                                }
                            }
                            if (!tooClose)
                                validTiles.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
            if (validTiles.Count > 0)
            {
                Vector2Int spawnTile = validTiles[Random.Range(0, validTiles.Count)];
                TerrainTile tile = surfaceTiles[spawnTile.x, spawnTile.y].GetComponent<TerrainTile>();
                Vector3 spawnPos = new Vector3(spawnTile.x * tileSize, tile.transform.position.y + 0.5f, spawnTile.y * tileSize);
                if (tribePrefab != null)
                {
                    GameObject tribe = Instantiate(tribePrefab, spawnPos, Quaternion.identity);
                    tribe.name = $"Tribe_{i}";
                    spawnedPositions.Add(spawnPos);
                    Debug.Log($"Spawned tribe at position {spawnPos}");
                }
            }
        }
    }

    void ApplyClimateEffects(GameObject tile, float temperature, float moisture)
    {
        Material mat = tile.GetComponent<Renderer>().material;
        Color baseColor = mat.color;
        float redValue = Mathf.Lerp(0.5f, 1f, temperature);
        float blueValue = Mathf.Lerp(0.5f, 1f, moisture);
        mat.color = new Color(redValue, baseColor.g, blueValue, baseColor.a);
    }

    private GameObject GetBiomePrefab(float height, float moisture, float temperature)
    {
        if (height < seasHeightThreshold)
            return seasPrefab;
        else if (height <= waterHeightThreshold)
            return waterPrefab;

        if (height < hillHeightThreshold)
        {
            if (temperature > hotTemperatureThreshold)
                return (moisture > wetMoistureThreshold) ? tropicalPrefab : desertPrefab;
            else if (temperature < coldTemperatureThreshold)
                return (temperature < 0.2f) ? arcticPrefab : tundraPrefab;
            else
                return (moisture > wetMoistureThreshold) ? grasslandPrefab : plainsPrefab;
        }
        else if (height < mountainHeightThreshold)
            return hillsPrefab;
        else if (height < alpineHeightThreshold)
            return mountainsPrefab;
        else
            return alpinePrefab;
    }

    //-------------------------------------------------------------------------
    // Updated GenerateRivers() method with a longer length limit.
    //-------------------------------------------------------------------------
    void GenerateRivers()
    {
        float tolerance = 0.05f;
        int riverLengthLimit = 200; // Increased length limit

        for (int i = 0; i < numberOfRivers; i++)
        {
            List<Vector2Int> validStarts = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (surfaceTiles[x, z] != null)
                    {
                        TerrainTile tile = surfaceTiles[x, z].GetComponent<TerrainTile>();
                        if (tile != null && tile.isSurface &&
                            tile.biomeType != "Water" &&
                            tile.biomeType != "Seas" &&
                            tile.biomeType != "River")
                        {
                            validStarts.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
            if (validStarts.Count == 0)
                break;

            Vector2Int start = validStarts[Random.Range(0, validStarts.Count)];
            Vector2Int current = start;
            int length = 0;
            while (length < riverLengthLimit)
            {
                length++;
                TerrainTile currentTile = surfaceTiles[current.x, current.y].GetComponent<TerrainTile>();
                if (currentTile == null)
                    break;

                if (currentTile.biomeType != "Water" && currentTile.biomeType != "Seas")
                {
                    Vector3 pos = currentTile.transform.position;
                    Destroy(surfaceTiles[current.x, current.y]);
                    GameObject riverTile = Instantiate(riverPrefab, pos, Quaternion.identity);
                    riverTile.name = $"River_Tile_{current.x}_{current.y}";
                    TerrainTile newTile = riverTile.GetComponent<TerrainTile>();
                    newTile.isSurface = true;
                    newTile.biomeType = "River";
                    surfaceTiles[current.x, current.y] = riverTile;
                }

                float currentHeight = currentTile.transform.position.y;
                Vector2Int next = current;
                float lowestHeight = currentHeight;
                bool foundLower = false;
                List<Vector2Int> equalCandidates = new List<Vector2Int>();

                Vector2Int[] directions = {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };

                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighbor = new Vector2Int(current.x + dir.x, current.y + dir.y);
                    if (neighbor.x < 0 || neighbor.x >= width || neighbor.y < 0 || neighbor.y >= depth)
                        continue;
                    TerrainTile neighborTile = surfaceTiles[neighbor.x, neighbor.y].GetComponent<TerrainTile>();
                    if (neighborTile != null)
                    {
                        float neighborHeight = neighborTile.transform.position.y;
                        if (neighborHeight < lowestHeight - tolerance)
                        {
                            lowestHeight = neighborHeight;
                            next = neighbor;
                            foundLower = true;
                        }
                        else if (Mathf.Abs(neighborHeight - currentHeight) <= tolerance)
                        {
                            equalCandidates.Add(neighbor);
                        }
                    }
                }

                if (!foundLower && equalCandidates.Count > 0)
                {
                    next = equalCandidates[Random.Range(0, equalCandidates.Count)];
                }

                if (next.Equals(current))
                    break;
                current = next;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Modified GenerateLavaFlows() method for very long lava flows.
    //-------------------------------------------------------------------------
    void GenerateLavaFlows()
    {
        int lavaFlowLengthLimit = 1000; // Extended limit for very long flows.
        int numberOfLavaFlows = 3;      // Adjust as desired.

        for (int i = 0; i < numberOfLavaFlows; i++)
        {
            List<Vector2Int> validStarts = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (mantleTiles[x, z] != null)
                    {
                        TerrainTile tile = mantleTiles[x, z].GetComponent<TerrainTile>();
                        if (tile != null && tile.isMantle && !tile.isLava)
                        {
                            validStarts.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }
            if (validStarts.Count == 0)
                break;

            Vector2Int start = validStarts[Random.Range(0, validStarts.Count)];
            Vector2Int current = start;
            int length = 0;

            while (length < lavaFlowLengthLimit)
            {
                length++;
                TerrainTile currentTile = mantleTiles[current.x, current.y].GetComponent<TerrainTile>();
                if (currentTile == null)
                    break;

                if (!currentTile.isLava)
                {
                    Vector3 pos = currentTile.transform.position;
                    Destroy(mantleTiles[current.x, current.y]);
                    GameObject lavaTile = Instantiate(lavaPrefab, pos, Quaternion.identity);
                    lavaTile.name = $"Lava_Tile_{current.x}_{current.y}";
                    TerrainTile newTile = lavaTile.GetComponent<TerrainTile>();
                    newTile.isMantle = true;
                    newTile.isLava = true;
                    newTile.biomeType = "Lava";
                    mantleTiles[current.x, current.y] = lavaTile;
                }

                List<Vector2Int> neighbors = new List<Vector2Int>();
                Vector2Int[] directions = {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighbor = new Vector2Int(current.x + dir.x, current.y + dir.y);
                    if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < depth)
                    {
                        if (mantleTiles[neighbor.x, neighbor.y] != null)
                        {
                            neighbors.Add(neighbor);
                        }
                    }
                }
                if (neighbors.Count == 0)
                    break;

                Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                if (next.Equals(current))
                    break;
                current = next;
            }
        }
    }

    //-------------------------------------------------------------------------
    // Generates the Moon layer.
    // – The base height is set relative to the orbit layer: orbitLayerHeight + moonHeightOffsetFromOrbit.
    // – Each tile's elevation is modified with Perlin noise, so lower-noise tiles become Moon Caves, while higher ones become Moon Dunes.
    // – The Moon is shifted to the left by moonXOffset.
    //-------------------------------------------------------------------------
    void GenerateMoonTerrain()
    {
        // Initialize the Moon tile array.
        moonTiles = new GameObject[width, depth];

        // Calculate the base height for the Moon relative to the orbit layer.
        float moonBaseHeight = orbitLayerHeight + moonHeightOffsetFromOrbit;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float noiseValue = Mathf.PerlinNoise((x + moonNoiseOffsetX) * moonNoiseScale, (z + moonNoiseOffsetZ) * moonNoiseScale);
                float elevationVariation = noiseValue * moonNoiseStrength;
                Vector3 pos = new Vector3(x * tileSize - moonXOffset, moonBaseHeight + elevationVariation, z * tileSize);

                string biome;
                GameObject prefab;
                if (noiseValue < moonCaveThreshold)
                {
                    prefab = moonCavesPrefab;
                    biome = "Moon Cave";
                }
                else
                {
                    prefab = moonDunesPrefab;
                    biome = "Moon Dunes";
                }

                GameObject tile = Instantiate(prefab, pos, Quaternion.identity);
                tile.name = $"Moon_Tile_{x}_{z}";

                TerrainTile terrainTile = tile.GetComponent<TerrainTile>();
                if (terrainTile != null)
                {
                    terrainTile.biomeType = biome;
                    terrainTile.isMoon = true;  // Flag set in TerrainTile.
                }
                moonTiles[x, z] = tile;
            }
        }
    }
}
