using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialEnemy : EnemyController
{
    float interval = 3f;
    public GameObject damageBall;

    protected override void Start()
    {
        hp = 2;
        base.Start();
        StartCoroutine("ShootRoutine");

    }

    protected override void OnBreak()
    {
        ActivateEffect();
        base.OnBreak();
    }

    void ActivateEffect()
    {
        // 例：パドルをスローにする
        // Object.FindAnyObjectByType<PaddleController>()
        //     ?.ApplySlow(0.5f, 3f);
    }


    protected override void UpdateColor()
    {
        sr.color =
            hp == 4 ? new Color(0.5f, 0f, 1f) : // 紫
            hp == 3 ? Color.blue :
            hp == 2 ? Color.yellow :
                      Color.red;
    }

    /// <summary>
    /// 一定時間ごとにボールを発射するコルーチン
    /// </summary>
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f,interval));
            Shoot(transform.position);
        }
    }

    /// <summary>
    /// 指定位置にボールを生成する
    /// </summary>
    public void Shoot(Vector2 pos)
    {
        Instantiate(damageBall, pos, Quaternion.identity);
    }

}
