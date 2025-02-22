using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileInfoPanel : MonoBehaviour
{
    public static TileInfoPanel Instance;
    
    [Header("UI Components")]
    public GameObject panelObject;
    public TextMeshProUGUI tileTypeText;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI yieldsText;

    private Camera mainCamera;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        panelObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TerrainTile tile = hit.collider.GetComponent<TerrainTile>();
            if (tile != null)
            {
                ShowTileInfo(tile, Input.mousePosition);
            }
            else
            {
                panelObject.SetActive(false);
            }
        }
        else
        {
            panelObject.SetActive(false);
        }
    }

    void ShowTileInfo(TerrainTile tile, Vector3 mousePos)
    {
        // Set panel position near mouse
        panelObject.transform.position = mousePos + new Vector3(10f, 10f, 0f);
        
        // Set tile type
        string tileType = "Unknown";
        if (tile.isSurface) tileType = tile.IsWaterTile ? "Water" : "Plains";
        if (tile.isMantle) tileType = "Mantle";
        if (tile.isCore) tileType = "Core";
        if (tile.isSky) tileType = "Sky";
        if (tile.isOrbit) tileType = "Orbit";
        if (tile.isRiver) tileType += " (River)";
        
        tileTypeText.text = $"Terrain: {tileType}";

        // Set resource info
        if (tile.resource != null && tile.resourceRevealed)
        {
            resourceText.text = $"Resource: {tile.resource.resourceName}\n{tile.resource.description}";
        }
        else
        {
            resourceText.text = "Resource: None";
        }

        // Set yields info
        var yields = tile.GetTotalYields();
        yieldsText.text = $"Yields:\nFood: {yields.food}\nProduction: {yields.production}\n" +
                         $"Gold: {yields.gold}\nScience: {yields.science}\nCulture: {yields.culture}";

        panelObject.SetActive(true);
    }
} 