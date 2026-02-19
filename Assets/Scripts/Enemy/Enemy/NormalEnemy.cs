using UnityEngine;
public class NormalEnemy : EnemyController
{
    protected override void Start()
    {
        hp = 1;
        base.Start();
    }

    protected override void UpdateColor()
    {
        sr.color = Color.paleGreen;
    }
}
