using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject   towerPrefab;

    [SerializeField]
    private int          towerBuildGold = 50;
    
    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private PlayerGold   PlayerGold;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > PlayerGold.CurrentGold)
        {
            return;
        }
        
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)
            return;

        tile.IsBuildTower = true;

        PlayerGold.CurrentGold -= towerBuildGold;

        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }

}
