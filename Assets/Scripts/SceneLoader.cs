using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CheckSdkReady();
    [SerializeField] YandexSDK yandexSDK;
    [SerializeField] Progress progress;
    [SerializeField] Loader objectsLoader;
    bool isSdkReady;

    private void Awake()
    {
        Debug.LogWarning("SceneLoaderAwake");
        ToggleSdk(false);
    }
    private void Start()
    {
#if UNITY_EDITOR
        ToggleSdk(true);
        objectsLoader.Loading();
#endif
    }
    private void Update()
    {
#if !UNITY_EDITOR
if (!isSdkReady) 
        CheckSdkReady();
#endif
    }
    //из jslib
    public void ToggleSdkReady()
    {
        Debug.Log("ToggleSdkReady");
        isSdkReady = true;
        ToggleSdk(true);
        //objectsLoader.Loading();
    }

    void ToggleSdk(bool state)
    {       
        yandexSDK.gameObject.SetActive(state);
        progress.gameObject.SetActive(state);
        
    }
    
}


