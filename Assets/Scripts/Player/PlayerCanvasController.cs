using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCanvasController : MonoBehaviour
{
    private Camera mainCamera;
    float weaponUpgradeTextTime = 1.25f;
    Sequence moveTextSequence;
    [SerializeField] TextMeshProUGUI damagedText;
    float damageValueTextTime = 0.5f;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Start()
    {
        ClearColor(damagedText);
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                            mainCamera.transform.rotation * Vector3.up);

    }

    public void ShowDamageValueText(float damageValue)
    {
        Transform tempTextObject = Instantiate
            (damagedText.transform,
             damagedText.transform.position,
             damagedText.transform.rotation,
             transform);
        tempTextObject.GetComponent<TextMeshProUGUI>().text = ((int)damageValue).ToString();
        Destroy(tempTextObject.gameObject, 1.1f * damageValueTextTime);

        tempTextObject.DOLocalMoveY(150, damageValueTextTime).
          SetEase(Ease.OutQuart).Play().SetAutoKill();
        Sequence fadeTextSequence = DOTween.Sequence();
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f * damageValueTextTime).
            SetEase(Ease.OutQuart));
        fadeTextSequence.AppendInterval(0.4f * damageValueTextTime);
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(0, 0.1f * damageValueTextTime).
            SetEase(Ease.OutQuart)).Play().SetAutoKill();

    }

    public void ShowWeaponUpgradeText(TextMeshProUGUI text)
    {
        Transform textTransform = text.transform;

        Transform tempTextObject = Instantiate
            (textTransform,
             textTransform.position,
             textTransform.rotation,
             this.transform);
        Destroy(tempTextObject.gameObject, 1.1f * weaponUpgradeTextTime);

        
        tempTextObject.localScale = Vector3.zero;

        moveTextSequence = DOTween.Sequence().SetAutoKill().Play();

        moveTextSequence.Append(
            tempTextObject.DOLocalMoveY(80, 0.5f * weaponUpgradeTextTime).
            SetEase(Ease.InOutSine));

        moveTextSequence.Join(
            tempTextObject.DOScale(1.5f, 0.5f * weaponUpgradeTextTime).
            SetEase(Ease.OutQuart));

        moveTextSequence.Join(
            tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f * weaponUpgradeTextTime).
            SetEase(Ease.InOutSine));

        moveTextSequence.Append(
             tempTextObject.DOLocalMoveY(200, 0.5f * weaponUpgradeTextTime).
            SetEase(Ease.InOutSine));

        moveTextSequence.Join(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f * weaponUpgradeTextTime).
           SetEase(Ease.InExpo));
    }

    public void ClearColor(TextMeshProUGUI tempText)
    {
        Color tempColor = tempText.color;
        tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, 0);
        tempText.color = tempColor;
    }

    private void OnDestroy()
    {
        moveTextSequence.Kill();
    }
}
