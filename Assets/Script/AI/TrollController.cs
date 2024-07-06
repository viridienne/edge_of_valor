using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollController : EnemyController
{
    public override void Spawn(Vector3 spawnPoint, List<Vector3> patrolPoints)
    {
        base.Spawn(spawnPoint, patrolPoints);
        Patrol();
    }
}
