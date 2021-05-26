using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleElementImage : StyleElement
{
    public Image image;

    protected virtual void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    public override void Apply()
    {
        List<StyleAttributeImage> imageAttributes = new List<StyleAttributeImage>();
        imageAttributes.AddRange(StyleManager.instance.CurrentStyle.imageAttributes);
        imageAttributes.AddRange(StyleManager.instance.CurrentStyle.multifunctionButtonAttributes);
        imageAttributes.AddRange(StyleManager.instance.CurrentStyle.animatedImageAttributes);
        foreach (var imageAttribute in imageAttributes)
        {
            if (imageAttribute.id == id)
            {
                image.sprite = imageAttribute.setSprite ? imageAttribute.sprite : image.sprite;
                image.color = imageAttribute.setColor ? imageAttribute.color : image.color;
                break;
            }
        }
    }
}
