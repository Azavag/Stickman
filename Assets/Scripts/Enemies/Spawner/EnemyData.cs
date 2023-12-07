using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyHealthData
{
    public int level;
    public float enemyMaxHealthData;
    public float minibossMaxHealthData;
    public float bossMaxHealthData;
}
[Serializable]
public class EnemyWeaponRotationSpeedData
{
    public int level;
    public float minibossWeaponRotationData;
    public float bossWeaponRotationData;
}
[Serializable]
public class EnemyWeaponDurabilitydData
{
    public int level;
    public float minibossWeaponDurabilityData;
    public float bossWeaponDurabilityData;
}

public class EnemyData : MonoBehaviour
{
    [SerializeField] EnemyHealthData[] healthGrades;
    float enemyMaxHealth;
    float minibossMaxHealth;
    float bossMaxHealth;
    [SerializeField] EnemyWeaponRotationSpeedData[] rotationSpeedGrades;
    float bossWeaponRotation;
    float minibossWeaponRotation;
    [SerializeField] EnemyWeaponDurabilitydData[] weaponDurabilitydGrades;
    float bossWeaponDurability;
    float minibossWeaponDurability;


    public float GetEnemyHealth()
    {
        return enemyMaxHealth;
    }
    public float GetMinibossHealth()
    {
        return minibossMaxHealth;
    }
    public float GetBossHealth()
    {
        return bossMaxHealth;
    }
    public float GetBossRotation()
    {
        return bossWeaponRotation;
    }
    public float GetMinibossRotation()
    {
        return minibossWeaponRotation;
    }
    public float GetBossWeaponDurability()
    {
        return bossWeaponDurability;
    }
    public float GetMinibossWeaponDurability()
    {
        return minibossWeaponDurability;
    }
    public void CheckLevelForHealth(int level)
    {
        int lastElementNumber = healthGrades.Length - 1;
        for (int i = 0; i <= lastElementNumber; i++)
        {
            if (level >= healthGrades[lastElementNumber].level)
            {
                enemyMaxHealth = healthGrades[lastElementNumber].enemyMaxHealthData;
                minibossMaxHealth = healthGrades[lastElementNumber].minibossMaxHealthData;
                bossMaxHealth = healthGrades[lastElementNumber].bossMaxHealthData;
                return;
            }
            if (level >= healthGrades[i].level && level <= healthGrades[i + 1].level)
            {
                enemyMaxHealth = healthGrades[i].enemyMaxHealthData;
                minibossMaxHealth = healthGrades[i].minibossMaxHealthData;
                bossMaxHealth = healthGrades[i].bossMaxHealthData;
                return;
            }
        }
    }
    public void CheckLevelForRotationSpeed(int level)
    {
        int lastElementNumber = rotationSpeedGrades.Length - 1;
        for (int i = 0; i <= lastElementNumber; i++)
        {
            if(level >= rotationSpeedGrades[lastElementNumber].level)
            {
                minibossWeaponRotation = rotationSpeedGrades[lastElementNumber].minibossWeaponRotationData;
                bossWeaponRotation = rotationSpeedGrades[lastElementNumber].bossWeaponRotationData;
                return;
            }
            if (level >= rotationSpeedGrades[i].level && level <= rotationSpeedGrades[i+1].level)
            {
                minibossWeaponRotation = rotationSpeedGrades[i].minibossWeaponRotationData;
                bossWeaponRotation = rotationSpeedGrades[i].bossWeaponRotationData;
                return;
            }
        }
    }
    public void CheckLevelForDurability(int level)
    {
        int lastElementNumber = weaponDurabilitydGrades.Length - 1;
        for (int i = 0; i <= lastElementNumber; i++)
        {
            if (level >= weaponDurabilitydGrades[lastElementNumber].level)
            {
                minibossWeaponDurability = weaponDurabilitydGrades[lastElementNumber].minibossWeaponDurabilityData;
                bossWeaponDurability = weaponDurabilitydGrades[lastElementNumber].bossWeaponDurabilityData;
                return;
            }
            if (level >= weaponDurabilitydGrades[i].level && level <= weaponDurabilitydGrades[i + 1].level)
            {
                minibossWeaponDurability = weaponDurabilitydGrades[i].minibossWeaponDurabilityData;
                bossWeaponDurability = weaponDurabilitydGrades[i].bossWeaponDurabilityData;
                return;
            }
        }
    }
}
