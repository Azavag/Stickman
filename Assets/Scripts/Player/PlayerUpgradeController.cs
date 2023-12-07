using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerWeaponGrades
{
    public int level;
    public TextMeshProUGUI weaponNameText;
    public GameObject weaponModel;
}


public class PlayerUpgradeController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] PlayerController player;
    [SerializeField] PlayerWeaponController weaponController;
    [SerializeField] PlayerCanvasController canvasController;
    [SerializeField] SoundController soundController;
    [Header("Улучшения")]
    [SerializeField] float movementSpeedUpgrade;
    [SerializeField] float damageUpgrade;
    [SerializeField] float rotationSpeedUpgrade;
    [Header("Кнопки")]
    [SerializeField] Button speedUpgradeButton;
    [SerializeField] Button rotationUpgradeButton;
    [SerializeField] Button damageUpgradeButton;
    [Header("Текстовые счётчики")]
    [SerializeField] TextMeshProUGUI speedPriceText;
    [SerializeField] TextMeshProUGUI rotationPriceText;
    [SerializeField] TextMeshProUGUI damagePriceText;
    int movementSpeedUpgradeCounter =0;
    int rotationSpeedUpgradeCounter = 0;
    int enemyDamageUpgradeCounter = 0;
    int maxUpgradeCount = 50;
    [Header("Оружия")]
    [SerializeField] PlayerWeaponGrades[] weaponGrades;
    [SerializeField] TextMeshProUGUI[] upgradesTexts;
    int weaponModelCounter = 0;
    GameObject currentWeaponModel;
    void Start()
    {
        foreach (var text in upgradesTexts)
        {
            canvasController.ClearColor(text);
        }
        foreach (var grade in weaponGrades)
        {
            grade.weaponModel.SetActive(false);
        }
        movementSpeedUpgradeCounter = Progress.Instance.playerInfo.speedUpgradeLevel;
        rotationSpeedUpgradeCounter = Progress.Instance.playerInfo.rotationUpgradeLevel;
        enemyDamageUpgradeCounter = Progress.Instance.playerInfo.damageUpgradeLevel;
        weaponModelCounter = Progress.Instance.playerInfo.weaponModelNumber;
        WearWeapon(weaponModelCounter);     
    }

    void WearWeapon(int upgradeLevel)
    {          
        currentWeaponModel = weaponGrades[upgradeLevel].weaponModel;
        currentWeaponModel.SetActive(true);
    }
    void CheckOnSwapWeapons(int upgradeLevel)
    {    
        foreach(var grade in weaponGrades)
        {
            if (upgradeLevel == grade.level)
            {
                weaponModelCounter++;
                Progress.Instance.playerInfo.weaponModelNumber = weaponModelCounter;
                YandexSDK.Save();
                currentWeaponModel.SetActive(false);
                canvasController.ShowWeaponUpgradeText(grade.weaponNameText);
                if(grade.weaponNameText.text != "")
                    soundController.Play("WeaponUpgrade");
                WearWeapon(weaponModelCounter);
                return;
            }
        }
    }

    public void UpgradeMovementSpeed()
    {
        player.ChangeMovementSpeed(movementSpeedUpgrade);
        movementSpeedUpgradeCounter++;
        if(movementSpeedUpgradeCounter == maxUpgradeCount)
        {
            BlockSpeedUpgradeButton();
            return;
        }

    }
    public void UpgradeRotationSpeed()
    {
        player.ChangeRotationSpeed(rotationSpeedUpgrade);
        rotationSpeedUpgradeCounter++;
        if (rotationSpeedUpgradeCounter == maxUpgradeCount)
        {
            BlockRotationUpgradeButton();
            return;
        }
    }
    public void UpgradeDealingDamage()
    {
        player.ChangeEnemyDamage(damageUpgrade);
        enemyDamageUpgradeCounter++;
        CheckOnSwapWeapons(enemyDamageUpgradeCounter);
        if (enemyDamageUpgradeCounter == maxUpgradeCount)
        {
            BlockDamageUpgradeButton();
            return;
        }
    }

    public void BlockSpeedUpgradeButton()
    {        
        speedUpgradeButton.interactable = false;
        UpdateText(speedPriceText);
    }
    public void BlockRotationUpgradeButton()
    {
        rotationUpgradeButton.interactable = false;
        UpdateText(rotationPriceText);
    }
    public void BlockDamageUpgradeButton()
    {
        damageUpgradeButton.interactable = false;
        UpdateText(damagePriceText);
    }

    void UpdateText(TextMeshProUGUI text)
    {
        if (Language.Instance.currentLanguage == "ru")
            text.text = "макс";
        else text.text = "max";
    }
}
