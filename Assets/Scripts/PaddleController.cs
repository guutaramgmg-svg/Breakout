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
    // パドルの基本移動速度
    public float speed = 10f;

    // Rigidbody2D（物理移動用）
    Rigidbody2D rb;

    // パドルが動ける左右の限界座標
    float minX = -2.2f;
    float maxX = 2.2f;

    // 状態異常用の速度倍率
    // 1.0 = 通常速度 / 0.5 = 半分の速さ
    float speedMultiplier = 1f;

    private int hp = 3;

    void Start()
    {
        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();
        //ApplySlow(0.4f,3f);
    }
    void FixedUpdate()
    {
        // マウス・タッチが使えない場合は処理しない
        if (Pointer.current == null) return;

        // 画面を押している間だけ追従
        if (Pointer.current.press.isPressed)
        {
            // 画面座標 → ワールド座標に変換
            Vector3 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            // 指（マウス）のX座標を移動範囲内に制限
            float targetX = Mathf.Clamp(worldPos.x, minX, maxX);

            // 現在位置から目標位置へなめらかに移動
            float newX = Mathf.MoveTowards(
                rb.position.x,
                targetX,
                speed * speedMultiplier * Time.fixedDeltaTime
            );

            // Rigidbody2D を使って移動
            rb.MovePosition(new Vector2(newX, rb.position.y));
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
}
