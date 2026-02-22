using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するクラス
/// ・ブロック生成
/// ・ボール生成
/// ・ゲームクリア判定
/// ・リトライ処理
/// </summary>
public class GameManager : MonoBehaviour
{
    // ===== プレハブ参照 =====
    [SerializeField] GameObject ball;          // ボールのプレハブ

    // ===== UI =====
    [SerializeField] GameObject uIStageSelect;     // ゲームスタート表示UI
    [SerializeField] GameObject uIGameOver;     // ゲームクリア表示UI
    [SerializeField] GameObject uIScore;         // ゲームスコア表示UI
    [SerializeField] GameObject uIlifePoint;     // ライプポイント表示UI

    // ===== ボール生成間隔 =====
    [SerializeField] float interval = 5f;      // 何秒ごとにボールを出すか

    // ===== ゲーム状態管理 =====
    bool isGameOver = false;  // ゲームクリア済みかどうか

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    void Start()
    {
        // ゲーム開始時はクリアUIを非表示
        uIGameOver.SetActive(false);
        uIStageSelect.SetActive(true);
        uIScore.SetActive(false);
        uIlifePoint.SetActive(false);
    }


    public void GameStart(int stage)
    {
        uIStageSelect.SetActive(false);
        uIScore.SetActive(true);
        uIlifePoint.SetActive(true);
        // 最初のボールを生成
        Instantiate(ball, new Vector2(0, -2), Quaternion.identity);
        // 一定間隔でボールを出すコルーチン開始
        StartCoroutine(ShootRoutine());

        //ステージ生成開始
        StageController.Instance.GameStart(stage);
    }

    /// <summary>
    /// 一定時間ごとにボールを発射するコルーチン
    /// </summary>
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Shoot(new Vector2(0, -2));
        }
    }

    /// <summary>
    /// 指定位置にボールを生成する
    /// </summary>
    public void Shoot(Vector2 pos)
    {
        Instantiate(ball, pos, Quaternion.identity);
    }

    void Update()
    {
        // 今回は使用していない
    }

    /// <summary>
    /// ブロックが破壊された時に呼ばれる
    /// </summary>
    public void OnBlockDestroyed()
    {
        // すでにゲームクリアしていたら何もしない
        if (isGameOver) return;

        // 残りブロック数を減らす
        ScoreManager.Instance.AddScore(33);
        // 全て壊されたらゲームクリア
        if (ScoreManager.Instance.score > 600)
        {
            Invoke("GameClear", 1f);
        }

    }

    /// <summary>
    /// ゲームクリア時の処理
    /// </summary>
    void GameClear()
    {
        // クリアUIを表示
        uIGameOver.SetActive(true);

        // クリア状態にする
        isGameOver = true;

        Debug.Log("GAME CLEAR!!");

        // ボールの定期生成を停止
        StopAllCoroutines();

        // ゲームを一時停止
        Time.timeScale = 0f;
    }

    /// <summary>
    /// リトライ（シーン再読み込み）
    /// UIボタンから呼ばれる想定
    /// </summary>
    public void Retry()
    {
        // 時間停止を解除
        Time.timeScale = 1f;

        // シーンを再読み込み
        SceneManager.LoadScene(0);
    }
}