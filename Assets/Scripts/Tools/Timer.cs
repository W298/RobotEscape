using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public delegate void ExecuteFunction();
    public ExecuteFunction exeFunc;

    public bool active = true;

    private float interval;
    private float timer;

    public Timer(float interval, ExecuteFunction exeFunc)
    {
        this.interval = interval;
        this.exeFunc = exeFunc;

        Reset();
    }

    public Timer(int frequency, ExecuteFunction exeFunc)
    {
        interval = 1f / frequency;
        timer = interval;
        this.exeFunc = exeFunc;
    }

    public void Reset()
    {
        timer = interval;
    }

    public void Update()
    {
        if (!active) return;
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer += interval;
            exeFunc();
        }
    }
}
