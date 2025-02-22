using UnityEngine;

public class UnitInputManager : MonoBehaviour
{
    public static UnitInputManager Instance;
    private Camera mainCamera;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            HandleUnitMovement();
        }
    }

    void HandleUnitMovement()
    {
        Unit selectedUnit = UnitSelectionManager.Instance.selectedUnit;
        if (selectedUnit == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = new Vector3(
                Mathf.Round(hit.point.x),
                selectedUnit.transform.position.y,
                Mathf.Round(hit.point.z)
            );
            
            selectedUnit.MoveTo(targetPosition);
        }
    }
} 