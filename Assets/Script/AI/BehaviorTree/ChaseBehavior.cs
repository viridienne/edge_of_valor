using System;
using UnityEngine;

public class ChaseBehavior : IBehavior
{
    private readonly Transform _transform;
    private readonly Transform _target;
    private readonly float _speed;
    private readonly float _attackRange;
    private readonly BSensor _sensor;
    public bool IsLoop { get; }
    
    public ChaseBehavior(Transform transform, Transform target, bool isLoop, float speed = 1.0f, float attackRange = 1.0f, BSensor sensor = null)
    {
        _transform = transform;
        _target = target;
        IsLoop = isLoop;
        _speed = speed;
        _attackRange = attackRange;
        _sensor = sensor;
    }

    public BNodeState Process()
    {
        Debug.Log("ChaseBehavior");
        if (_target == null ||_sensor == null || _sensor != null && !_sensor.IsPlayerInRange) 
        {
            return BNodeState.FAILURE;
        }

        var direction = _target.position - _transform.position;
        if(Vector2.Distance(_transform.position, _target.position) < _attackRange)
        {
           return BNodeState.SUCCESS;
        }

        _transform.position += direction.normalized * (_speed * Time.deltaTime);
        return BNodeState.RUNNING;
    }

    public void Reset()
    {
        
    }
}