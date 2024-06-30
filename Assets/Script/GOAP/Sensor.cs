using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Sensor : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _timeBetweenScans;
    
    private CircleCollider2D _cachedTrigger;

    private GameObject _target;
    private Vector3 _lastKnownPosition;
    
    private void Start()
    {
        _cachedTrigger = GetComponent<CircleCollider2D>();
        _cachedTrigger.isTrigger = true;
        _cachedTrigger.radius = _radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
