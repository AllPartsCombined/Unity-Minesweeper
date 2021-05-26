using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleElementButton : StyleElement
{
    public Button button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public override void Apply()
    {
        foreach (var buttonAttribute in StyleManager.instance.CurrentStyle.buttonAttributes)
        {
            if (buttonAttribute.id == id)
            {
                switch (button.transition)
                {
                    case Selectable.Transition.ColorTint:
                        ColorBlock colors = new ColorBlock();
                        colors.normalColor = buttonAttribute.setNormalColor ? buttonAttribute.normalColor : button.colors.normalColor;
                        colors.highlightedColor = buttonAttribute.setHighlightedColor ? buttonAttribute.highlightedColor : button.colors.highlightedColor;
                        colors.pressedColor = buttonAttribute.setPressedColor ? buttonAttribute.pressedColor : button.colors.pressedColor;
                        colors.selectedColor = buttonAttribute.setSelectedColor ? buttonAttribute.selectedColor : button.colors.selectedColor;
                        colors.disabledColor = buttonAttribute.setDisabledColor ? buttonAttribute.disabledColor : button.colors.disabledColor;
                        button.colors = colors;
                        break;
                    case Selectable.Transition.SpriteSwap:
                        SpriteState spriteState = new SpriteState();
                        spriteState.highlightedSprite = buttonAttribute.setHighlightedSprite ? buttonAttribute.highlightedSprite : button.spriteState.highlightedSprite;
                        spriteState.pressedSprite = buttonAttribute.setPressedSprite ? buttonAttribute.pressedSprite : button.spriteState.pressedSprite;
                        spriteState.selectedSprite = buttonAttribute.setSelectedSprite ? buttonAttribute.selectedSprite : button.spriteState.selectedSprite;
                        spriteState.disabledSprite = buttonAttribute.setDisabledSprite ? buttonAttribute.disabledSprite : button.spriteState.disabledSprite;
                        button.spriteState = spriteState;
                        break;
                }
                break;
            }
        }
    }
}
