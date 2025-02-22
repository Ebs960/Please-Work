using UnityEngine;
using System.Collections.Generic;

public class TechnologyManager : MonoBehaviour
{
    public static TechnologyManager Instance { get; private set; }
    public List<Technology> globalTechTree;

    private void Awake()
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

    public void InitializeCivTechTree(CivilizationData civData)
    {
        if (civData != null && globalTechTree != null)
        {
            civData.availableTechnologies = new List<Technology>(globalTechTree);
        }
    }

    public void SetCurrentCivData(CivilizationData civData)
    {
        civData.currentTech = civData.availableTechnologies[0];
        civData.researchCost = civData.currentTech.researchCost;
        civData.currentResearchProgress = 0;
    }

    public void UpdateResearchProgress(CivilizationData civData)
    {
        if (civData != null && civData.currentTech != null)
        {
            civData.currentResearchProgress += civData.sciencePerTurn;
            if (civData.currentResearchProgress >= civData.researchCost)
            {
                CompleteTechnology(civData);
            }
        }
    }

    private void CompleteTechnology(CivilizationData civData)
    {
        if (civData != null && civData.currentTech != null)
        {
            civData.availableTechnologies.Remove(civData.currentTech);
            civData.currentTech = civData.availableTechnologies.Count > 0 ? civData.availableTechnologies[0] : null;
            civData.researchCost = civData.currentTech != null ? civData.currentTech.researchCost : 0;
            civData.currentResearchProgress = 0;
        }
    }
}