using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3   distance = Vector3.down * 20.0f;
    
    private Transform     _targetTransform;
    private RectTransform _rectTransform;

    public void Setup(Transform target)
    {
        _targetTransform = target;
        _rectTransform   = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (_targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPosition  = Camera.main.WorldToScreenPoint(_targetTransform.position);
        _rectTransform.position = screenPosition + distance;
    }
}
