using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private List<StageSequencer> sequencer = default;

    [SerializeField] public Transform enemyPool = default;


    float stageProgressTime = 0;

    public int stageSelect = 0;

    private Coroutine m_StageCreateCoroutine;

    private static StageController instance;
    public static StageController Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GameStart(int stage)
    {
        
        sequencer[stage].Load();
        sequencer[stage].Reset();
        stageProgressTime = 0;

        m_StageCreateCoroutine = StartCoroutine(StageCreate(stage));

    }
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

    public void StopSageCreate()
    {
        if(m_StageCreateCoroutine != null)
        {
            StopCoroutine(m_StageCreateCoroutine);
            m_StageCreateCoroutine = null;
        }
    }
}
