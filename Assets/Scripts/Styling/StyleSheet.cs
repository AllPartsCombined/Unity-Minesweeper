using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStyle", menuName = "Style", order = 1)]
public class StyleSheet : ScriptableObject
{
    public List<StyleAttributeImage> imageAttributes = new List<StyleAttributeImage>();
    public List<StyleAttributeText> textAttributes = new List<StyleAttributeText>();
    public List<StyleAttributeButtton> buttonAttributes = new List<StyleAttributeButtton>();
    public List<StyleAttributeMultifunctionButtton> multifunctionButtonAttributes = new List<StyleAttributeMultifunctionButtton>();
    public List<StyleAttributeAnimatedImage> animatedImageAttributes = new List<StyleAttributeAnimatedImage>();
}

[System.Serializable]
public abstract class StyleAttribute
{
    public string id;
    public bool setColor;
    public Color color;
}

[System.Serializable]
public class StyleAttributeImage : StyleAttribute
{
    public bool setSprite = true;
    public Sprite sprite;
}

[System.Serializable]
public class StyleAttributeText : StyleAttribute
{
    public bool setFont;
    public Font font;
    public bool setFontSize;
    public int fontSize;
}

[System.Serializable]
public class StyleAttributeButtton : StyleAttribute
{
    [Header("Sprite Swap Transition")]
    public bool setHighlightedSprite;
    public Sprite highlightedSprite;
    public bool setPressedSprite = true;
    public Sprite pressedSprite;
    public bool setSelectedSprite;
    public Sprite selectedSprite;
    public bool setDisabledSprite;
    public Sprite disabledSprite;

    [Header("Color Tint Transition")]
    public bool setNormalColor = true;
    public Color normalColor;
    public bool setHighlightedColor = true;
    public Color highlightedColor;
    public bool setPressedColor = true;
    public Color pressedColor;
    public bool setSelectedColor = true;
    public Color selectedColor;
    public bool setDisabledColor;
    public Color disabledColor;
}

[System.Serializable]
public class StyleAttributeMultifunctionButtton : StyleAttributeImage
{
    public bool setDefaultSprite = true;
    public Sprite defaultSprite;
    public bool setHoverSprite;
    public Sprite hoverSprite;
    public bool setClickedSprite;
    public Sprite clickedSprite;
}

[System.Serializable]
public class StyleAttributeAnimatedImage : StyleAttributeImage
{
    public bool setSprites;
    public Sprite[] sprites;
    public bool setFPS;
    public int fps;
}