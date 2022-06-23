using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float          maxHP;
    private float          currentHP;
    private bool           _isDie = false;
    private Enemy          _enemy;
    private SpriteRenderer _spriteRenderer;

    public float MaxHP     => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP       = maxHP;
        _enemy          = GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (_isDie == true)
            return;

        currentHP -= damage;
        
        StopCoroutine("HitMotion");
        StartCoroutine("HitMotion");

        if (currentHP <= 0)
        {
            _isDie = true;
            _enemy.OnDie(EnemyDestroyType.Die);
        }
    }

    private IEnumerator HitMotion()
    {
        Color color = _spriteRenderer.color;

        color.a = 0.4f;
        _spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f;
        _spriteRenderer.color = color;
    }
}
