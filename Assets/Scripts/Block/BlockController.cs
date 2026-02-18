using UnityEngine;

/// <summary>
/// ブロックの種類を表す列挙型
/// </summary>
public enum BlockType
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
public class BlockController : MonoBehaviour
{
    // このブロックの種類
    public BlockType blockType;

    // ブロックの耐久値（ヒットポイント）
    [SerializeField] protected int hp = 1;

    // 見た目変更用の SpriteRenderer
    protected SpriteRenderer sr;

    /// <summary>
    /// オブジェクト生成時に最初に呼ばれる
    /// 参照の取得は必ずここで行う
    /// </summary>
    protected virtual void Awake()
    {
        // SpriteRenderer を取得
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
        // 初期HPに応じた色を設定
        UpdateColor();
    }

    /// <summary>
    /// 他のオブジェクトと衝突した時に呼ばれる
    /// </summary>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // ボール以外との衝突は無視
        if (!collision.gameObject.CompareTag("Ball")) return;

        // ダメージ処理
        TakeDamage();
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    protected virtual void TakeDamage()
    {
        // HPを減らす
        hp--;

        // HPに応じて色を更新
        UpdateColor();

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
        FindFirstObjectByType<GameManager>()?.OnBlockDestroyed();

    }

    /// <summary>
    /// HPや種類に応じてブロックの色を変更する
    /// 派生クラスでオーバーライドする前提
    /// </summary>
    protected virtual void UpdateColor()
    {
        // デフォルト色（通常ブロック用）
        sr.color = Color.white;
    }
}
