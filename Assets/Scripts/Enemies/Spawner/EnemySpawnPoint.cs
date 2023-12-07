using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    public bool canSpawn;
    BoxCollider CheckTrigger;
    float spawnRadius;

    private void Awake()
    {
        CheckTrigger = GetComponent<BoxCollider>();
    }
    void Start()
    {
        spawnRadius = CheckTrigger.size.x/2;
        canSpawn = true;
    }


    public void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 randomPoint = GenerateRandomPointInSphere(transform.position, spawnRadius);
        GameObject tempEnemy = Instantiate(enemyPrefab,
            randomPoint,
            Quaternion.identity);
        //Поворот в сторону игрока
        Vector3 lookDirection = playerTransform.position - tempEnemy.transform.position;
        lookDirection.y = 0.0f;
        tempEnemy.transform.rotation = Quaternion.LookRotation(lookDirection);
        if(tempEnemy.TryGetComponent(out EnemyController enemy))
        {
            enemy.UnderGround(true);
            enemy.SetTargetTransform(playerTransform);          
        }
        if (tempEnemy.TryGetComponent(out BossController boss))
        {
            boss.SetAntiPlayerDirection();          
        }
    }
    Vector3 GenerateRandomPointInSphere(Vector3 center, float radius)
    {
        float randomAngle = Random.Range(0.0f, Mathf.PI * 2);
        float randomRadius = Random.Range(0, radius);
        float x = center.x + randomRadius * Mathf.Cos(randomAngle);
        float z = center.z + randomRadius * Mathf.Sin(randomAngle);
        float y = center.y - 2f;
        return new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canSpawn = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canSpawn = true;
        }
    }
}
