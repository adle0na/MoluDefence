using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int          _wayPointCount;
    private Transform[]  _wayPoints;
    private int          _currentIndex = 0;
    private Movement2D   _movement2D;
    private EnemySpawner _enemySpawner;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        _movement2D        = GetComponent<Movement2D>();
        this._enemySpawner = enemySpawner;

        _wayPointCount     = wayPoints.Length;
        this._wayPoints    = new Transform[_wayPointCount];
        this._wayPoints    = wayPoints;

        transform.position = _wayPoints[_currentIndex].position;

        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.02f * _movement2D.MoveSpeed)
                NextMoveTo();

            yield return null;
        }
        
    }

    private void NextMoveTo()
    {
        if (_currentIndex < _wayPointCount - 1)
        {
            transform.position = _wayPoints[_currentIndex].position;
            _currentIndex++;
            Vector3 direction = (_wayPoints[_currentIndex].position - transform.position).normalized;
            _movement2D.MoveTo(direction);
        }
        else
            OnDie();
    }

    public void OnDie()
    {
        _enemySpawner.DestroyEnemy(this);
    }
}
