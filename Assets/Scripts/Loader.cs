using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] GameObject canvases, sdk, controllers, player;
    [SerializeField] bool inEditor;
    [SerializeField] Progress progress;
    [SerializeField] YandexSDK yandex;

    private void Awake()
    {
        sdk.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        canvases.SetActive(false);
        controllers.SetActive(false);
    }
    private void Start()
    {
        Application.targetFrameRate = -1;
#if UNITY_EDITOR
        yandex.gameObject.SetActive(true);
        progress.gameObject.SetActive(true);
#endif
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        sdk.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        canvases.SetActive(true);
        controllers.SetActive(true);
        yield return null;
    }
}
