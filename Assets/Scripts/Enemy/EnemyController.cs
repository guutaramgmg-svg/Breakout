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

    // ブロックの耐久値（ヒットポイント）
    [SerializeField] protected int hp = 1;



    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
        // 初期HPに応じた色を設定
    }

    /// <summary>
    /// 他のオブジェクトと衝突した時に呼ばれる
    /// </summary>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("衝突した: " + collision.gameObject.name);
        // ボール以外との衝突は無視
        if (collision.gameObject.CompareTag("Ball"))
        {
        // ダメージ処理
        TakeDamage();            
        }
        if (collision.gameObject.CompareTag("Buster"))
        {
            Debug.Log("バスター当たった");
        OnBreak();            
        }

    }


    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Buster"))
    {
        Debug.Log("バスター当たった");
        OnBreak();
    }
}


    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    protected virtual void TakeDamage()
    {
        // HPを減らす
        hp--;

        // HPに応じて色を更新

        // HPが0以下なら破壊
        if (hp <= 0)
        {
            OnBreak();
        }
    }

    /// <summary>
    /// ブロックが壊れた時の処理
    /// （派生クラスで拡張可能）
    /// </summary>
    protected virtual void OnBreak()
    {

        // 自分自身を削除
        Destroy(gameObject);
        // GameManager にブロック破壊を通知
        // FindFirstObjectByType<GameManager>()?.OnBlockDestroyed();
        GameManager.Instance.OnBlockDestroyed();

    }

}
