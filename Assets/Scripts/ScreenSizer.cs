using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizer : MonoBehaviour
{
    public Vector2 aspectRatio = new Vector2(1100, 700);
    public bool debug = false;
    public RectTransform minefield;
    public Vector2Int padding;
    public CanvasScaler canvasScaler;
    private Vector2Int lastResolution;
    private bool setting;
    private bool forceResize;

    private void Start()
    {
        lastResolution = new Vector2Int(Screen.width, Screen.height);
        GameController.Instance.OnInit += ForceResize;
    }

    public void ForceResize()
    {
        forceResize = true;
    }

    private void Update()
    {
        var resolution = new Vector2Int(Screen.width, Screen.height);
        if (debug)
            Debug.Log("Screen: " + resolution);
//#if !UNITY_EDITOR
        if (!setting)
        {
            aspectRatio = new Vector2(minefield.sizeDelta.x + padding.x, minefield.sizeDelta.y + padding.y);
            if (resolution.x != lastResolution.x)
            {
                float h = resolution.x * (aspectRatio.y / aspectRatio.x);
                StartCoroutine(SetResolution(resolution.x, (int)h));
            }
            else if (resolution.y != lastResolution.y || forceResize)
            {
                float w = resolution.y * (aspectRatio.x / aspectRatio.y);
                StartCoroutine(SetResolution((int)w, resolution.y));
            }
        }
//#endif
    }
    IEnumerator SetResolution(int w, int h)
    {
        setting = true;
        forceResize = false;
        canvasScaler.referenceResolution = new Vector2(minefield.sizeDelta.x + padding.x, minefield.sizeDelta.y + padding.y);
        Screen.SetResolution(w, h, false);
        yield return new WaitForSeconds(.5f);
        lastResolution = new Vector2Int(w, h);
        setting = false;
    }
}
