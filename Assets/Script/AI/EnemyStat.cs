public class EnemyStat
{
    public float Speed;
    public float AttackRange;
    public float AttackRate;
    public float Damage;
    public float Health;
    public bool IsDead => Health <= 0;
    public EnemyStat(EnemyConfig config)
    {
        Init(config);
    }

    public void Init(EnemyConfig config)
    {
        Speed = config.Speed;
        AttackRange = config.AttackRange;
        AttackRate = config.AttackRate;
        Damage = config.Damage;
        Health = config.Health;
    }
}
