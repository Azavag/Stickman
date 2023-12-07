using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject miniBossPrefab;
    [SerializeField] SpawnData spawnData;
    [SerializeField] GameController gameController;
    [SerializeField] EnemyData enemyData;
    [Header("—павн")]
    [SerializeField] EnemySpawnPoint[] spawnPoints;
    [SerializeField] float timeBeforeLevel;
    [SerializeField] bool isMinibossOnLevel;
    [SerializeField] bool isBossOnLevel;
    float timeBetweenSpawns;
    int deadEnemiesOnLevel;
    List<GameObject> enemies = new List<GameObject>();
    int minibossesCount;
    int bossesCount;
    int killToSpawnMiniboss, killToSpawnBoss;
    EnemySpawnPoint currentSpawnPoint;
    [Header("—лайдер")]
    [SerializeField] Slider waveSlider;
    int enemiesOnLevel;
    int levelNumber;
    private IEnumerator spawnRoutine;

    private void Awake()
    {
        EventManager.EnemyDied += OnEnemyDied;
        EventManager.PlayerDied += OnPlayerDied;
    }

    public void StartLevel()
    {
        levelNumber = gameController.GetCurrentLevelNumber();
        spawnRoutine = SpawnEnemiesCoroutine();
        SetEnemiesStats();
        StartCoroutine(StartLevelCoroutine());
    }
    void SetEnemiesStats()
    {
        enemyData.CheckLevelForHealth(levelNumber);
        enemyPrefab.GetComponent<EnemyController>().SetMaxHealth(enemyData.GetEnemyHealth());
        miniBossPrefab.GetComponent<EnemyController>().SetMaxHealth(enemyData.GetMinibossHealth());
        bossPrefab.GetComponent<EnemyController>().SetMaxHealth(enemyData.GetBossHealth());

        enemyData.CheckLevelForRotationSpeed(levelNumber);
        miniBossPrefab.GetComponent<BossController>().SetRotationSpeed(enemyData.GetMinibossRotation());
        bossPrefab.GetComponent<BossController>().SetRotationSpeed(enemyData.GetBossRotation());

        enemyData.CheckLevelForDurability(levelNumber);
        miniBossPrefab.GetComponent<BossController>().SetWeaponDurablity(enemyData.GetMinibossWeaponDurability());
        bossPrefab.GetComponent<BossController>().SetWeaponDurablity(enemyData.GetBossWeaponDurability());        
    }

    IEnumerator StartLevelCoroutine()
    {
        waveSlider.value = 0;      
        LevelSetup(levelNumber);      
        yield return new WaitForSeconds(timeBeforeLevel);
        StartCoroutine(spawnRoutine);
        yield return null;
    }
    void LevelSetup(int levelNumber)
    {
        enemiesOnLevel = spawnData.GetLevelEnemiesCount(levelNumber);
        enemies.Clear();
        for (int i = 0;i < enemiesOnLevel; i++)
        {
            enemies.Add(enemyPrefab);
            
        }
        if(isMinibossOnLevel)
        {
            minibossesCount = spawnData.SetupMinibosses(gameController.GetCurrentLevelNumber());
            killToSpawnMiniboss = (int)(spawnData.GetBossSpawnPercent() / 100f * enemiesOnLevel / (minibossesCount + 1));
            enemiesOnLevel += minibossesCount;
        }
        if (isBossOnLevel)
        {
            bossesCount = spawnData.SetupBosses(gameController.GetCurrentLevelNumber());
            killToSpawnBoss = (int)(spawnData.GetBossSpawnPercent() / 100f * enemiesOnLevel);
            enemiesOnLevel += bossesCount;
        }          
        waveSlider.maxValue = enemiesOnLevel;
        deadEnemiesOnLevel = 0;
    }
   
    IEnumerator SpawnEnemiesCoroutine()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            currentSpawnPoint = ChooseRandomSpawnPointNumber();
            currentSpawnPoint.SpawnEnemy(enemies[i]);
            timeBetweenSpawns = spawnData.GetTimingSpawnCurve().Evaluate((float)i / enemies.Count);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }  
    EnemySpawnPoint ChooseRandomSpawnPointNumber()
    {
        int number = Random.Range(0, spawnPoints.Length);
        while (!spawnPoints[number].canSpawn)
        {
            number = Random.Range(0, spawnPoints.Length);
        }
        return spawnPoints[number];
    }

    void SpawnMiniboss()
    {
        currentSpawnPoint.SpawnEnemy(miniBossPrefab);
    }
    void SpawnBoss()
    {
        currentSpawnPoint.SpawnEnemy(bossPrefab);
    }
    
    void OnEnemyDied()
    {
        waveSlider.value++;
        deadEnemiesOnLevel++;
        if(isMinibossOnLevel && (deadEnemiesOnLevel % killToSpawnMiniboss) == 0 && minibossesCount > 0)
        {
            SpawnMiniboss();
            minibossesCount--;
        }
        if (isBossOnLevel && (deadEnemiesOnLevel % killToSpawnBoss) == 0 && bossesCount > 0)
        {
            SpawnBoss();
            bossesCount--;
        }
        if (deadEnemiesOnLevel == enemiesOnLevel)
        {
            gameController.EndLevel(true);
        }
    }
    void OnPlayerDied()
    {
        StopAllCoroutines();    
    }
    void OnDestroy() 
    {
        EventManager.EnemyDied -= OnEnemyDied;
    }
}
