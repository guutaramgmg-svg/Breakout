using System.Buffers;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private StageSequencer sequencer = default;

    [SerializeField] public Transform enemyPool = default;


    float stageProgressTime = 0;

    private static StageController instance;
    public static StageController Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GameStart()
    {
        sequencer.Load();
        sequencer.Reset();
        stageProgressTime = 0;

        StartCoroutine(StageCreate());

    }
    IEnumerator StageCreate()
    {
        while (stageProgressTime < 200f)
        {
            sequencer.Step(stageProgressTime);

            stageProgressTime += Time.deltaTime;
            yield return null;
        }
    }

}
