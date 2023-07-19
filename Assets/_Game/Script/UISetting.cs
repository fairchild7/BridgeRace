using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UITemplate
{
    public void ContinueButton()
    {
        BR_UIManager.Instance.gamePlay.Open();
        GameControl.Instance.Resume();
        Close();
    }
}
