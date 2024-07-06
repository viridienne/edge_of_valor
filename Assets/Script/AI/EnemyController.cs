using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Spawn(Vector3 spawnPoint, List<Vector3> patrolPoints);
    public void Idle();
    public void Patrol();
    public void Chase();
    public void Attack();
    public void TakeDamage(float damage);
    public void Die();
    public void SetTarget(Transform target);
    public void SetDestination(Vector3 destination);
   public void SetConfig(EnemyConfig config);
   public void Respawn();
}

public enum EnemyState
{
    Idle,  
    Patrol,
    Chase,
    Attack,
    TakeDamage,
    Die
}

public class HandleEnemySpawn
{
    public bool IsRespawnable;
    public float SpawnInterval;
    public float LastSpawnTime;
    public float LastDeathTime;
    
    public void Init(EnemyConfig config)
    {
        IsRespawnable = config.IsRespawnable;
        SpawnInterval = config.SpawnInterval;
    }
    
    public void Set(float lastSpawnTime, float lastDeathTime)
    {
        LastSpawnTime = lastSpawnTime;
        LastDeathTime = lastDeathTime;
    }
    public void SetLastSpawnTime(float lastSpawnTime)
    {
        LastSpawnTime = lastSpawnTime;
    }
    public void SetLastDeathTime(float lastDeathTime)
    {
        LastDeathTime = lastDeathTime;
    }
    public void Reset()
    {
        LastSpawnTime = 0;
        LastDeathTime = 0;
    }
    public bool IsSpawnable()
    {
        if (!IsRespawnable) return false;
        return Time.realtimeSinceStartup - LastDeathTime >= SpawnInterval;
    }
}
public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    
    protected Vector3 _spawnPoint;
    protected List<Vector3> _patrolPoints;
    protected Transform _transform;
    protected EnemyState _state;
    protected BehaviorTree _behaviorTree;   
    protected EnemyConfig _config;
    protected EnemyStat _stat;
    protected HandleEnemySpawn _handleEnemySpawn;
    public HandleEnemySpawn SpawnHandler => _handleEnemySpawn;
    public bool IsDead => _stat.IsDead;
    public virtual void Spawn(Vector3 spawnPoint, List<Vector3> patrolPoints)
    {
        _spawnPoint = spawnPoint;
        _patrolPoints = patrolPoints;
        _transform = transform;
        _handleEnemySpawn.SetLastSpawnTime(Time.realtimeSinceStartup);
    }

    public void Idle()
    {
    }

    public void Patrol()
    {
        _behaviorTree.AddChild(new RepeatedNode(new PatrolBehavior(_transform, _spriteRenderer, _patrolPoints, _stat.Speed),
            "Patrol"));
        _state = EnemyState.Patrol;
    }

    public void Chase()
    {
        throw new System.NotImplementedException();
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
       gameObject.SetActive(false);
       _state = EnemyState.Die;
    }

    public void SetTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void SetDestination(Vector3 destination)
    {
        throw new System.NotImplementedException();
    }

    public void SetConfig(EnemyConfig config)
    {
        _config = config;
        _stat = new EnemyStat(config);
        _handleEnemySpawn = new HandleEnemySpawn();
        _behaviorTree = new BehaviorTree(_config.EnemyName);
    }

    public void Respawn()
    {
        _stat.Init(_config);
        _transform.position = _spawnPoint;
        _handleEnemySpawn.Reset();
        gameObject.SetActive(true);
        _handleEnemySpawn.SetLastSpawnTime(Time.realtimeSinceStartup);
    }

    private void Update()
    {
        if (_state == EnemyState.Patrol)
        {
            _behaviorTree.Process();
        }
    }
}
