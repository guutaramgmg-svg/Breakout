using UnityEngine;

public class HardEnemy : EnemyStatus
{
    protected override void Start()
    {
        Hp = 4;
        base.Start();
    }
}
