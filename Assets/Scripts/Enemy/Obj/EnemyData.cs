using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public Enemy[] EnemySO;

    // TODO 将来的にリサイクル
    public EnemyController enemyPrefab;

    public enum EnemyObj
    {
        Knaight,
        Ninjya,
        Touzoku,
        Maou,
    }

    public enum Move
    {
        Down,
        Left,
        Right,
        Up,

    }



    public List<RuntimeAnimatorController> enemyAnimatorList;
    public List<RuntimeAnimatorController> moveAnimatorList;

    public static EnemyData Instance;

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


    public RuntimeAnimatorController GetEnemyAnim(EnemyObj enemy)
    {
        return enemyAnimatorList[(int)enemy];
    } 
    
    public RuntimeAnimatorController GetMoveAnim(Move move)
    {
        return moveAnimatorList[(int)move];
    } 

}
