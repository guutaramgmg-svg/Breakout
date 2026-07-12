using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(menuName = "StageSequencer")]
public class StageSequencer : ScriptableObject
{
    [SerializeField] private String filename = "";
    
    EnemyController EnemyPrefab => EnemyData.Instance.enemyPrefab;
    private Enemy[] EnemyList => EnemyData.Instance.EnemySO;

    public struct StageData
    {
        public readonly float eventPos;
        public readonly float arg1, arg2;
        public readonly Enemy arg3;

        public StageData(float _eventpos, float _x, float _y, Enemy _type)
        {
            eventPos = _eventpos;
            arg1 = _x;
            arg2 = _y;
            arg3 = _type;
        }
    }

    StageData[] stageDatas;
    private int stagedataidx = 0;
    public void Load()
    {
        Debug.Log("Load");
        //名前から番号を逆引きする
        var enemyTable = new Dictionary<string, Enemy>();
        foreach (var enemy in EnemyList)
        {
            enemyTable[enemy.name] = enemy;
        }

        //CSVデータ読み込み
        var csvdata = Resources.Load<TextAsset>(filename).text;
        StringReader sr = new StringReader(csvdata);

        var stagecsvdata = new List<StageData>();

        while (sr.Peek() != -1)
        {
            var line = sr.ReadLine();
            var cols = line.Split(',');
            //4列でなければは無視　TODO後で検討
            if (cols.Length != 4) continue;

            if (!enemyTable.TryGetValue(cols[3], out Enemy enemy))
            {
                Debug.LogWarning($"Enemy '{cols[3]}' not found.");
                continue;
            }

            stagecsvdata.Add(
                new StageData(
                    float.Parse(cols[0]), // シーケンス
                    float.Parse(cols[1]), // x座標
                    float.Parse(cols[2]), // y座標
                    enemy) //一旦適当に数値　エネミープレファブのリスト番号

            );
        }
        Debug.Log("ステージステップカウント：" + stagecsvdata.Count);
        GameManager.Instance.UpdateEnemyMaxCount(stagecsvdata.Count);
        stageDatas = stagecsvdata.OrderBy(item => item.eventPos).ToArray();
    }

    public void Reset()
    {
        stagedataidx = 0;
    }



    public void Step(float _stageProgressTime)
    {
        while (stagedataidx < stageDatas.Length &&
         stageDatas[stagedataidx].eventPos <= _stageProgressTime)
        {
            var stageData = stageDatas[stagedataidx];
            var instance = Instantiate(EnemyPrefab,
            StageController.Instance.enemyPool);


            // 配置 
            instance.transform.localPosition =
            new Vector3(stageData.arg1, stageData.arg2,0);
            // エネミーの初期化
            instance.Initalize(stageData.arg3);
            stagedataidx++;
        }
        
    }
}
