using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        Timer.Instance.OnTimeUpdated += HandleTimeUpdated;
    }

    private void HandleTimeUpdated(int time)
    {
        time = Mathf.Clamp(time, 0, 999);
        text.text = time.ToString("000");
    }
}
