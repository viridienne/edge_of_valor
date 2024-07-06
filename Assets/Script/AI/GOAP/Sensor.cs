using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESensorType
{
    Player
}

public static class SensorUtil
{
    public static string GetTag(this ESensorType type)
    {
        return type switch
        {
            ESensorType.Player => "Player",
            _ => "NONE"
        };
    }
}

[RequireComponent(typeof(CircleCollider2D))]
public class Sensor : MonoBehaviour
{
    [SerializeField] private ESensorType _sensorType;
    [SerializeField] private float _radius;
    [SerializeField] private float _timeBetweenScans; // reevaluate target every x seconds
    
    private CircleCollider2D _cachedTrigger;

    private GameObject _target;
    private Vector3 _lastKnownPosition;
    private float _lastScanTime;
    public Vector3 TargetPosition => _target != null ? _target.transform.position : Vector3.zero;
    public bool IsTargetInRange => TargetPosition != Vector3.zero;
    
    public Action OnTargetChanged;

    private void Awake()
    {
        _cachedTrigger = GetComponent<CircleCollider2D>();
        _cachedTrigger.isTrigger = true;
        _cachedTrigger.radius = _radius;
    }

    private void Update()
    {
        if (Time.time - _lastScanTime > _timeBetweenScans)
        {
            _lastScanTime = Time.time;
            UpdateTargetPosition(_target);
        }
    }

    /// <summary>
    /// update when target is new or target position has changed
    /// </summary>
    /// <param name="target"></param>
    private void UpdateTargetPosition(GameObject target = null)
    {
        _target = target;
        if (IsTargetInRange &&
            (_lastKnownPosition != TargetPosition || _lastKnownPosition != Vector3.zero))
        {
            _lastKnownPosition = TargetPosition;
            OnTargetChanged?.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_sensorType.GetTag()))
        {
            UpdateTargetPosition(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_sensorType.GetTag()))
        {
            UpdateTargetPosition();
        }
    }
}
