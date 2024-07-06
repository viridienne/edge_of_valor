using System.Collections.Generic;
using NodeExtension;
using UnityEngine;

public interface IEnemy
{
    public void Spawn(Vector3 spawnPoint, List<Vector3> patrolPoints);
    public void TakeDamage(float damage);
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
    [SerializeField] protected BSensor _sensor;
    
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
        BuildBehaviorTree();
    }

    public virtual void BuildBehaviorTree()
    {
        _behaviorTree = new BehaviorTree(_config.EnemyName);
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
        _behaviorTree.Process();
    }
}

