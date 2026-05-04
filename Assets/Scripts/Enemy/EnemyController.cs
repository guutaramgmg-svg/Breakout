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
public class EnemyController : MonoBehaviour
{
    // このブロックの種類
    public EnemyType blockType;

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
        // 初期HPに応じた色を設定
    }


}
