using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    [Header("��������� �������")]
    [SerializeField] AnimationCurve levelsEnemiesCountCurve;
    [SerializeField] int maxLevelsCount;                    //������������ �������
    [SerializeField] int maxEnemiesCountOnLevel;            //������������ ���������� ����� �� ������(�� 300 ������)
    [SerializeField] int minEnemiesCountOnLevel;            //����������� ���������� ����� �� ������ (�� ������ ������)
    [Header("��������� ����")]
    [SerializeField] AnimationCurve timesBetweenSpawnsCurve;
    [Header("����� ������")]
    [SerializeField] int bossSpawnPercent;
   
    void Start()
    {
        SetupLevelCurve();           
    }
    void SetupLevelCurve()
    {
        Keyframe[] keys = new Keyframe[4];
        keys[0] = new Keyframe(0, minEnemiesCountOnLevel);                //����� ��� ������� ������
        keys[1] = new Keyframe((0.25f * maxLevelsCount)-1, 100);
        keys[2] = new Keyframe((0.6f * maxLevelsCount)-1, 300);
        keys[keys.Length - 1] = new Keyframe(maxLevelsCount - 1, maxEnemiesCountOnLevel);   //����� ��� ���������� ������
        levelsEnemiesCountCurve = new AnimationCurve(keys);
        levelsEnemiesCountCurve.postWrapMode = WrapMode.Clamp;
    }
    
    public int SetupMinibosses(int currentLevel)
    {
        switch (currentLevel)
        {
            case >= 0 and <= 4: return 0;
            case > 4 and <= 20: return 1;
            case > 20 and <= 40: return 2;
            case > 40 and <= 60: return 3;
            case > 60 and <= 80: return 4;
            case > 80 and < 100: return 5;
            case > 100: return 6;
            default: return 0;
        }
    }
    public int SetupBosses(int currentLevel)
    {
        switch (currentLevel)
        {
            case >= 0: return 1;
            default: return 0;
        }
    }
    public int GetLevelEnemiesCount(int levelNumber)
    {
        return (int)levelsEnemiesCountCurve.Evaluate(levelNumber);
    }

    public AnimationCurve GetTimingSpawnCurve()
    {
        return timesBetweenSpawnsCurve;
    }
    public int GetBossSpawnPercent()
    {
        return bossSpawnPercent;
    }
}
