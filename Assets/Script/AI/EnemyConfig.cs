using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public string ID;
    public string EnemyName;
    public float Speed;
    public float AttackRange;
    public float AttackRate;
    public float Damage;
    public float Health;
    public GameObject RootPrefab;
    public bool IsRespawnable;
    [ShowIf("IsRespawnable")]public float SpawnInterval;
    public bool CanPatrol;
}