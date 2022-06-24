using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;

    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera     _mainCamera;
    private Ray        _ray;
    private RaycastHit _hit;
    private Transform  _hitTransform = null;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
            return;
        
        if (Input.GetMouseButton(0))
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
            StartCoroutine("CheckRayHit");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_hitTransform == null || _hitTransform.CompareTag("Tower") == false)
                towerDataViewer.OffPanel();

            _hitTransform = null;
        }
    }

    private IEnumerator CheckRayHit()
    {
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            _hitTransform = _hit.transform;
            
            if (_hit.transform.CompareTag("Tile"))
            {
                towerSpawner.SpawnTower(_hit.transform);
            }
            else if (_hit.transform.CompareTag("Tower"))
            {
                towerDataViewer.OnPanel(_hit.transform);
            }
        }
        yield break;
    }
}
