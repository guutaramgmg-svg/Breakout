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
    [SerializeField] GameObject normalBlock;   // 通常ブロック
    [SerializeField] GameObject hardBlock;     // 硬いブロック
    [SerializeField] GameObject specialBlock;  // 特殊ブロック

    // ===== UI =====
    [SerializeField] GameObject gameStart;     // ゲームスタート表示UI
    [SerializeField] GameObject gameClear;     // ゲームクリア表示UI
    [SerializeField] GameObject score;         // ゲームスコア表示UI
    [SerializeField] GameObject lifePoint;     // ライプポイント表示UI

    // ===== ボール生成間隔 =====
    [SerializeField] float interval = 5f;      // 何秒ごとにボールを出すか

    // ===== ゲーム状態管理 =====
    int blockCount = 0;        // 現在残っているブロック数
    bool isGameClear = false;  // ゲームクリア済みかどうか

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    void Start()
    {
        // ゲーム開始時はクリアUIを非表示
        gameClear.SetActive(false);
        gameStart.SetActive(true);
        score.SetActive(false);
        lifePoint.SetActive(false);
    }


    public void GameStart()
    {
        gameStart.SetActive(false);
        score.SetActive(true);
        lifePoint.SetActive(true);
        // 最初のボールを生成
        Instantiate(ball, new Vector2(0, -2), Quaternion.identity);
        // 一定間隔でボールを出すコルーチン開始
        StartCoroutine(ShootRoutine());

        //ステージ生成開始
        StageController.Instance.GameStart();
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
    /// 指定位置にランダムなブロックを生成する
    /// </summary>
    void SpawnBlock(Vector2 pos)
    {
        // 0～9 の乱数を取得
        int rand = Random.Range(0, 10);

        // 確率でブロックを切り替える
        // 0～5 : 通常
        // 6～8 : 硬い
        // 9    : 特殊
        GameObject prefab =
            rand < 6 ? normalBlock :
            rand < 9 ? hardBlock :
                       specialBlock;

        // ブロック生成
        Instantiate(prefab, pos, Quaternion.identity);

        // 残りブロック数を増やす
        blockCount++;
    }

    /// <summary>
    /// ブロックが破壊された時に呼ばれる
    /// </summary>
    public void OnBlockDestroyed()
    {
        // すでにゲームクリアしていたら何もしない
        if (isGameClear) return;

        // 残りブロック数を減らす
        ScoreManager.Instance.AddScore(33);
        blockCount--;
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
        gameClear.SetActive(true);

        // クリア状態にする
        isGameClear = true;

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
