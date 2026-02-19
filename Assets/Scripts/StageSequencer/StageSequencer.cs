using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;



[CreateAssetMenu(menuName = "StageSequencer")]
public class StageSequencer : ScriptableObject
{
    [SerializeField] private String filename = "";
    [SerializeField] EnemyController[] enemyPrefabs = default;

    public struct StageData
    {
        public readonly float eventPos;
        public readonly float arg1, arg2;
        public readonly uint arg3;

        public StageData(float _eventpos, float _x, float _y, uint _type)
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
        var revarr = new Dictionary<string, uint>();
        for (uint i = 0; i < enemyPrefabs.Length; ++i)
        {
            revarr.Add(enemyPrefabs[i].name, i);
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

            stagecsvdata.Add(
                new StageData(
                    float.Parse(cols[0]), // シーケンス
                    float.Parse(cols[1]), // x座標
                    float.Parse(cols[2]), // y座標
                    revarr.ContainsKey(cols[3]) ? revarr[cols[3]] : 0) //一旦適当に数値　エネミープレファブのリスト番号

            );
        }
        stageDatas = stagecsvdata.OrderBy(item => item.eventPos).ToArray();

    }

    public void Reset()
    {
        stagedataidx = 0;
    }



    public void Step(float _stageProgressTime)
    {
        Debug.Log("Step");
        while (stagedataidx < stageDatas.Length &&
         stageDatas[stagedataidx].eventPos <= _stageProgressTime)
        {
            var enmtmp = Instantiate(enemyPrefabs[stageDatas[stagedataidx].arg3]);
            // エネミープールに配置
            enmtmp.transform.parent = StageController.Instance.enemyPool;
            // 配置 
            enmtmp.transform.localPosition =
            new Vector3(stageDatas[stagedataidx].arg1, stageDatas[stagedataidx].arg2,0);
            ++stagedataidx;
        }
        
    }
}
