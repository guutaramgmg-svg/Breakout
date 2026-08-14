using UnityEngine;

public class FieldController : MonoBehaviour
{
    public int Hp;
    public Animator moveAnimator;
    public Animator fieldAnimator;

    public Field fieldData;

    public FieldStatus fieldStatus;
    PoolContent poolcontent;

    public void Initialize(Field fieldData)
    {
        poolcontent = GetComponent<PoolContent>();
        var field = fieldData;
        if (field == null)
        {
            Debug.LogError("fieldDataがありません。");
            return ;
        }



        // 移動アニメーション取得
        moveAnimator = GetComponent<Animator>();
        if(moveAnimator == null)
        {
            Debug.LogError("移動用のアニメーションがありません。");
            return ;
        }
        moveAnimator.runtimeAnimatorController = FieldObjectData.Instance.GetMoveAnim(field.FieldMove);

        // フィールドアニメーション取得
        fieldAnimator = fieldStatus.GetComponent<Animator>();
        if(fieldAnimator == null)
        {
            Debug.LogError("ステータス側のアニメーションがありません。");
            return ;
        }

        fieldAnimator.runtimeAnimatorController = FieldObjectData.Instance.GetFieldAnim(field.FieldObj);
        
        // HP取得
        Hp = field.FieldHp;

        // FieldStatusを初期状態に戻す
        fieldStatus.ResetStatus();

    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
    }

    public virtual void Death()
    {
        poolcontent.HideFromStage();
    }
}
