using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public bool startOn1 = true;

    private int time = 0;
    private bool timerActive = false;

    public int Time
    {
        get => time;
    }

    public event Action<int> OnTimeUpdated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void StartTimer()
    {
        if (timerActive)
            return;
        timerActive = true;
        StartCoroutine(TimerCoroutine());
    }

    public void StopTimer(bool clear = false)
    {
        timerActive = false;
        if(clear)
            UpdateTime(0);
        StopAllCoroutines();
    }

    private void UpdateTime(int val)
    {
        time = val;
        if (OnTimeUpdated != null)
            OnTimeUpdated(time);
    }

    IEnumerator TimerCoroutine()
    {
        if (startOn1)
            UpdateTime(1);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateTime(++time);
        }
    }
}
