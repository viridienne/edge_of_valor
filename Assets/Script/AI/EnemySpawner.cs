using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpawnPoint
{
    public Vector3 InitialPosition;
    public EnemyConfig EnemyConfig;
    public List<Vector3> PatrolPoints;
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    private readonly Dictionary<int, EnemyController> _spawnedEnemies = new();
    private void Start()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            var enemy = Instantiate(spawnPoint.EnemyConfig.RootPrefab, spawnPoint.InitialPosition, Quaternion.identity);
            var enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetConfig(spawnPoint.EnemyConfig);
            enemyController.Spawn(spawnPoint.InitialPosition, spawnPoint.PatrolPoints);
            _spawnedEnemies.Add(enemy.GetInstanceID(), enemyController);
        }
        
        InvokeRepeating(nameof(CountDown), 1, 1);
    }
    
    
    public void CountDown()
    {
        foreach (var e in _spawnedEnemies)
        {
            var enemy = e.Value;
            if (enemy.IsDead)
            {
                if (enemy.SpawnHandler.IsSpawnable())
                {
                    enemy.Respawn();
                }
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(CountDown));
    }
}
