using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public CityPanel cityPanel;
    public TechnologyPanel technologyPanel;
    public ResourcePanel resourcePanel;
    public UnitPanel unitPanel;

    private List<BasePanel> allPanels;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePanels();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializePanels()
    {
        allPanels = new List<BasePanel>
        {
            cityPanel,
            technologyPanel,
            resourcePanel,
            unitPanel
        };
    }

    public void HideAllPanels()
    {
        foreach (var panel in allPanels)
        {
            if (panel != null)
                panel.Hide();
        }
    }

    void Update()
    {
        // Panel hotkeys
        if (Input.GetKeyDown(KeyCode.C))
            cityPanel?.Toggle();
        if (Input.GetKeyDown(KeyCode.T))
            technologyPanel?.Toggle();
        if (Input.GetKeyDown(KeyCode.R))
            resourcePanel?.Toggle();
        if (Input.GetKeyDown(KeyCode.U))
            unitPanel?.Toggle();

        // Escape key hides all panels
        if (Input.GetKeyDown(KeyCode.Escape))
            HideAllPanels();
    }
}