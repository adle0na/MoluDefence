using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum WeaponState { SearchTarget = 0, AttackToTarget}
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject   projectilePrefab;
    [SerializeField]
    private Transform    spawnPoint;
    [SerializeField]
    private float        attackRate  = 0.5f;
    [SerializeField]
    private float        attackRange = 2.0f;
    [SerializeField]
    private int          attackDamage = 1;
    
    private WeaponState  _weaponState = WeaponState.SearchTarget;
    private Transform    _attackTarget = null;
    private EnemySpawner _enemySpawner;

    public void Setup(EnemySpawner enemySpawner)
    {
        this._enemySpawner = enemySpawner;

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
            float closestDistSqr = Mathf.Infinity;

            for (int i = 0; i < _enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(_enemySpawner.EnemyList[i].transform.position, transform.position);

                if (distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    _attackTarget = _enemySpawner.EnemyList[i].transform;
                }
            }

            if (_attackTarget != null)
                ChangeState(WeaponState.AttackToTarget);

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (_attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            float distance = Vector3.Distance(_attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                _attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(attackRate);

            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        clone.GetComponent<Projectile>().Setup(_attackTarget, attackDamage);
    }
}
