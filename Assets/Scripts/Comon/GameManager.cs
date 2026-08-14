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

    // ===== ボール生成間隔 =====
    [SerializeField] float interval = 5f;      // 何秒ごとにボールを出すか

    // ===== ゲーム状態管理 =====
    bool isGameOver = false;  // ゲームクリア済みかどうか

    // UIクラス
    [SerializeField] UIManager uiManager;

    public static GameManager Instance;

    /// <summary>
    /// エネミー総数
    /// </summary>
    private int m_EnemyMaxCount;
    /// <summary>
    /// 撃破エネミー数
    /// </summary>
    private int m_EnemyCount;

    /// <summary>
    /// 現在のステージ
    /// </summary>
    private int m_Stage;

    private int ShootCount;

    public void EnemyCountReset()
    {
        m_EnemyCount = 0;
        m_EnemyMaxCount = 0;
        UpdateEnemyCount();        
    }

    public bool CheckEnemyCount()
    {
        if(m_EnemyCount == 0)
        {
            return false;
        }
        return true;
    }

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
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    void Start()
    {
    }

    public void GameStart(int stage)
    {
        m_Stage = stage;
        EnemyCountReset();

        uiManager.GameStart(stage);
        //ステージ生成開始
        StageController.Instance.GameStart(stage);
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

        m_EnemyCount++;
        UpdateEnemyCount();

        // 全て壊されたらゲームクリア
        if (m_EnemyCount >= m_EnemyMaxCount)
        {
            if(m_Stage == 2)
            {
                Debug.Log("クリア処理");
                Invoke("GameClear", 1f);                
                return;
            }

            // ネクストステージアニメーション
            m_Stage++;
            GameStop();
            GameStart(m_Stage);
        }
    }
    public void GameStop()
    {
        uiManager.GameStop();
        StageController.Instance.StopSageCreate();
    }



    /// <summary>
    /// ゲームクリア時の処理
    /// </summary>
    void GameClear()
    {
        uiManager.GameClear();
        // クリア状態にする
        isGameOver = true;

        Debug.Log("GAME CLEAR!!");

        // ボールの定期生成を停止
        StopAllCoroutines();

        // ゲームを一時停止
        Time.timeScale = 0f;
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void GameOver()
    {
        uiManager.GameOver();

        isGameOver = true;
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

    /// <summary>
    /// エネミー総数のUI更新
    /// </summary>
    /// <param name="enemyMaxCount"></param>
    public void UpdateEnemyMaxCount(int enemyMaxCount)
    {
        m_EnemyMaxCount = enemyMaxCount;        
        UpdateEnemyCount();
    }

    /// <summary>
    /// エネミー討伐数UI更新
    /// </summary>
    public void UpdateEnemyCount()
    {
        uiManager.UpdateEnemyCountText(m_EnemyCount,m_EnemyMaxCount);
    }

}