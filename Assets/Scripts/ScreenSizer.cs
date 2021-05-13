using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizer : MonoBehaviour
{
    public Vector2 aspectRatio = new Vector2(1100, 700);
    public bool debug = false;
    private Vector2Int lastResolution;
    private bool setting;

    private void Awake()
    {
        lastResolution = new Vector2Int(Screen.width, Screen.height);
    }

    private void Update()
    {
        var resolution = new Vector2Int(Screen.width, Screen.height);
        if (debug)
            Debug.Log("Screen: " + resolution);
#if !UNITY_EDITOR
        if (!setting)
        {
            if (resolution.x != lastResolution.x)
            {
                float h = resolution.x * (aspectRatio.y / aspectRatio.x);
                StartCoroutine(SetResolution(resolution.x, (int)h));
            }
            else if (resolution.y != lastResolution.y)
            {
                float w = resolution.y * (aspectRatio.x / aspectRatio.y);
                StartCoroutine(SetResolution((int)w, resolution.y));
            }
        }
#endif
    }
    IEnumerator SetResolution(int w, int h)
    {
        setting = true;
        Screen.SetResolution(w, h, false);
        yield return new WaitForSeconds(.5f);
        lastResolution = new Vector2Int(w, h);
        setting = false;
    }
}
