using UnityEngine;
using System.Collections.Generic;

public class FieldObjectData : MonoBehaviour
{
    public Field[] FieldSO;

    public FieldController fieldPrefab;

    public enum FieldObj
    {
        Leaf,
        Stone,
    }

    public enum Move
    {
        Show,
        Down,
        Left,
        Right,
        Up,

    }
    public List<RuntimeAnimatorController> fieldAnimatorList;

    public List<RuntimeAnimatorController> moveAnimatorList;

    public static FieldObjectData Instance;

    void Awake()
    {
        // すでに Instance が存在していたら自分を破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 自分自身を Instance に登録
        Instance = this;
    }

    public RuntimeAnimatorController GetFieldAnim(FieldObj field)
    {
        return fieldAnimatorList[(int)field];
    } 

    public RuntimeAnimatorController GetMoveAnim(Move move)
    {
        return moveAnimatorList[(int)move];
    } 
}
