using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "UI Theme", menuName = "UI/Theme")]
public class UITheme : ScriptableObject
{
    [Header("Colors")]
    public Color panelBackground;
    public Color buttonNormal;
    public Color buttonHover;
    public Color textColor;

    [Header("Sprites")]
    public Sprite panelSprite;
    public Sprite buttonSprite;
    public Sprite sliderBackground;
    public Sprite sliderFill;

    [Header("Fonts")]
    public TMP_FontAsset mainFont;
    public float titleFontSize = 24;
    public float normalFontSize = 18;
} 