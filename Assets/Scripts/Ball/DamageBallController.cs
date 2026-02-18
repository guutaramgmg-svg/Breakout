using UnityEngine;

public class DamageBallController : MonoBehaviour
{
    // ボールの移動速度
    public float speed = 5f;

    // 最大バウンド回数（将来の制御用）
    public int maxBounceCount = 10;

    // 拡散時に生成するボールの数
    public int spreadCount = 2;

    // 物理演算用
    private Rigidbody2D rb;

    // 色変更用
    private SpriteRenderer sr;

    /// <summary>
    /// オブジェクト生成時に最初に呼ばれる
    /// 必須コンポーネントの取得
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// 初期方向へボールを発射
    /// </summary>
    void Start()
    {
        // 上方向を基本に、左右ランダムな角度をつける
        Vector2 initialDirection =
            new Vector2(Random.Range(-0.2f,0.2f), 1f).normalized;

        // 初速を設定
        rb.linearVelocity = initialDirection * speed;

    }

    /// <summary>
    /// 毎フレーム呼ばれる
    /// </summary>
    void Update()
    {
        // 画面下に落ちたら削除
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

}
