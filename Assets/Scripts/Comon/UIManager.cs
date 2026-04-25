using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    // ===== UI =====
    // ゲームスタート表示UI
    [SerializeField] GameObject uiStageSelect;
    // ゲームオーバー表示UI
    [SerializeField] GameObject uiGameCler;
    // ゲームクリア表示UI
    [SerializeField] GameObject uiGameOver;
    // ゲームスコア表示UI
    [SerializeField] GameObject uiScore;
    // ライプポイント表示UI
    [SerializeField] GameObject uilifePoint;

    // ステージ
    [SerializeField] GameObject uiStage;

    [SerializeField] TextMeshProUGUI uiStageText;

    // エネミーの数表示UI
    [SerializeField] GameObject uiEnemyCount;

    //エネミーの数
    [SerializeField] TextMeshProUGUI uiEnemyCountText;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    void Start()
    {
        // ステージセレクト 表示
        uiGameCler.SetActive(false);
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(true);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);
        uiEnemyCount.SetActive(false);        
    }

    /// <summary>
    /// ゲームスタート時
    /// </summary>
    public void GameStart(int stage)
    {
        // スコア、ライフポイント表示
        uiGameCler.SetActive(false);
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(true);
        uilifePoint.SetActive(true);
        uiEnemyCount.SetActive(true);     
        uiStage.SetActive(true);
        UpdateStage(stage);
    }

    /// <summary>
    /// ゲームクリア時
    /// </summary>
    public void GameClear()
    {
        // クリア表示
        uiGameCler.SetActive(true);
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);   
        uiEnemyCount.SetActive(false);        
    }

    public void GameOver()
    {
        uiGameCler.SetActive(false);
        uiGameOver.SetActive(true);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);   
        uiEnemyCount.SetActive(false);        
    }

    public void GameStop()
    {
        uiGameCler.SetActive(false);
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);   
        uiEnemyCount.SetActive(false);        
    }

    public void UpdateEnemyCountText(int enemyCount,int enemyMax)
    {
        uiEnemyCountText.text = enemyCount + "/" + enemyMax;
    }

    private void UpdateStage(int stage)
    {
        int viewStage = stage + 1;
        uiStageText.text = "Stage:" + viewStage;
    }
}
