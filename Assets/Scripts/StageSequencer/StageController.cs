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

        StartCoroutine(StageCreate(stage));

    }
    IEnumerator StageCreate(int stage)
    {
        while (stageProgressTime < 200f)
        {
            sequencer[stage].Step(stageProgressTime);

            stageProgressTime += Time.deltaTime;
            yield return null;
        }
    }

}
