using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BSensor : MonoBehaviour
{
    [SerializeField] private ESensorType _sensorType;
    [SerializeField] private float _radius;
    public bool IsPlayerInRange { get; private set; }
    private CircleCollider2D _cachedTrigger;
    private void Awake()
    {
        _cachedTrigger = GetComponent<CircleCollider2D>();
        _cachedTrigger.isTrigger = true;
        _cachedTrigger.radius = _radius;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(_sensorType.GetTag()))
        {
            Debug.LogError("Player in range");
            IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_sensorType.GetTag()))
        {
            Debug.LogError("Player out range");
            IsPlayerInRange = false;
        }
    }
}