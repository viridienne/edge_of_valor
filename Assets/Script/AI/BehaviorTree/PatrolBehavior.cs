using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : IBehavior
{
    protected readonly List<Vector3> _waypoints;
    protected int _currentWaypointIndex;
    protected readonly Transform _transform;
    protected readonly float _speed = 1.0f;
    protected SpriteRenderer _spriteRenderer;
    public PatrolBehavior(Transform transform,SpriteRenderer spriteRenderer, List<Vector3> waypoints, float speed = 1.0f)
    {
        _transform = transform;
        _waypoints = waypoints;
        _currentWaypointIndex = 0;
        _speed = speed;
        _spriteRenderer = spriteRenderer;
    }
    public BNodeState Process()
    {
        if (_waypoints.Count == 0)
        {
            return BNodeState.FAILURE;
        }
        if(_currentWaypointIndex >= _waypoints.Count)
        {
            return BNodeState.SUCCESS;
        }
        var targetPosition = _waypoints[_currentWaypointIndex];
        var direction = targetPosition - _transform.position;
        if (Vector3.Distance(_transform.position, targetPosition) < 0.1f)
        {
            _currentWaypointIndex++;
        }
        else
        {
            _spriteRenderer.flipX = direction.x > 0;
            _transform.position += direction.normalized * (_speed * Time.deltaTime);
        }
        return BNodeState.RUNNING;
    }
    public void Reset()
    {
        _currentWaypointIndex = 0;
    }
}