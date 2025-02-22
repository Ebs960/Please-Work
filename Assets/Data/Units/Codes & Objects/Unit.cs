using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public CivilizationData owner;
    public int currentMovementPoints;
    protected bool isMoving = false;
    protected Renderer unitRenderer;
    protected TerrainGenerator terrainGenerator;

    // UI Components
    private TextMeshPro nameLabel;
    protected Camera mainCamera;

    protected virtual void Start()
    {
        currentMovementPoints = unitData.movementPoints;
        unitRenderer = GetComponent<Renderer>();
        terrainGenerator = FindAnyObjectByType<TerrainGenerator>();
        mainCamera = Camera.main;

        CreateNameLabel();
    }

    private void CreateNameLabel()
    {
        // Create a new GameObject for the label
        GameObject labelObj = new GameObject($"{gameObject.name}_Label");
        labelObj.transform.SetParent(transform);
        labelObj.transform.localPosition = new Vector3(0, 2f, 0); // Adjust height as needed

        // Add TextMeshPro component
        nameLabel = labelObj.AddComponent<TextMeshPro>();
        nameLabel.fontSize = 3;
        nameLabel.alignment = TextAlignmentOptions.Center;

        // Make text visible from both sides
        nameLabel.enableCulling = false;

        // Add outline
        nameLabel.fontMaterial.EnableKeyword("OUTLINE_ON");
        nameLabel.outlineWidth = 0.2f;
        nameLabel.outlineColor = Color.black;

        // Set the text
        string unitName = unitData != null ? unitData.unitName : "Unknown Unit";
        string ownerName = owner?.baseCiv?.civName ?? "No Owner";
        nameLabel.text = $"{unitName}\n{ownerName}";

        // Set color
        nameLabel.color = owner?.baseCiv?.civColor ?? Color.white;
    }

    protected virtual void Update()
    {
        if (nameLabel != null)
        {
            // Make text always face camera
            nameLabel.transform.rotation = Camera.main.transform.rotation;
        }
    }

    protected virtual void OnDestroy()
    {
        if (nameLabel != null)
        {
            Destroy(nameLabel.gameObject);
        }
    }

    public virtual void OnSelected()
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = Color.yellow;
        }
        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowUnitPanel(this);
        }
    }

    public virtual void OnDeselected()
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = Color.white;
        }
    }

    public virtual void MoveTo(Vector3 targetPosition)
    {
        if (isMoving || currentMovementPoints <= 0)
        {
            Debug.Log($"Cannot move: Moving={isMoving}, MovementPoints={currentMovementPoints}");
            return;
        }

        // Round the position to the nearest tile
        Vector3 roundedTarget = new Vector3(
            Mathf.Round(targetPosition.x),
            targetPosition.y,
            Mathf.Round(targetPosition.z)
        );

        // Check if the move is only one tile away
        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(roundedTarget.x, 0, roundedTarget.z)
        );

        if (distance > 1.5f)
        {
            Debug.Log($"Target too far: {distance} units");
            return;
        }

        Debug.Log($"Starting movement to {roundedTarget}");
        StartCoroutine(MoveRoutine(roundedTarget));
    }

    protected virtual IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        float moveSpeed = 5f; // Units per second

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            transform.position = newPosition;

            yield return null;
        }

        transform.position = targetPosition;
        currentMovementPoints--;
        isMoving = false;
        Debug.Log($"Movement complete. Remaining points: {currentMovementPoints}");
    }
}