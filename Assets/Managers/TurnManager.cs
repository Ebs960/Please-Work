using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    private int currentCivIndex = 0;
    public int globalRoundCount = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize each civilization's turn count
        if (CivilizationManager.Instance != null)
        {
            foreach (CivilizationData civ in CivilizationManager.Instance.civilizationDataInstances)
            {
                civ.personalTurnCount = 1;
            }
        }

        // Update the Global UI to show the current civilization
        if (GlobalUIManager.Instance != null)
        {
            GlobalUIManager.Instance.UpdateCivilizationUI(GetCurrentCivilization());
        }
    }

    public CivilizationData GetCurrentCivilization()
    {
        var civs = CivilizationManager.Instance.civilizationDataInstances;
        if (civs != null && civs.Count > 0)
        {
            return civs[currentCivIndex];
        }
        return null;
    }

    public void EndTurn()
    {
        var civs = CivilizationManager.Instance.civilizationDataInstances;
        if (civs == null || civs.Count == 0) return;

        // 1. Generate Resources and Update Production for Current Civilization's Cities
        foreach (City city in FindObjectsByType<City>(FindObjectsSortMode.None))
        {
            if (city.owner == civs[currentCivIndex])
            {
                city.GenerateResources();
                city.UpdateProduction();
            }
        }

        // 2. Update Research Progress
        TechnologyManager.Instance.UpdateResearchProgress(civs[currentCivIndex]);

        // 3. Increment the current civilization's turn count
        CivilizationData currentCiv = civs[currentCivIndex];
        currentCiv.personalTurnCount++;

        // 4. Move to the next civilization
        currentCivIndex++;
        if (currentCivIndex >= civs.Count)
        {
            currentCivIndex = 0;
            globalRoundCount++;
        }

        // 5. Reset Movement Points for the New Civilization's Units
        foreach (Unit unit in FindObjectsByType<Unit>(FindObjectsSortMode.None))
        {
            if (unit.owner == civs[currentCivIndex])
            {
                unit.currentMovementPoints = unit.unitData.movementPoints;
            }
        }

        // 6. Update Global UI with the new current civilization
        if (GlobalUIManager.Instance != null)
        {
            GlobalUIManager.Instance.UpdateCivilizationUI(GetCurrentCivilization());
        }
    }

    public void NextTurn()
    {
        EndTurn();
    }
}