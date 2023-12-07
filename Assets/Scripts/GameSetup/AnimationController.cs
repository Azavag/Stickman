using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;

public class AnimationController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] InterfaceNavigationController navigationController;
    [SerializeField] KilledEnemiesCounting enemiesCounting;
    [SerializeField] AdvManager advManager;
    [Header("Панель конца уровня")]
    [SerializeField] Transform EndLevelPanel;
    [SerializeField] float inScaleAnimTime;
    [SerializeField] float outScaleAnimTime;
    [SerializeField] Transform[] textObjects;
    [SerializeField] Transform acceptButton;
    [SerializeField] TextMeshProUGUI[] killedTypesTexts;
    [SerializeField] TextMeshProUGUI earnedCountText;   
    Sequence textShowSequence;
    float textShowAnimTime = 0.25f;
    [Header("Начало уровня")]
    [SerializeField] TextMeshProUGUI startLevelText;
    [SerializeField] float fadeAnimTime;
    [Header("Панель настроек")]
    [SerializeField] Transform SettingsPanel;
    [SerializeField] float settingsInScaleAnimTime;
    [SerializeField] float settingsOutScaleAnimTime;
    [SerializeField] Ease settingAnimEase;
    void Start()
    {      
        AnimateStartLevelText();
    }

    void AnimateStartLevelText()
    {
        startLevelText.DOFade(0.5f, fadeAnimTime).
            SetLoops(-1, LoopType.Yoyo).Play();
        startLevelText.transform.DOScale(0.9f, fadeAnimTime).
            SetLoops(-1, LoopType.Yoyo).Play();
    }
    public void ShowSettingsPanelPanel()
    {
        SettingsPanel.localScale = Vector3.zero;
        SettingsPanel.DOScale(1f, settingsInScaleAnimTime).
            SetEase(settingAnimEase).Play().SetAutoKill().SetUpdate(true);        
    }
    public void HideSettingsPanel(TweenCallback tweenCallback)
    {
        SettingsPanel.DOScale(0, settingsOutScaleAnimTime).
            SetEase(settingAnimEase).Play().OnComplete(tweenCallback).SetAutoKill().SetUpdate(true);
    }

    public void ShowEndLevelPanel()  
    {
        EndLevelPanel.localScale = Vector3.zero;
        EndLevelPanel.DOScale(1f, inScaleAnimTime).
           SetEase(Ease.OutBounce).OnComplete(ShowTextsInEndPanel).SetAutoKill();
        textShowSequence = DOTween.Sequence().SetAutoKill();

        foreach (Transform textObject in textObjects)
        {
            textObject.transform.localScale = Vector3.zero;          
            textShowSequence.Append(textObject.DOScale(1, textShowAnimTime).SetEase(Ease.OutQuad));
        }
        acceptButton.transform.localScale = Vector3.zero;
        textShowSequence.OnComplete(Callback);
        for (int tempCounter = 0; tempCounter < killedTypesTexts.Length; tempCounter++)
        {
            killedTypesTexts[tempCounter].text =
                enemiesCounting.GetKilledTypeCount((EnemyType)tempCounter).ToString();
        }
        earnedCountText.text = enemiesCounting.GetLevelMoneyCount().ToString();
        EndLevelPanel.DOPlay();
    }
    void Callback()
    {
        advManager.ShowAdv();
        StartCoroutine(ShowAcceptButton());
    }
    IEnumerator ShowAcceptButton()
    {
        yield return new WaitForSeconds(0.75f);       
        acceptButton.DOScale(1, textShowAnimTime).SetEase(Ease.OutQuad).SetAutoKill().Play();
    }

 
    public void HideEndLevelPanel(TweenCallback tweenCallback)
    {
        EndLevelPanel.DOScale(0, outScaleAnimTime).
            SetEase(Ease.InBounce).Play().OnComplete(tweenCallback).SetAutoKill();
    }
    void ShowTextsInEndPanel()
    {      
        textShowSequence.Play();
    }


}
