using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleElementMultifunctionButton : StyleElementImage
{
    public MultifunctionButton button;

    protected override void Awake()
    {
        base.Awake();
        if (button == null)
            button = GetComponent<MultifunctionButton>();
    }

    public override void Apply()
    {
        base.Apply();
        foreach (var buttonAttribute in StyleManager.instance.CurrentStyle.multifunctionButtonAttributes)
        {
            if (buttonAttribute.id == id)
            {

                button.defaultSprite = buttonAttribute.setDefaultSprite ? buttonAttribute.defaultSprite : button.defaultSprite;
                button.hoverSprite = buttonAttribute.setHoverSprite ? buttonAttribute.hoverSprite : button.hoverSprite;
                button.clickedSprite = buttonAttribute.setClickedSprite ? buttonAttribute.clickedSprite : button.clickedSprite;
            }
        }
    }
}
