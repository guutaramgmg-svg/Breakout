using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ライフ（HP）を管理するクラス
/// ・現在のライフ数を保持
/// ・TextMeshProで数値表示
/// ・Sliderでライフゲージ表示
/// ・DOTweenでカウントアップ / ダウン演出
/// </summary>
public class LifeManager : MonoBehaviour
{
    // ===== シングルトン =====
    // 他のスクリプトから LifeManager.Instance でアクセスできる
    public static LifeManager Instance;

    // ===== ライフ数値 =====
    private int maxLife = 100;   // ライフの最大値
    private int life = 100;      // 実際のライフ値（最終的な値）
    private int preLife = 100;   // 表示用ライフ（アニメーション中に変化）
    private bool isCountUp;      // カウントアップ（アニメ中）かどうか

    // DOTweenのシーケンス（アニメ管理用）
    Sequence sequence;

    // ===== UI =====
    [SerializeField] public TextMeshProUGUI scoreText; // ライフ数値表示テキスト
    [SerializeField] Slider lifeSlider;                // ライフゲージ用スライダー

    /// <summary>
    /// オブジェクト生成時に一度だけ呼ばれる
    /// シングルトンの初期化処理
    /// </summary>
    void Awake()
    {
        // すでに Instance が存在していたら自分を削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 自分自身を Instance に登録
        Instance = this;
    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// ライフとUIの初期設定
    /// </summary>
    void Start()
    {
        // 表示用ライフを最大値で初期化
        preLife = maxLife;

        // スライダーの最大値を設定
        lifeSlider.maxValue = maxLife;

        // スライダーの現在値を設定
        lifeSlider.value = preLife;
    }

    /// <summary>
    /// ライフを増減させる処理
    /// value がマイナスならダメージ
    /// value がプラスなら回復
    /// </summary>
    public void AddScore(int value)
    {
        // 現在表示しているライフを保存
        preLife = life;

        // ライフを加算（マイナスでダメージ）
        life += value;

        // すでにアニメーション中なら強制終了
        if (isCountUp == true)
        {
            sequence.Kill(true);
        }

        // ライフ数値変更のアニメーション開始
        CountUpAnim();
    }

    /// <summary>
    /// 毎フレーム呼ばれる
    /// アニメーション中のみUIを更新する
    /// </summary>
    void Update()
    {
        if (isCountUp == true)
        {
            // ライフ数値テキスト更新
            scoreText.SetText("{0:000}", preLife);

            // スライダーの表示更新
            lifeSlider.value = preLife;
        }
    }

    /// <summary>
    /// DOTweenを使ってライフ表示を滑らかに変化させる
    /// </summary>
    void CountUpAnim()
    {
        // アニメーション中フラグをON
        isCountUp = true;

        // 表示用ライフ（preLife）を最終値（life）まで変化させる
        sequence = DOTween.Sequence()
            .Append(DOTween.To(
                () => preLife,          // 現在の値
                num => preLife = num,   // 更新処理
                life,                  // 最終的な値
                0.5f                   // アニメーション時間
            ))
            .AppendInterval(0.1f)      // 少し待つ
            .AppendCallback(() => isCountUp = false); // アニメ終了
    }
}
