using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] bool isDamageNumberShow;
    [SerializeField] Toggle toggle;
    private void Start()
    {
        toggle.isOn = Progress.Instance.playerInfo.isShowDamageNumber;
        isDamageNumberShow = Progress.Instance.playerInfo.isShowDamageNumber;
        StaticSettings.isStaticDamageNumberShow = isDamageNumberShow;
    }
    
    public void Toggle_Changed(bool newValue)
    {
        isDamageNumberShow = newValue;
        StaticSettings.isStaticDamageNumberShow = isDamageNumberShow;
        Progress.Instance.playerInfo.isShowDamageNumber = isDamageNumberShow;
    }
}

public static class StaticSettings
{
    static public bool isStaticDamageNumberShow;
    public static bool CanShowDamageNumber()
    {
        return isStaticDamageNumberShow;
    }
}

