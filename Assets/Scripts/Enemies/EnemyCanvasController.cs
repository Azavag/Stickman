using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCanvasController : MonoBehaviour
{
    [SerializeField] Transform damageValueTextObject;
    protected Transform currentUsedTextObj;

    private Camera mainCamera;

    float damageValueTextTime = 0.5f;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    protected virtual void Start()
    {
        ClearColor(damageValueTextObject);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                            mainCamera.transform.rotation * Vector3.up);
    }

    public void ShowDamageValueText(float damageValue)
    {
        Transform tempTextObject = Instantiate
            (damageValueTextObject,
             damageValueTextObject.position,
             damageValueTextObject.rotation,
             transform);
        tempTextObject.GetComponent<TextMeshProUGUI>().text = ((int)damageValue).ToString();
        Destroy(tempTextObject.gameObject, damageValueTextTime);

        tempTextObject.DOLocalMoveY(30, damageValueTextTime).
          SetEase(Ease.OutQuart).Play().SetAutoKill();
        Sequence fadeTextSequence = DOTween.Sequence();
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f * damageValueTextTime).
            SetEase(Ease.OutQuart));
        fadeTextSequence.AppendInterval(0.4f * damageValueTextTime);
        fadeTextSequence.Append(tempTextObject.GetComponent<TextMeshProUGUI>().DOFade(0, 0.1f * damageValueTextTime).
            SetEase(Ease.OutQuart)).Play().SetAutoKill();
        
    }
    protected void ClearColor(Transform textObject)
    {
        Color tempColor = textObject.GetComponent<TextMeshProUGUI>().color;
        tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, 0);
        textObject.GetComponent<TextMeshProUGUI>().color = tempColor;
    }
    
   
    private void OnDestroy()
    {
        damageValueTextObject.DOKill();
    }
}
