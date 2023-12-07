using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;



public class ShopController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] PlayerUpgradeController playerUpgrade;
    [SerializeField] SoundController soundController;
    [Header("Цены")]
    [SerializeField] int[] priceGrades;
    int movementSpeedUpgradePrice, damageUpgradePrice, rotationSpeedUpgradePrice;
    int movementUpgradePriceLevel = 0, damageUpgradePriceLevel = 0, rotationUpgradePriceLevel = 0;
    int maxUpgradeCount = 50;
    [Header("Тексты")]
    [SerializeField] TextMeshProUGUI speedPriceText;
    [SerializeField] TextMeshProUGUI rotationPriceText;
    [SerializeField] TextMeshProUGUI damagePriceText;
    void Start()
    {
        movementUpgradePriceLevel = Progress.Instance.playerInfo.speedUpgradeLevel;
        rotationUpgradePriceLevel = Progress.Instance.playerInfo.rotationUpgradeLevel;
        damageUpgradePriceLevel = Progress.Instance.playerInfo.damageUpgradeLevel;
        ChangePriceLevel(out movementSpeedUpgradePrice, movementUpgradePriceLevel);      
        ChangePriceLevel(out rotationSpeedUpgradePrice, rotationUpgradePriceLevel);      
        ChangePriceLevel(out damageUpgradePrice, damageUpgradePriceLevel);       
        UpdatePriceText(speedPriceText, movementSpeedUpgradePrice);
        UpdatePriceText(rotationPriceText, rotationSpeedUpgradePrice);
        UpdatePriceText(damagePriceText, damageUpgradePrice);
        if (movementSpeedUpgradePrice == 0)
            playerUpgrade.BlockSpeedUpgradeButton();
        if (rotationSpeedUpgradePrice == 0)
            playerUpgrade.BlockRotationUpgradeButton();
        if (damageUpgradePrice == 0)
            playerUpgrade.BlockDamageUpgradeButton();
    }

    //По кнопкам
    public void TryBuyMovementSpeedUpgrade() 
    { 
        if(moneyManager.GetMoneyCount() < movementSpeedUpgradePrice)
        {
            FailedBuy();
            return;
        }
        moneyManager.UpdateMoneyCount(-movementSpeedUpgradePrice);
        SuccessBuy();      
        playerUpgrade.UpgradeMovementSpeed();
        movementUpgradePriceLevel++;
        Progress.Instance.playerInfo.speedUpgradeLevel = movementUpgradePriceLevel;
        YandexSDK.Save();
        if (movementUpgradePriceLevel == maxUpgradeCount)
            return;
        ChangePriceLevel(out movementSpeedUpgradePrice, movementUpgradePriceLevel);
        UpdatePriceText(speedPriceText, movementSpeedUpgradePrice);
        YandexSDK.Save();
    }
    public void TryBuyRotationSpeedUpgrade() 
    {
        if(moneyManager.GetMoneyCount() < rotationSpeedUpgradePrice)
        {
            FailedBuy();
            return;
        }     
        moneyManager.UpdateMoneyCount(-rotationSpeedUpgradePrice);
        SuccessBuy();
        playerUpgrade.UpgradeRotationSpeed();
        rotationUpgradePriceLevel++;
        Progress.Instance.playerInfo.rotationUpgradeLevel = rotationUpgradePriceLevel;
        YandexSDK.Save();
        if (rotationUpgradePriceLevel == maxUpgradeCount)
            return;
        ChangePriceLevel(out rotationSpeedUpgradePrice, rotationUpgradePriceLevel);
        UpdatePriceText(rotationPriceText, rotationSpeedUpgradePrice);
        YandexSDK.Save();
    }
    public void TryBuyEnemyDamageUpgrade() 
    {       
        if (moneyManager.GetMoneyCount() < damageUpgradePrice)
        {
            FailedBuy();
            return;
        }
        
        moneyManager.UpdateMoneyCount(-damageUpgradePrice);
        SuccessBuy();
        playerUpgrade.UpgradeDealingDamage();
        damageUpgradePriceLevel++;
        Progress.Instance.playerInfo.damageUpgradeLevel = damageUpgradePriceLevel;
        YandexSDK.Save();
        if (damageUpgradePriceLevel == maxUpgradeCount)
            return;
        ChangePriceLevel(out damageUpgradePrice, damageUpgradePriceLevel);
        UpdatePriceText(damagePriceText, damageUpgradePrice);
       
    }

    void ChangePriceLevel(out int price,int upgLevel)
    {
        if (upgLevel < priceGrades.Length)
        {
            price = priceGrades[upgLevel];
        }
        else
        {
            price = 0;           
        }
    }

        void UpdatePriceText(TextMeshProUGUI priceText, int price)
    {
        priceText.text = price.ToString();
    }
    void SuccessBuy()
    {
        soundController.Play("SuccesBuy");
    }
    void FailedBuy()
    {
        soundController.Play("FailBuy");
    }
}
