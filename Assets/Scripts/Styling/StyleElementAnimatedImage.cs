using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleElementAnimatedImage : StyleElementImage
{
    public AnimatedImage animatedImage;

    protected override void Awake()
    {
        base.Awake();
        if (animatedImage == null)
            animatedImage = GetComponent<AnimatedImage>();
    }

    public override void Apply()
    {
        base.Apply();
        foreach (var animatedImageAttribute in StyleManager.instance.CurrentStyle.animatedImageAttributes)
        {
            if (animatedImageAttribute.id == id)
            {

                animatedImage.sprites = animatedImageAttribute.setSprites ? animatedImageAttribute.sprites : animatedImage.sprites;
                animatedImage.fps = animatedImageAttribute.setFPS ? animatedImageAttribute.fps : animatedImage.fps;
            }
        }
    }
}
