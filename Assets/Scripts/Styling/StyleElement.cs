using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StyleElement : MonoBehaviour
{
    public string id;
    public abstract void Apply();

    private void Start()
    {
        Apply();
        StyleManager.instance.OnStyleChange += Apply;
    }

    private void OnDestroy()
    {
        StyleManager.instance.OnStyleChange -= Apply;
    }
}
