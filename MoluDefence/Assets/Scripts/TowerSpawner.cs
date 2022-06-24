using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[]  towerTemplate;
    
    [SerializeField]
    private EnemySpawner     enemySpawner;

    [SerializeField]
    private PlayerGold       playerGold;

    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private bool       _isOnTowerButton  = false;
    private GameObject _followTowerClone = null;
    private int        _towerType;

    public void ReadyToSpawnTower(int type)
    {
        _towerType = type;
        
        if (_isOnTowerButton == true)
            return;
        
        if (towerTemplate[_towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        _isOnTowerButton  = true;
        _followTowerClone = Instantiate(towerTemplate[_towerType].followTowerPrefab);
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        if (_isOnTowerButton == false)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        _isOnTowerButton   = false;
        tile.IsBuildTower = true;

        playerGold.CurrentGold -= towerTemplate[_towerType].weapon[0].cost;

        Vector3 position  = tileTransform.position + Vector3.back;
        GameObject clone  = Instantiate(towerTemplate[_towerType].towerPrefab, position, Quaternion.identity);
        
        clone.GetComponent<TowerWeapon>().Setup(this, enemySpawner, playerGold, tile);
        
        OnBuffAllBuffTowers();
        
        Destroy(_followTowerClone);
        
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _isOnTowerButton = false;
                Destroy(_followTowerClone);
                break;
            }
            yield return null;
        }
        
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; ++ i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (weapon.WeaponType == WeaponType.Buff)
                weapon.OnBuffAroundTower();
        }
    }
}
