using UnityEngine;
public class NormalEnemy : EnemyStatus
{
    protected override void Start()
    {
        hp = 1;
        base.Start();
    }

}
