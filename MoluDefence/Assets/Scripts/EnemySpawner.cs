using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Value")]
    [SerializeField]
    private GameObject  enemyPrefab;
    [SerializeField]
    private GameObject  enemyHpSlider;
    [SerializeField]
    private Transform   canvasTransform;
    [SerializeField]
    private float       spawnTime;
    [SerializeField]
    private int         maxEnemyCount = 100;
    [SerializeField]
    private Transform[] wayPoints;
    
    private List<Enemy> _enemyList;
    public List<Enemy> EnemyList => _enemyList;
    
    [Header("Player Value")]
    [SerializeField]
    private PlayerHP    playerHP;
    [SerializeField]
    private PlayerGold  PlayerGold;
    
    private void Awake()
    {
        _enemyList = new List<Enemy>();
        
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy      enemy = clone.GetComponent<Enemy>();
            
            enemy.Setup(this, wayPoints);
            _enemyList.Add(enemy);

            SpawnEnemyHpSlider(clone);
            
            yield return new WaitForSeconds(spawnTime);
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
        
        _enemyList.Remove(enemy);
        
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
