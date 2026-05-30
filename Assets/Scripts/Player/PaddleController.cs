using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ブロック崩し用パドル制御クラス
/// ・マウス／タッチ操作で左右移動
/// ・状態異常（スロー）に対応
/// </summary>
public class PaddleController : MonoBehaviour
{
    #region プロパティ
    #region プレハブ
    [Tooltip("ボールのプレハブ")]
    [SerializeField] GameObject ball;

    [Tooltip("キャッチアクション")]
    [SerializeField] GameObject attack;

    [Tooltip("特殊攻撃バスター")]
    [SerializeField] GameObject buster;
    #endregion

    #region 移動関連
    // パドルの基本移動速度（未使用：旧ロジック用）
    public float speed = 10f;
    // Rigidbody2D（物理移動に使用）
    Rigidbody2D rb;
    // パドルが動ける左右の限界座標
    float minX = -2.2f;
    float maxX = 2.2f;
    // 状態異常による速度倍率
    // 1.0 = 通常 / 0.5 = 半減
    float speedMultiplier = 1f;
    #endregion

    #region ステータス

    // プレイヤーのHP
    private int hp = 3;

    [Tooltip("キャッチ状況(trueの時下フリックで強化攻撃)")]
    public bool isCatch;

    #endregion

    #region タッチ制御

    // タッチ開始位置（スワイプ計算用）
    Vector2 touchStartPos;

    // スワイプ中かどうか
    bool isSwiping = false;

    // フリックとして判定する最小移動距離
    float swipeThreshold = 50f;

    // 押し始めた時間（タップ判定用）
    float pressStartTime;

    [Tooltip("タップとして認識する最大時間")]
    [SerializeField] float tapThreshold = 0.15f;

    #endregion

    #region パドル位置制御
    // ドラッグ開始時のパドルのX座標
    float startPaddleX;

    #endregion

    #region 参照
    // メインカメラ
    Camera cam;
    #endregion

    #endregion

    #region ライフサイクル
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        //ApplySlow(0.4f,3f);
    }

    /// <summary>
    /// 物理
    /// </summary>
    void FixedUpdate()
    {
        // // マウス・タッチが使えない場合は処理しない
        // if (Pointer.current == null) return;

        // // 画面を押している間だけ追従
        // if (Pointer.current.press.isPressed)
        // {
        //     // 画面座標 → ワールド座標に変換
        //     Vector3 screenPos = Pointer.current.position.ReadValue();
        //     Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        //     // 指（マウス）のX座標を移動範囲内に制限
        //     float targetX = Mathf.Clamp(worldPos.x, minX, maxX);

        //     // 現在位置から目標位置へなめらかに移動
        //     float newX = Mathf.MoveTowards(
        //         rb.position.x,
        //         targetX,
        //         speed * speedMultiplier * Time.fixedDeltaTime
        //     );

        //     // Rigidbody2D を使って移動
        //     rb.MovePosition(new Vector2(newX, rb.position.y));
        // }
    }
    
    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        if (Pointer.current == null) return;

        // 押した瞬間
        if (Pointer.current.press.wasPressedThisFrame)
        {
            pressStartTime = Time.time;

            touchStartPos = Pointer.current.position.ReadValue();
            startPaddleX = rb.position.x; // ★これ追加

            isSwiping = true;
        }

        // 押してる間
        if (Pointer.current.press.isPressed)
        {
            HandleDragMove();
        }

        // 離した瞬間
        if (Pointer.current.press.wasReleasedThisFrame && isSwiping)
        {
            float pressTime = Time.time - pressStartTime;
            // 短いタップだけ
            if (pressTime < tapThreshold)
            {
                OnTap();
            }
            Vector2 endPos = Pointer.current.position.ReadValue();
            Vector2 swipe = endPos - touchStartPos;

            DetectSwipe(swipe);

            isSwiping = false;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ダメージボール受けたら
        if (collision.gameObject.CompareTag("Damage"))
        {
            hp--;
            LifeManager.Instance.AddScore(-15);

            Debug.Log("HP:" + hp);
        }
    }

    #endregion

    #region メソッド
    /// <summary>
    /// タップ長押しに合わせてパドルを動かす機能
    /// </summary>
    void HandleDragMove()
    {
        // 現在のタッチ座標を取得
        Vector2 currentPos = Pointer.current.position.ReadValue();

        // 指の移動量（差分）
        float deltaX = currentPos.x - touchStartPos.x;

        // スクリーン → ワールド変換
        float worldDelta = deltaX * 0.01f;
        float targetX = startPaddleX + worldDelta;

        // 範囲制限
        targetX = Mathf.Clamp(targetX, minX, maxX);
        rb.MovePosition(new Vector2(targetX, rb.position.y));
    }

    /// <summary>
    /// ボールの発射
    /// </summary>
    public void BoolShot()
    {
        Instantiate(ball, this.transform.position, Quaternion.identity);
    }
    
    // タップ時
    void OnTap()
    {
    }

    /// <summary>
    /// スワイプ方向を判定して、対応する処理を実行する
    /// </summary>
    /// <param name="swipe">タッチ開始位置から終了位置までの差分ベクトル</param>
    void DetectSwipe(Vector2 swipe)
    {
        // 短すぎる動きは無視
        if (swipe.magnitude < swipeThreshold) return;

        // 上フリック
        if (swipe.y > Mathf.Abs(swipe.x))
        {
            OnSwipeUp();
        }
        // 下フリック
        else if (-swipe.y > Mathf.Abs(swipe.x))
        {
            OnSwipeDown();
        }
    }

    /// <summary>
    /// 上フリック
    /// </summary>
    void OnSwipeUp()
    {
        //Debug.Log("上フリック！");
             //GameObject obj = Instantiate(buster, this.transform.position, Quaternion.identity);
             //Destroy(obj,2f); //2秒後に消す                         
    }

    /// <summary>
    /// 下フリック
    /// </summary>
    void OnSwipeDown()
    {
        //Debug.Log("下フリック！");

        if (isCatch)
        {
            //バスター
            GameObject obj = Instantiate(buster, this.transform.position, Quaternion.identity);
            Destroy(obj,2f); //2秒後に消す                         
            isCatch = false;
        }
        else
        {
            // アタック
            GameObject obj = Instantiate(attack, this.transform.position, Quaternion.identity);
            Destroy(obj,0.5f); //0.5秒後に消す             
        }
    }

    /// <summary>
    /// スロー状態を付与する
    /// </summary>
    /// <param name="rate">速度倍率（例: 0.5f）</param>
    /// <param name="duration">持続時間（秒）</param>
    public void ApplySlow(float rate, float duration)
    {
        // 既存の状態異常を解除してから開始
        StopAllCoroutines();
        StartCoroutine(SlowCoroutine(rate, duration));
    }

    /// <summary>
    /// 一定時間スピードを遅くするコルーチン
    /// </summary>
    IEnumerator SlowCoroutine(float rate, float duration)
    {
        // 移動速度を下げる
        speedMultiplier = rate;

        // 指定時間待つ
        yield return new WaitForSeconds(duration);

        // 速度を元に戻す
        speedMultiplier = 1f;
    }

    #endregion 
}
