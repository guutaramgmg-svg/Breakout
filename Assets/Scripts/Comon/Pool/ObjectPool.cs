using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを再利用するためのプール管理クラス
/// </summary>
public class ObjectPool : MonoBehaviour
{
    // プールに入れる元となるオブジェクト
    [SerializeField] PoolContent content = default;

    // 使用可能なオブジェクトを管理するキュー
    Queue<PoolContent> objQueue;

    // プールしておく最大オブジェクト数
    [SerializeField] int MaxObjs = 20;

    // Start is called before the first frame update
    void Start()
    {
        // 指定した数の要素を持つQueueを作成
        objQueue = new Queue<PoolContent>(MaxObjs);

        // オブジェクトの事前作成
        for (int i = 0; i < MaxObjs; ++i)
        {
            // Prefabからオブジェクト生成
            var tmpobj = Instantiate(content);

            // Hierarchy上でこのObjectPoolの子にする
            tmpobj.transform.parent = transform;

            // 初期位置を画面外へ移動
            // 使用されるまで表示されないようにする
            tmpobj.transform.localPosition = new Vector3(100, 0, 100);

            tmpobj.gameObject.SetActive(false);

            // 作成したオブジェクトを待機キューへ追加
            objQueue.Enqueue(tmpobj);
        }
    }

    /// <summary>
    /// プールからオブジェクトを取り出して使用状態にする
    /// </summary>
    /// <param name="_position">出現位置</param>
    /// <param name="_angle">出現角度</param>
    /// <returns></returns>
    public PoolContent Launch(Vector3 _position, float _angle)
    {
    Debug.Log($"Queue = {objQueue}");

    if (objQueue == null)
    {
        Debug.LogError("ObjectPoolがまだ初期化されていません");
        return null;
    }
    
    // 使用可能なオブジェクトがない場合は追加生成
    PoolContent tmpobj;
    if (objQueue.Count <= 0)
    {
        Debug.Log("プール不足のためオブジェクトを追加生成");
        tmpobj = Instantiate(content, transform);
                tmpobj.gameObject.SetActive(true);
        }
        else
        {
        // Queueから１つ取り出す
        tmpobj = objQueue.Dequeue();
            
        }

        // 使用可能なオブジェクトがない場合は何もしない
//        if (objQueue.Count <= 0) return null;

////////////TODO MaxObjsが少ない数値の場合、エラーになってしまう。原因は？

        // 非表示状態から表示状態へ変更
        tmpobj.gameObject.SetActive(true);

        // 指定位置・角度でステージに出現させる
        tmpobj.ShowInStage(_position, _angle);

        // 使用するオブジェクトを返す
        return tmpobj;
    }

    /// <summary>
    /// 使用済みオブジェクトをプールへ戻す
    /// </summary>
    /// <param name="_obj"></param>
    public void Collect(PoolContent _obj)
    {
        // オブジェクトを非表示にする
        _obj.gameObject.SetActive(false);

        // 再利用できるQueueへ戻す
        objQueue.Enqueue(_obj);
    }

    /// <summary>
    /// プール内の全オブジェクトをリセットする
    /// </summary>
    public void ResetAll()
    {
        // 子オブジェクトへHideFromStageメッセージを送信
        // 対象メソッドがない場合は無視する
        BroadcastMessage("HideFromStage", SendMessageOptions.DontRequireReceiver);
    }
}
