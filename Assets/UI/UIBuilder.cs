using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    public UITheme theme;

    void Start()
    {
        CreateMainCanvas();
    }

    void CreateMainCanvas()
    {
        // Create main canvas
        GameObject canvasObj = new GameObject("UI Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Add UIManager
        UIManager uiManager = canvasObj.AddComponent<UIManager>();

        // Create all panels
        uiManager.cityPanel = CreateCityPanel(canvasObj.transform);
        uiManager.unitPanel = CreateUnitPanel(canvasObj.transform);
        uiManager.technologyPanel = CreateTechnologyPanel(canvasObj.transform);
        uiManager.resourcePanel = CreateResourcePanel(canvasObj.transform);
    }

    CityPanel CreateCityPanel(Transform parent)
    {
        // Create panel object
        GameObject panelObj = CreateBasePanel("City Panel", parent);
        CityPanel cityPanel = panelObj.AddComponent<CityPanel>();

        // Create layout
        GameObject content = new GameObject("Content");
        content.transform.SetParent(panelObj.transform, false);
        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.spacing = 10;

        // Add city name
        cityPanel.cityNameText = CreateText(content.transform, "City Name", 24);
        cityPanel.populationText = CreateText(content.transform, "Population: 1", 18);
        cityPanel.resourcesText = CreateText(content.transform, "Resources", 18);

        // Add production section
        GameObject productionObj = new GameObject("Production");
        productionObj.transform.SetParent(content.transform, false);
        
        cityPanel.currentProductionText = CreateText(productionObj.transform, "Currently Producing: None", 18);
        cityPanel.productionProgressSlider = CreateSlider(productionObj.transform);
        cityPanel.addProductionButton = CreateButton(productionObj.transform, "Add Production");

        // Position panel
        RectTransform rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0.3f, 1);
        rect.pivot = new Vector2(0, 0.5f);
        rect.anchoredPosition = new Vector2(10, 0);

        return cityPanel;
    }

    UnitPanel CreateUnitPanel(Transform parent)
    {
        // Create panel object
        GameObject panelObj = CreateBasePanel("Unit Panel", parent);
        UnitPanel unitPanel = panelObj.AddComponent<UnitPanel>();

        // Create layout
        GameObject content = new GameObject("Content");
        content.transform.SetParent(panelObj.transform, false);
        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.spacing = 10;

        // Add unit info
        unitPanel.unitNameText = CreateText(content.transform, "Unit Name", 24);
        unitPanel.ownerText = CreateText(content.transform, "Owner: None", 18);
        unitPanel.statsText = CreateText(content.transform, "Stats", 18);
        unitPanel.movementText = CreateText(content.transform, "Movement: 2/2", 18);

        // Add buttons
        GameObject buttonsObj = new GameObject("Buttons");
        buttonsObj.transform.SetParent(content.transform, false);
        HorizontalLayoutGroup buttonLayout = buttonsObj.AddComponent<HorizontalLayoutGroup>();
        buttonLayout.spacing = 10;

        unitPanel.moveButton = CreateButton(buttonsObj.transform, "Move");
        unitPanel.attackButton = CreateButton(buttonsObj.transform, "Attack");
        unitPanel.foundCityButton = CreateButton(buttonsObj.transform, "Found City");

        // Position panel
        RectTransform rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.7f, 0.5f);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 0.5f);
        rect.anchoredPosition = new Vector2(-10, 0);

        return unitPanel;
    }

    TechnologyPanel CreateTechnologyPanel(Transform parent)
    {
        GameObject panelObj = CreateBasePanel("Technology Panel", parent);
        return panelObj.AddComponent<TechnologyPanel>();
    }

    ResourcePanel CreateResourcePanel(Transform parent)
    {
        GameObject panelObj = CreateBasePanel("Resource Panel", parent);
        return panelObj.AddComponent<ResourcePanel>();
    }

    GameObject CreateBasePanel(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        // Add required components
        Image backgroundImage = obj.AddComponent<Image>();
        backgroundImage.color = new Color(0, 0, 0, 0.8f);
        
        obj.AddComponent<CanvasGroup>();

        // Set size
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 400);

        if (theme != null)
        {
            backgroundImage.sprite = theme.panelSprite;
            backgroundImage.color = theme.panelBackground;
        }

        return obj;
    }

    TextMeshProUGUI CreateText(Transform parent, string text, int fontSize)
    {
        GameObject obj = new GameObject("Text");
        obj.transform.SetParent(parent, false);
        
        TextMeshProUGUI textComponent = obj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;

        if (theme != null)
        {
            textComponent.font = theme.mainFont;
            textComponent.color = theme.textColor;
        }

        return textComponent;
    }

    Button CreateButton(Transform parent, string text)
    {
        GameObject obj = new GameObject("Button");
        obj.transform.SetParent(parent, false);
        
        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f);
        
        Button button = obj.AddComponent<Button>();
        
        TextMeshProUGUI tmp = CreateText(obj.transform, text, 16);
        tmp.color = Color.white;

        return button;
    }

    Slider CreateSlider(Transform parent)
    {
        GameObject obj = new GameObject("Slider");
        obj.transform.SetParent(parent, false);
        
        Slider slider = obj.AddComponent<Slider>();
        
        // Create background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(obj.transform, false);
        Image backgroundImage = background.AddComponent<Image>();
        backgroundImage.color = Color.gray;
        
        // Create fill area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(obj.transform, false);
        Image fillImage = fillArea.AddComponent<Image>();
        fillImage.color = Color.white;
        
        slider.targetGraphic = fillImage;

        return slider;
    }
} 