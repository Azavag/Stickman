using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action EnemyDied;
    public static event Action<EnemyType> EnemyTypeDied;
    public static event Action PlayerDied;
    public static event Action GameReseted;

    public static void InvokeEnemyDied()
    {
        EnemyDied?.Invoke();
    }
    public static void InvokeEnemyTypeDied(EnemyType type)
    {
        EnemyTypeDied?.Invoke(type);
    }
    public static void InvokePlayerDied()
    {
        PlayerDied?.Invoke();
    }
    public static void InvokeGameReseted()
    {
        GameReseted?.Invoke();
    }


}

