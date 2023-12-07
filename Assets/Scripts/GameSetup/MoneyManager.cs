using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] int moneyCount = 0;
    [SerializeField] TextMeshProUGUI moneyText;

    void Start()
    {
        moneyCount = Progress.Instance.playerInfo.moneyCount;
        UpdateMoneyText();
    }

    public void UpdateMoneyCount(int moneyDiff)
    {
        moneyCount += moneyDiff;
        Progress.Instance.playerInfo.moneyCount = moneyCount;
        UpdateMoneyText();
    }
    void UpdateMoneyText()
    {
        moneyText.text = moneyCount.ToString();
    }
    
    public int GetMoneyCount()
    {
        return moneyCount;
    }

}
