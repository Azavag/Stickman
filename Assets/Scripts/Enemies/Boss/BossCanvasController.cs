using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossCanvasController : EnemyCanvasController
{
    [SerializeField] Transform immortalityTextObject;
    [SerializeField] Transform unweaponTextObject;

    float immortalityTextTime = 0.5f;
    float unweaponTextTime = 1.2f;

    protected override void Start()
    {
        base.Start();
        ClearColor(immortalityTextObject);
        ClearColor(unweaponTextObject);
    }
    public void ShowImmortalText()
    {
        Transform tempTextObject = Instantiate
         (immortalityTextObject,
          immortalityTextObject.position,
          immortalityTextObject.rotation,
          transform);
        Destroy(tempTextObject.gameObject, immortalityTextTime);
        tempTextObject.DOLocalMoveY(30, immortalityTextTime).
          SetEase(Ease.OutQuart).Play().SetAutoKill();
        Sequence fadeTextSequence = DOTween.Sequence();
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f * immortalityTextTime).
            SetEase(Ease.OutQuart));
        fadeTextSequence.AppendInterval(0.4f * immortalityTextTime);
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(0, 0.1f * immortalityTextTime).
            SetEase(Ease.OutQuart)).Play().SetAutoKill();
    }
    public void ShowUnweaponText()
    {
        unweaponTextObject.DOLocalMoveY(30, unweaponTextTime).
           SetEase(Ease.OutQuart).Play().OnComplete(RewindUnweaponTween).SetAutoKill();

        unweaponTextObject.GetComponent<TextMeshProUGUI>().DOFade(1, unweaponTextTime/2).
            SetEase(Ease.OutQuart).Play().SetAutoKill();
    }
    void RewindUnweaponTween()
    {
        ClearColor(unweaponTextObject);
        unweaponTextObject.DORewind();
    }

    private void OnDestroy()
    {
        immortalityTextObject.DOKill();
        unweaponTextObject.DOKill();
    }
}
