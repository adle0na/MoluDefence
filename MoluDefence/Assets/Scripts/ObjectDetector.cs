using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;

    private Camera       _mainCamera;
    private Ray          _ray;
    private RaycastHit   _hit;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            StartCoroutine("CheckRayHit");
        }
    }

    private IEnumerator CheckRayHit()
    {
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            if (_hit.transform.CompareTag("Tile"))
            {
                towerSpawner.SpawnTower(_hit.transform);
            }
        }
        yield break;
    }
}
