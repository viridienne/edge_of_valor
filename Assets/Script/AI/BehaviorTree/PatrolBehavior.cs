using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : IBehavior
{
    protected readonly List<Vector3> _waypoints;
    protected int _currentWaypointIndex;
    protected readonly Transform _transform;
    protected readonly float _speed = 1.0f;
    protected SpriteRenderer _spriteRenderer;
    protected float _waitTime;
    protected float _lastMoveTime;
    public PatrolBehavior(Transform transform,SpriteRenderer spriteRenderer, List<Vector3> waypoints, float speed = 1.0f, float waitTime = 0.0f, bool isLoop = false)
    {
        _transform = transform;
        _waypoints = waypoints;
        _currentWaypointIndex = 0;
        _speed = speed;
        _spriteRenderer = spriteRenderer;
        _waitTime = waitTime;
        IsLoop = isLoop;
    }

    public bool IsLoop { get; }

    public BNodeState Process()
    {
        if (_waypoints.Count == 0)
        {
            return BNodeState.FAILURE;
        }
        if(_currentWaypointIndex >= _waypoints.Count)
        {
            if (!IsLoop)
            {
                return BNodeState.SUCCESS;
            }
            _currentWaypointIndex = 0;
        }
        var targetPosition = _waypoints[_currentWaypointIndex];
        var direction = targetPosition - _transform.position;
        if (Vector3.Distance(_transform.position, targetPosition) < 0.1f)
        {
            if (Time.time - _lastMoveTime < _waitTime)
            {
                return BNodeState.RUNNING;
            }
            _currentWaypointIndex++;
        }
        else
        {
            _spriteRenderer.flipX = direction.x > 0;
            _transform.position += direction.normalized * (_speed * Time.deltaTime);
            _lastMoveTime = Time.time;
        }
        return BNodeState.RUNNING;
    }
    public void Reset()
    {
        _currentWaypointIndex = 0;
    }
}

public class DoSomethingBehavior : IBehavior
{
    readonly Action _action;
    public DoSomethingBehavior(Action action, bool isLoop = false)
    {
        _action = action;
        IsLoop = isLoop;
    }

    public bool IsLoop { get; }

    public BNodeState Process()
    {
        _action?.Invoke();
        return BNodeState.SUCCESS;
    }

    public void Reset()
    {
    }
}

public class WaitBehavior : IBehavior
{
    private readonly float _duration;
    private float _startTime;
    public WaitBehavior(float duration, bool isLoop = false)
    {
        _duration = duration;
        IsLoop = isLoop;
    }

    public bool IsLoop { get; }

    public BNodeState Process()
    {
        if (Time.time - _startTime >= _duration)
        {
            return BNodeState.SUCCESS;
        }
        return BNodeState.RUNNING;
    }

    public void Reset()
    {
        _startTime = Time.time;
    }
}