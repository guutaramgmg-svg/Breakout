using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;



[CreateAssetMenu(menuName = "StageSequencer")]
public class StageSequencer : ScriptableObject
{
    [SerializeField] private String enemyname = "";
    
    [SerializeField] private String fieldname = "";

    EnemyController EnemyPrefab => EnemyObjectData.Instance.enemyPrefab;

    FieldController FieldPrefab => FieldObjectData.Instance.fieldPrefab;

    private Enemy[] EnemyList => EnemyObjectData.Instance.EnemySO;

    public struct EnemyData
    {
        public readonly float eventPos;
        public readonly float arg1, arg2;
        public readonly Enemy arg3;

        public EnemyData(float _eventpos, float _x, float _y, Enemy _type)
        {
            eventPos = _eventpos;
            arg1 = _x;
            arg2 = _y;
            arg3 = _type;
        }
    }



    EnemyData[] stageDatas;


    private int stagedataidx = 0;
    private int fielddataidx = 0;

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
        var csvdataAsset = Resources.Load<TextAsset>(enemyname);
        if (csvdataAsset == null)
        {
            Debug.Log($"CSVが見つかりません: {enemyname}");
            return;
        }

        var csvfieldAsset = Resources.Load<TextAsset>(fieldname);

        if (csvfieldAsset == null)
        {
            Debug.Log($"フィールドCSVが見つかりません: {fieldname}");
            return;
        }
        var csvdata = csvdataAsset.text;
        var csvfield = csvfieldAsset.text;

        StringReader sr = new StringReader(csvdata);
        StringReader fieldsr = new StringReader(csvfield);


        var stagecsvdata = new List<EnemyData>();
        //var fieldcsvdata = new List<FieldData>();
 
        // エネミーデータ読み込み
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
                new EnemyData(
                    float.Parse(cols[0]), // シーケンス
                    float.Parse(cols[1]), // x座標
                    float.Parse(cols[2]), // y座標
                    enemy) //一旦適当に数値　エネミープレファブのリスト番号

            );
        }

        // ステージデータ読み込み
        while(fieldsr.Peek() != -1)
        {
            var line = fieldsr.ReadLine();
            var cols = line.Split(',');

            List<int> row = new List<int>();

            foreach(string col in cols)
            {
                row.Add(int.Parse(col));
            }            
            fieldDatas.Add(row);
        }

        Debug.Log("ステージステップカウント：" + stagecsvdata.Count);
        // エネミーの数を保持
        GameManager.Instance.UpdateEnemyMaxCount(stagecsvdata.Count);
        stageDatas = stagecsvdata.OrderBy(item => item.eventPos).ToArray();
    }

    public void Reset()
    {
        stagedataidx = 0;
        fielddataidx = 0;
    }



    public void Step(float _stageProgressTime)
    {
        while (stagedataidx < stageDatas.Length &&
         stageDatas[stagedataidx].eventPos <= _stageProgressTime)
        {
            var stageData = stageDatas[stagedataidx];
            var obj = StageController.Instance.enemyPool.GetComponent<ObjectPool>().Launch(
            new Vector3(stageData.arg1, stageData.arg2, 0),
            0f);

            obj.GetComponent<EnemyController>().Initalize(stageData.arg3);
            stagedataidx++;
        }
        
    }
    List<List<int>> fieldDatas = new List<List<int>>();
    public void CreateField()
    {
        float x = -3f ,y = 5.25f;

        while(fielddataidx < fieldDatas.Count)
        {
            int colIndex = 0;
            while (colIndex < fieldDatas[fielddataidx].Count)
            {
                int id = fieldDatas[fielddataidx][colIndex];

                if (id != 0)
                {
                    var obj = StageController.Instance.fieldPool.GetComponent<ObjectPool>().Launch(
                        new Vector3(x,y,0),
                        0f);
                        if(obj == null)
                    {
                        Debug.Log("オブジェクトなし");
                    }
//                        obj.GetComponent<FieldController>().fieldData = FieldObjectData.Instance.FieldSO[id];
                        obj.GetComponent<FieldController>().Initialize(FieldObjectData.Instance.FieldSO[id]);
                        

                    // var field = Instantiate(
                    // FieldPrefab,
                    // new Vector3(x,y,0),
                    // Quaternion.identity,
                    // StageController.Instance.fieldPool);
                    // field.GetComponent<FieldController>().fieldData = FieldObjectData.Instance.FieldSO[id];
                }

                x += 0.5f;
                colIndex++;
            }
            y -= 0.5f;
            x = -3f; // 初期化
            fielddataidx++;
        }
    }
}
