using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Value")]
    [SerializeField]
    private GameObject  enemyHpSlider;
    [SerializeField]
    private Transform   canvasTransform;
    [SerializeField]
    private Transform[] wayPoints;

    private Wave        currentWave;
    private int         currentEnemyCount;
    private List<Enemy> enemyList;
    public List<Enemy>  EnemyList => enemyList;
    public int CurrentEnemyCount  => currentEnemyCount;
    public int MaxEnemyCount      => currentWave.maxEnemyCount;
    
    [Header("Player Value")]
    [SerializeField]
    private PlayerHP    playerHP;
    [SerializeField]
    private PlayerGold  PlayerGold;
    
    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;
        
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int   enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy      enemy = clone.GetComponent<Enemy>();
            
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHpSlider(clone);
            
            spawnEnemyCount++;
            
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
    
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Die)
        {
            PlayerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        
        enemyList.Remove(enemy);
        
        Destroy(enemy.gameObject);
    }
    
    private void SpawnEnemyHpSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHpSlider);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;
        
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
