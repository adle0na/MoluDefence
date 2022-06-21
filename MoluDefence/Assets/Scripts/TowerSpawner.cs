using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private EnemySpawner enemySpawner;

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)
            return;

        tile.IsBuildTower = true;

        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }

}
