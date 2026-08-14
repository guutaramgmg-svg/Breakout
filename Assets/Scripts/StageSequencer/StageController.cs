using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private List<StageSequencer> sequencer = default;

    [SerializeField] public Transform enemyPool = default;

    [SerializeField] public Transform fieldPool = default;


    float stageProgressTime = 0;

    public int stageSelect = 0;

    private Coroutine m_StageCreateCoroutine;

    #region インスタンス化処理

    private static StageController instance;
    public static StageController Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    /// <param name="stage"></param>
    public void GameStart(int stage)
    {
        //FieldReset();
        
        sequencer[stage].Load();
        sequencer[stage].Reset();
        stageProgressTime = 0;
        // フィールドの生成        
        sequencer[stage].CreateField();
        m_StageCreateCoroutine = StartCoroutine(StageCreate(stage));

    }

    public void FieldReset()
    {
        // TODO 一旦削除するが　使い回す処理に変更する
        foreach(Transform child in fieldPool.transform)
        {
            if(child.GetComponent<FieldController>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// ステージを生成する
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    IEnumerator StageCreate(int stage)
    {           
        //スタート少し遅らせる
        yield return new WaitForSeconds(1f);
        while (GameManager.Instance.CheckEnemyCount())
        {
            yield return new WaitForSeconds(1f);
        }

        while (stageProgressTime < 200f)
        {
            sequencer[stage].Step(stageProgressTime);

            stageProgressTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// ステージ生成を途中で止める
    /// </summary>
    public void StopSageCreate()
    {
        if(m_StageCreateCoroutine != null)
        {
            StopCoroutine(m_StageCreateCoroutine);
            m_StageCreateCoroutine = null;
        }
    }
}
