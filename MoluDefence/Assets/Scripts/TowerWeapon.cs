using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum WeaponType  {Cannon = 0, Laser, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, }
public class TowerWeapon : MonoBehaviour
{
    [Header("TowerData")]
    [SerializeField]
    private TowerTemplate  towerTemplate;
    
    [SerializeField]
    private Transform      spawnPoint;

    [SerializeField]
    private WeaponType     weaponType;
    
    [Header("Cannon")]
    [SerializeField]
    private GameObject     projectilePrefab;

    [Header("Laser")] 
    [SerializeField]
    private LineRenderer   lineRenderer;

    [SerializeField]
    private Transform      hitEffect;

    [SerializeField]
    private LayerMask      targetLayer;

    private int            _level = 0;
    private PlayerGold     _playerGold;
    private SpriteRenderer _spriteRenderer;
    private WeaponState    _weaponState  = WeaponState.SearchTarget;
    private Transform      _attackTarget = null;
    private EnemySpawner   _enemySpawner;
    private Tile           _ownerTile;
    
    public Sprite TowerSprite => towerTemplate.weapon[_level].sprite;
    public float Damage       => towerTemplate.weapon[_level].damage;
    public float Rate         => towerTemplate.weapon[_level].rate;
    public float Range        => towerTemplate.weapon[_level].range;
    public int Level          => _level + 1;
    public int MaxLevel       => towerTemplate.weapon.Length;
    
    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        _spriteRenderer     = GetComponent<SpriteRenderer>();
        
        this._enemySpawner  = enemySpawner;

        this._playerGold    = playerGold;

        this._ownerTile     = ownerTile;
        
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(_weaponState.ToString());

        _weaponState = newState;

        StartCoroutine(_weaponState.ToString());
    }

    private void Update()
    {
        if (_attackTarget != null)
            RotateToTarget();

    }

    private void RotateToTarget()
    {
        var enemyPos = _attackTarget.position;
        var thisPos  = transform.position;
        
        float dx = enemyPos.x - thisPos.x;
        float dy = enemyPos.y - thisPos.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            _attackTarget = FindClosestAttackTarget();

            if (_attackTarget != null)
            {
                switch (weaponType)
                {
                    case WeaponType.Cannon:
                        ChangeState(WeaponState.TryAttackCannon);
                        break;
                    case WeaponType.Laser:
                        ChangeState(WeaponState.TryAttackLaser);
                        break;
                }
            }
            yield return null;
        }
    }
    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            yield return new WaitForSeconds(towerTemplate.weapon[_level].rate);
            
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        EnableLaser();

        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            SpawnLaser();

            yield return null;
        }
    }
    
    private Transform FindClosestAttackTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        for (int i = 0; i < _enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(_enemySpawner.EnemyList[i].transform.position, transform.position);

            if (distance <= towerTemplate.weapon[_level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                _attackTarget = _enemySpawner.EnemyList[i].transform;
            }
        }
        return _attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        if (_attackTarget == null)
        {
            return false;
        }

        float distance = Vector3.Distance(_attackTarget.position, transform.position);
        
        if (distance > towerTemplate.weapon[_level].range)
        {
            _attackTarget = null;
            return false;
        }
        
        return true;
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        clone.GetComponent<Projectile>().Setup(_attackTarget, towerTemplate.weapon[_level].damage);
    }

    private void SpawnLaser()
    {
        Vector3 direction = _attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction,
                                                    towerTemplate.weapon[_level].range, targetLayer);
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == _attackTarget)
            {
                lineRenderer.SetPosition(0, spawnPoint.position);
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                hitEffect.position = hit[i].point;
                _attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[_level].damage * Time.deltaTime);
            }
        }
    }

    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    public bool Upgrade()
    {
        if (_playerGold.CurrentGold < towerTemplate.weapon[_level + 1].cost)
        {
            return false;
        }
        
        _level++;
        _spriteRenderer.sprite   = towerTemplate.weapon[_level].sprite;
        _playerGold.CurrentGold -= towerTemplate.weapon[_level].cost;

        if (weaponType == WeaponType.Laser)
        {
            lineRenderer.startWidth = 0.05f + _level * 0.05f;
            lineRenderer.endWidth   = 0.05f;
        }
        return true;
    }

    public void Sell()
    {
        _playerGold.CurrentGold += towerTemplate.weapon[_level].sell;
        _ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }
}
