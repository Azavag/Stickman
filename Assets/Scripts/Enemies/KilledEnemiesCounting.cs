using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledEnemiesCounting : MonoBehaviour
{
    [SerializeField] MoneyManager moneyManager;
    Dictionary<EnemyType, int> enemyKilledList = new Dictionary<EnemyType, int>();
    int levelMoneyCount;
    [SerializeField] int moneyForKill = 2;
    [SerializeField] int bossMoneyMultiplier = 5, minibossMoneyMultiplier = 3;
    private void Awake()
    {
        EventManager.EnemyTypeDied += OnEnemyTypeDied;
        EventManager.GameReseted += OnGameReset;
    }

    void Start()
    {
        levelMoneyCount = 0;
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            enemyKilledList.Add(enemyType, 0);
        }
    }
    private void OnEnemyTypeDied(EnemyType enemyType)
    {
        enemyKilledList[enemyType]++;
        int tempMoney = 0;
        switch (enemyType)
        {
            case EnemyType.Regular:
                tempMoney = moneyForKill;
                break;
            case EnemyType.Miniboss:
                //levelMoneyCount += moneyForKill * minibossMoneyMultiplier;
                tempMoney = moneyForKill * minibossMoneyMultiplier;
                break;
            case EnemyType.Boss:
                //levelMoneyCount += moneyForKill * bossMoneyMultiplier;
                tempMoney = moneyForKill * bossMoneyMultiplier;
                break;
            default:
                break;
        }
        levelMoneyCount += tempMoney;
        moneyManager.UpdateMoneyCount(tempMoney);
    }
    public int GetLevelMoneyCount()
    {
        return levelMoneyCount;
    }
    private void OnGameReset()
    {
        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            enemyKilledList[enemyType] = 0;
        }
        levelMoneyCount = 0;
    }   

    public int GetKilledTypeCount(EnemyType enemyType)
    {
        return enemyKilledList[enemyType];
    }

    private void OnDestroy()
    {
        EventManager.EnemyTypeDied -= OnEnemyTypeDied;
        EventManager.GameReseted -= OnGameReset;
    }

}
