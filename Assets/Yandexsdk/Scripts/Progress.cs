using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int levelNumber = 0;             //++
    public int moneyCount = 0;              //++
    public float musicVolume = 1;           //++
    public float effectsVolume = 1;         //++
    public int speedUpgradeLevel = 0;       //++
    public int rotationUpgradeLevel = 0;    //++
    public int damageUpgradeLevel = 0;      //++
    public float playerDamage = 10f;        //++
    public float playerSpeed = 70f;         //++
    public float playerRotation = 200f;     //++
    public int weaponModelNumber = 0;       //++
    public bool isShowDamageNumber = true;  //++
    
}


public class Progress : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public static Progress Instance;
    [SerializeField] YandexSDK yandexSDK;
   
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            yandexSDK.Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}



