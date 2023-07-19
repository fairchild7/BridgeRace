using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : UITemplate
{
    public void SettingButton()
    {
        BR_UIManager.Instance.setting.Open();
        GameControl.Instance.Pause();
        Close();
    }
}
