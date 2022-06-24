using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D _movement2D;
    private Transform  _target;
    private float      _damage;

    public void Setup(Transform target, float damage)
    {
        _movement2D  = GetComponent<Movement2D>();
        this._target = target;
        this._damage = damage;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _movement2D.MoveTo(direction);
        }
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        if (collision.transform != _target)
            return;
        
        collision.GetComponent<EnemyHP>().TakeDamage(_damage);
        Destroy(gameObject);
    }
}


