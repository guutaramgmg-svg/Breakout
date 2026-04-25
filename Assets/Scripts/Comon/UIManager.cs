using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{

    // ===== UI =====
    [SerializeField] GameObject uiStageSelect;     // ゲームスタート表示UI
    [SerializeField] GameObject uiGameOver;     // ゲームクリア表示UI
    [SerializeField] GameObject uiScore;         // ゲームスコア表示UI
    [SerializeField] GameObject uilifePoint;     // ライプポイント表示UI

    // ステージ
    [SerializeField] GameObject uiStage;         // ステージ

    [SerializeField] GameObject uiEnemyCount;    // エネミーの数表示UI

    [SerializeField] TextMeshProUGUI uiEnemyCountText;    //エネミーの数

    /// <summary>
    /// コンストラクタ
    /// </summary>
    void Start()
    {
        // ステージセレクト 表示
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(true);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);
        uiEnemyCount.SetActive(false);        
    }

    /// <summary>
    /// ゲームスタート時
    /// </summary>
    public void GameStart()
    {
        // スコア、ライフポイント表示
        uiGameOver.SetActive(false);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(true);
        uilifePoint.SetActive(true);
        uiEnemyCount.SetActive(true);     
        uiStage.SetActive(true);
    }

    /// <summary>
    /// ゲームクリア時
    /// </summary>
    public void GameClear()
    {
        // クリア表示
        uiGameOver.SetActive(true);
        uiStageSelect.SetActive(false);
        uiScore.SetActive(false);
        uilifePoint.SetActive(false);   
        uiEnemyCount.SetActive(false);        
    }

    public void GameStop()
    {
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
}
