using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
/// スコアを管理するクラス
/// ・現在のスコアを保持
/// ・TextMeshProでスコア表示
/// ・DOTweenでカウントアップ演出
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // ===== シングルトン =====
    // どこからでも ScoreManager.Instance でアクセスできる
    public static ScoreManager Instance;

    // ===== スコア数値 =====
    private int score;        // 実際のスコア（最終値）
    private int preScore;     // 表示用スコア（アニメーション中に変化）
    private bool isCountUp;   // カウントアップ演出中かどうか

    // DOTweenのシーケンス（アニメ管理用）
    Sequence sequence;

    // ===== UI =====
    [SerializeField] public TextMeshProUGUI scoreText; // スコア表示用テキスト

    /// <summary>
    /// オブジェクト生成時に一度だけ呼ばれる
    /// シングルトンの初期化処理
    /// </summary>
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

    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="value">加算するスコア量</param>
    public void AddScore(int value)
    {
        // 現在表示しているスコアを保存
        preScore = score;

        // スコアを加算
        score += value;

        // すでにカウントアップ中ならアニメを強制終了
        if (isCountUp == true)
        {
            sequence.Kill(true);
        }

        // スコアのカウントアップ演出開始
        CountUpAnim();
    }

    /// <summary>
    /// 毎フレーム呼ばれる
    /// カウントアップ中のみUIを更新する
    /// </summary>
    void Update()
    {
        if (isCountUp == true)
        {
            // 表示用スコアをTextに反映
            scoreText.SetText("{0:0000}", preScore);
        }
    }

    /// <summary>
    /// DOTweenを使ってスコアを滑らかに増加させる演出
    /// </summary>
    void CountUpAnim()
    {
        // カウントアップ演出中フラグをON
        isCountUp = true;

        // 表示用スコア（preScore）を最終スコア（score）まで変化させる
        sequence = DOTween.Sequence()
            .Append(DOTween.To(
                () => preScore,          // 現在の表示スコア
                num => preScore = num,   // スコア更新処理
                score,                  // 最終的なスコア
                0.5f                     // アニメーション時間
            ))
            .AppendInterval(0.1f)        // 少し待機
            .AppendCallback(() => isCountUp = false); // 演出終了
    }
}
