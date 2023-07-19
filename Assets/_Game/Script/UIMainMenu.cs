using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UITemplate
{
    public void PlayButton()
    {
        BR_UIManager.Instance.gamePlay.Open();
        LevelManager.Instance.StartLevel();
        Close();
    }
}
