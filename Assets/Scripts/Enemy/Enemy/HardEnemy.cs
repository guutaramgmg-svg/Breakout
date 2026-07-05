using UnityEngine;

public class HardEnemy : EnemyStatus
{
    protected override void Start()
    {
        hp = 4;
        base.Start();
    }
}
