using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance;
    public Unit selectedUnit;
    public Color selectionColor = Color.yellow;
    private Camera mainCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mainCamera = Camera.main;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Left click for selection
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
        // Right click for movement
        else if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            HandleMovement();
        }
        // Press 'F' to found a city if the selected unit is a settler
        else if (Input.GetKeyDown(KeyCode.F) && selectedUnit is SettlerUnit)
        {
            ((SettlerUnit)selectedUnit).FoundCity();
        }
    }

    void HandleSelection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Unit clickedUnit = hit.collider.GetComponent<Unit>();
            if (clickedUnit != null)
            {
                SelectUnit(clickedUnit);
                Debug.Log($"Selected unit: {clickedUnit.unitData.unitName}");
            }
            else
            {
                DeselectCurrentUnit();
            }
        }
    }

    void HandleMovement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TerrainTile targetTile = hit.collider.GetComponent<TerrainTile>();
            if (targetTile != null)
            {
                Vector3 targetPosition = targetTile.transform.position;
                Debug.Log($"Attempting to move to tile at {targetPosition}");
                selectedUnit.MoveTo(targetPosition);
            }
            else
            {
                Debug.Log("No valid tile hit");
            }
        }
    }

    public void SelectUnit(Unit unit)
    {
        if (selectedUnit != null)
        {
            selectedUnit.OnDeselected();
        }

        selectedUnit = unit;
        if (unit != null)
        {
            unit.OnSelected();
        }
    }

    public void DeselectCurrentUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.OnDeselected();
            selectedUnit = null;
            Debug.Log("Unit deselected");
        }
    }
}