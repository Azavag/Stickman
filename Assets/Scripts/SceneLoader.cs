using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CheckSdkReady();
    [SerializeField] YandexSDK yandexSDK;
    [SerializeField] Progress progress;
    [SerializeField] bool isSdkReady;


    private void Update()
    {
#if !UNITY_EDITOR
        CheckSdkReady();
        if (isSdkReady)
        {       
            progress.gameObject.SetActive(true);
        }
        if(isSdkReady && yandexSDK.dataIsLoaded)
            SceneManager.LoadScene(1);
#endif
#if UNITY_EDITOR
        progress.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
#endif

    }

    public void ToggleSdkReady()
    {
        isSdkReady = true;
        yandexSDK.gameObject.SetActive(true);
        progress.gameObject.SetActive(true);
    }
    
}


