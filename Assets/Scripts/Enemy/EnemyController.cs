using System.Data.Common;
using UnityEngine;

/// <summary>
/// ブロックの種類を表す列挙型
/// </summary>
public enum EnemyType
{
    Normal,   // 通常ブロック（1回で壊れる）
    Hard,     // 硬いブロック（複数回ヒットが必要）
    Special   // 特殊ブロック（効果付き）
}

/// <summary>
/// すべてのブロックの基底クラス
/// ・HP管理
/// ・色更新
/// ・破壊処理
/// </summary>
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
//    public Enemy enemy;

    // このブロックの種類
    public EnemyType blockType;

    public int Hp;

    public Animator moveAnimator;

    public Animator enemyAnimator;

    public EnemyStatus enemyStatus;

   public void Initalize(Enemy enemyData)
    {
        var enemy = enemyData;
        // 移動アニメーション取得
        moveAnimator = GetComponent<Animator>();
        moveAnimator.runtimeAnimatorController = EnemyData.Instance.GetMoveAnim(enemy.EnemyMove);
        // エネミーアニメーション取得
        enemyAnimator = enemyStatus.GetComponent<Animator>();
        enemyAnimator.runtimeAnimatorController = EnemyData.Instance.GetEnemyAnim(enemy.EnemyObj);
        // HP取得
        Hp = enemy.EnemyHp;

    }


    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
    }


}
