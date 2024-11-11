using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDifficulty : MonoBehaviour
{
    public GameObject covid;
    public GameObject enemyTank;
    public GameObject tankTrap;
    public GameObject tower;
    public GameObject mortar;
    public GameObject healthPackage;
    public GameObject bonusPackage;
    public GameObject slowTimePackage;

    private float planeWidth;
    private float planeHeight;
    private Vector3 planeCenter;

    public float spawnInterval = 20.0f;
    public float healthPackageInterval = 30.0f;
    public float bonusPackageInterval = 10.0f;
    public float slowTimePackageInterval = 10.0f;
    public float difficultyIncreaseTime = 20.0f;
    public float lastDifficultyIncreaseTime = 0f;

    public float pursuerSpeed = 1.0f;

    public float groupSpawnRadius = 1f;
    public float groupSpawnChance = 0.75f; 

    void Start()
    {
        planeWidth = transform.localScale.x * 10;
        planeHeight = transform.localScale.z * 10;
        planeCenter = transform.position;

        RandomlySpawn(covid, 1.0f);
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnHealthPackages()); 
        StartCoroutine(SpawnBonusPackages()); 
        StartCoroutine(SpawnSlowTimePackages()); 
    }

    void Update()
    {
        if (Time.time - lastDifficultyIncreaseTime >= difficultyIncreaseTime)
        {
            IncreaseDifficulty();
            lastDifficultyIncreaseTime = Time.time;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (Random.Range(0f, 1f) < groupSpawnChance)
            {
                SpawnEnemyGroup();
            }
            else
            {
                GameObject[] enemyPrefabs = new GameObject[] { enemyTank, tankTrap, tower, mortar };
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                RandomlySpawn(randomEnemy, 0.0f);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnHealthPackages()
    {
        while (true)
        {
            RandomlySpawn(healthPackage, 1.0f); 
            yield return new WaitForSeconds(healthPackageInterval);
        }
    }

    IEnumerator SpawnBonusPackages()
    {
        while (true)
        {
            RandomlySpawn(bonusPackage, 0.5f); 
            yield return new WaitForSeconds(bonusPackageInterval);
        }
    }

    IEnumerator SpawnSlowTimePackages()
    {
        while (true)
        {
            RandomlySpawn(slowTimePackage, 0.5f); 
            yield return new WaitForSeconds(bonusPackageInterval);
        }
    }

    void RandomlySpawn(GameObject prefab, float height)
    {
        float randomX = Random.Range(-planeWidth / 2, planeWidth / 2);
        float randomZ = Random.Range(-planeHeight / 2, planeHeight / 2);
        Vector3 spawnPosition = new Vector3(randomX, height, randomZ) + planeCenter;

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    void SpawnEnemyGroup()
    {
        int groupSize = Random.Range(2, 5);
        Vector3 groupCenter = new Vector3(Random.Range(-planeWidth / 2, planeWidth / 2), 0.0f, Random.Range(-planeHeight / 2, planeHeight / 2)) + planeCenter;

        for (int i = 0; i < groupSize; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-groupSpawnRadius, groupSpawnRadius), 0, Random.Range(-groupSpawnRadius, groupSpawnRadius));
            Vector3 spawnPosition = groupCenter + randomOffset;

            GameObject[] enemyPrefabs = new GameObject[] { enemyTank, tankTrap, tower, mortar };
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            RandomlySpawn(randomEnemy, 0.0f);
        }
    }

    void IncreaseDifficulty()
    {
        spawnInterval -= spawnInterval > 5.0f ? 0.5f : 0.0f;
        pursuerSpeed += 0.15f;
    }
}
