using UnityEngine;

public class FieldController : MonoBehaviour
{
    public int Hp;
    public Animator moveAnimator;
    public Animator fieldAnimator;

    public Field fieldData;

    public FieldStatus fieldStatus;


    public void Start()
    {
        Initalize(fieldData);
    }

    public void Initalize(Field fieldData)
    {
        var field = fieldData;
        // 移動アニメーション取得
        moveAnimator = GetComponent<Animator>();
        moveAnimator.runtimeAnimatorController = FieldObjectData.Instance.GetMoveAnim(field.FieldMove);

        // フィールドアニメーション取得
        fieldAnimator = fieldStatus.GetComponent<Animator>();
        fieldAnimator.runtimeAnimatorController = FieldObjectData.Instance.GetFieldAnim(field.FieldObj);
        
        // HP取得
        Hp = field.FieldHp;

    }



}
