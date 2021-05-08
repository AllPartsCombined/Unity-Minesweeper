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
        text.text = time.ToString("000");
    }
}
