using UnityEngine;

public class HardEnemy : EnemyController
{
    protected override void Start()
    {
        hp = 4;
        base.Start();
    }

    protected override void UpdateColor()
    {
        sr.color =
            hp == 4 ? new Color(0.5f, 0f, 1f) : // 紫
            hp == 3 ? Color.blue :
            hp == 2 ? Color.yellow :
                      Color.red;
    }
}
