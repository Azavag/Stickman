using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceNavigationController : MonoBehaviour
{
    [Header("Канвасы")]
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject ingameCanvas;
    [SerializeField] GameObject endLevelCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject resourcesCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject joystickCanvas;
    [Header("Тексты")]
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI endLevelText;
    [Header("Ссылка")]
    [SerializeField] AnimationController animationController;

    string levelCountInterText;
    string winLevelInterText;
    string loseLevelInterText;

    private void Start()
    {
        if (Language.Instance.currentLanguage == "ru")
        {
            levelCountInterText = "уровень";
            winLevelInterText = "Победа";
            loseLevelInterText = "Провал";
        }
        else
        {
            levelCountInterText = "level";
            winLevelInterText = "Victory";
            loseLevelInterText = "Failure";
        }


    }
    public void CanvasesSetup()
    {
        ShowEndLevelCanvas(false);
        ShowIngameCanvas(false);
        ShowPauseCanvas(false);
        ShowSettingsCanvas(false);
        ShowJoystickCanvas(true);
        ShowStartCanvas(true);
        ShowShopCanvas(true);
        ShowResourcesCanvas(true);
    }
    public void ChangeStartToIngame()
    {
        ShowStartCanvas(false);
        ShowShopCanvas(false);
        ShowIngameCanvas(true);
    }
    public void ChangeIngameToEnd()
    {
        ShowIngameCanvas(false);
        ShowJoystickCanvas(false);
        ShowEndLevelCanvas(true);
        animationController.ShowEndLevelPanel();
    }
    public void ChangeEndToMenu()
    {
        animationController.HideEndLevelPanel(CloseEndLevelCanvas);
        ShowJoystickCanvas(true);
    }
    void CloseEndLevelCanvas()
    {
        ShowStartCanvas(true);
        ShowShopCanvas(true);
        ShowEndLevelCanvas(false);
    }
    //По кнопке
    public void ChangeMenuToSettings()
    {
        ShowSettingsCanvas(true);
        animationController.ShowSettingsPanelPanel();
    }
    public void ChangePauseToSettings()
    {
        ShowSettingsCanvas(true);
        animationController.ShowSettingsPanelPanel();
    }
    public void ChangeSettingsToMenu()
    {
        animationController.HideSettingsPanel(CloseSettingsCanvas);
    }
    void CloseSettingsCanvas()
    {
        ShowSettingsCanvas(false);
    }

    public void UpdateLevelNumberText(int levelNumber)
    {
        levelNumberText.text = (levelNumber + 1).ToString() + $" {levelCountInterText}";
    }

    public void UpdateEndLevelText(bool success)
    {
        if (success)
            endLevelText.text = winLevelInterText;
        else endLevelText.text = loseLevelInterText;
    }

    public void ShowPauseCanvas(bool state)
    {
        pauseCanvas.SetActive(state);
    }
    void ShowShopCanvas(bool state)
    {
        shopCanvas.SetActive(state);
    }
    public void ShowIngameCanvas(bool state)
    {
        ingameCanvas.SetActive(state);
    }
    public void ShowEndLevelCanvas(bool state)
    {
        endLevelCanvas.SetActive(state);
    }
    void ShowResourcesCanvas(bool state)
    {
        resourcesCanvas.SetActive(state);
    }
    void ShowStartCanvas(bool state)
    {
        startCanvas.SetActive(state);
    }
    public void ShowSettingsCanvas(bool state)
    {
        settingsCanvas.SetActive(state);
    }
    void ShowJoystickCanvas(bool state)
    {
        joystickCanvas.SetActive(state);
        joystickCanvas.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
}
