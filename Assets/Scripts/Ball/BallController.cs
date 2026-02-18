using UnityEngine;

/// <summary>
/// ボールの挙動を管理するクラス
/// ・初期発射
/// ・バウンド回数管理
/// ・拡散処理
/// ・色変更
/// </summary>
public class BallController : MonoBehaviour
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

    // 現在のバウンド回数
    private int bounceCount = 0;

    // すでに拡散済みかどうか
    private bool hasSpread = false;

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
            new Vector2(Random.Range(-1f, 1f), 1f).normalized;

        // 初速を設定
        rb.linearVelocity = initialDirection * speed;

        // 初期色を反映
        UpdateColor();
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

    /// <summary>
    /// 何かに衝突した時に呼ばれる
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        // バウンド回数をカウント
        bounceCount++;

        // ボール同士が衝突したら拡散（未拡散の場合のみ）
        if (collision.gameObject.CompareTag("Ball") && !hasSpread)
        {
            Spread();
            hasSpread = true;

            // 元のボールは消す
            Destroy(gameObject);
            return;
        }

        // パドルに当たった時の反射制御
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // 衝突位置の差分
            float hitPos =
                transform.position.x - collision.transform.position.x;

            // パドルの半分の幅
            float paddleHalfWidth =
                collision.collider.bounds.size.x / 2f;

            // -1 ～ 1 に正規化
            float normalized = hitPos / paddleHalfWidth;
            normalized = Mathf.Clamp(normalized, -0.9f, 0.9f);

            // 反射方向を計算
            Vector2 dir = new Vector2(normalized, 1f).normalized;
            rb.linearVelocity = dir * speed;
        }
    }

    /// <summary>
    /// ボールを複数方向に拡散させる
    /// </summary>
    void Spread()
    {
        // 360度を均等に分割
        float angleStep = 360f / spreadCount;

        for (int i = 0; i < spreadCount; i++)
        {
            // 角度をラジアンに変換
            float rad = angleStep * i * Mathf.Deg2Rad;

            // 発射方向を計算
            Vector2 dir = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ).normalized;

            // 新しいボールを生成
            GameObject newBall = Instantiate(
                gameObject,
                transform.position,
                Quaternion.identity
            );

            BallController bc = newBall.GetComponent<BallController>();

            // 拡散済みに設定（無限拡散防止）
            bc.hasSpread = true;
            bc.bounceCount = 0;

            // 色を変更
            bc.UpdateColor();

            // 速度を設定
            Rigidbody2D newRb = newBall.GetComponent<Rigidbody2D>();
            newRb.linearVelocity = dir * speed;
        }
    }

    /// <summary>
    /// ボールの状態に応じて色を変更する
    /// </summary>
    void UpdateColor()
    {
        if (sr == null) return;

        // 拡散済みは色を変える
        sr.color = hasSpread ? Color.magenta : Color.white;
    }
}
