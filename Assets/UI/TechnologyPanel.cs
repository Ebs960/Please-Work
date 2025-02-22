using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechnologyPanel : BasePanel
{
    public TextMeshProUGUI titleText;
    public Transform techTreeContent;

    protected override void Awake()
    {
        base.Awake();
        if (titleText != null)
            titleText.text = "Technology Tree";
    }
} 