using UnityEngine;
using TMPro;

public class UnitLabel : MonoBehaviour
{
    private TextMeshProUGUI labelText;
    private Unit unit;
    private Camera mainCamera;
    private Canvas worldCanvas;
    
    void Start()
    {
        mainCamera = Camera.main;
        unit = GetComponent<Unit>();
        
        // Create world space canvas
        GameObject canvasObj = new GameObject("UnitCanvas");
        worldCanvas = canvasObj.AddComponent<Canvas>();
        worldCanvas.renderMode = RenderMode.WorldSpace;
        canvasObj.AddComponent<CanvasRenderer>();
        
        // Create label
        GameObject labelObj = new GameObject("UnitLabel");
        labelObj.transform.SetParent(canvasObj.transform, false);
        labelText = labelObj.AddComponent<TextMeshProUGUI>();
        
        // Set up label
        labelText.fontSize = 12;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.color = Color.white;
        labelText.outlineWidth = 0.2f;
        labelText.outlineColor = Color.black;
        
        // Position canvas above unit
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = new Vector3(0, 2f, 0);
        canvasObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        
        UpdateLabel();
    }

    void Update()
    {
        if (worldCanvas != null)
        {
            worldCanvas.transform.LookAt(mainCamera.transform);
            worldCanvas.transform.Rotate(0, 180, 0);
        }
    }

    void UpdateLabel()
    {
        if (unit != null && labelText != null)
        {
            string unitName = unit.unitData.unitName;
            string ownerName = unit.owner?.baseCiv?.civName ?? "No Owner";
            labelText.text = $"{unitName}\n{ownerName}";
            labelText.color = unit.owner?.baseCiv?.civColor ?? Color.white;
        }
    }
} 