using UnityEngine;
public class NormalEnemy : EnemyStatus
{
    protected override void Start()
    {
        hp = 3;
        base.Start();
    }

}
