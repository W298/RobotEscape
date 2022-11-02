using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelController
{
    public float defaultLevel;
    public float currentLevel;

    public Timer incTimer;
    public Timer decTimer;

    public LevelController(float defaultLevel, float incInterval, float decInterval)
    {
        this.defaultLevel = defaultLevel;

        incTimer = new Timer(incInterval, () => { if (currentLevel < 100) currentLevel++; });
        decTimer = new Timer(decInterval, () => { if (currentLevel > 0) currentLevel--; });

        currentLevel = this.defaultLevel;
    }

    public void Update()
    {
        incTimer.Update();
        decTimer.Update();
    }
}
