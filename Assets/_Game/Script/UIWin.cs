using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UITemplate
{
    public Text score;

    public void MainMenuButton()
    {
        BR_UIManager.Instance.mainMenu.Open();
        Close();
    }
}
