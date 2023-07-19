using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : SimpleSingleton<GameControl>
{
    private void Awake()
    {
        BR_UIManager.Instance.mainMenu.Open();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void End()
    {
        LevelManager.Instance.StopAllEnemies();
    }
}
