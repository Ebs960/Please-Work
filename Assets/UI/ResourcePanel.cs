using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcePanel : BasePanel
{
    public TextMeshProUGUI titleText;
    public Transform resourceListContent;

    protected override void Awake()
    {
        base.Awake();
        if (titleText != null)
            titleText.text = "Resources";
    }
} 