using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject  enemyPrefab;
    [SerializeField]
    private float       spawnTime;
    [SerializeField]
    private Transform[] wayPoints;

    private List<Enemy> _enemyList;

    public List<Enemy> EnemyList => _enemyList;
    
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

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);

        Destroy(enemy.gameObject);
    }
}
