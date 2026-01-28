using UnityEngine;

public class SpecialBlock : BlockController
{
    protected override void Start()
    {
        hp = 2;
        base.Start();
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
}
