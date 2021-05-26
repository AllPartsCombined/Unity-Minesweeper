using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleElementText : StyleElement
{
    public Text text;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<Text>();
    }

    public override void Apply()
    {
        foreach (var textAttribute in StyleManager.instance.CurrentStyle.textAttributes)
        {
            if (textAttribute.id == id)
            {
                text.font = textAttribute.setFont ? textAttribute.font : text.font;
                text.fontSize = textAttribute.setFontSize ? textAttribute.fontSize : text.fontSize;
                text.color = textAttribute.setColor ? textAttribute.color : text.color;
                break;
            }
        }
    }
}
