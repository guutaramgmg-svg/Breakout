using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectPoolで管理される個々のオブジェクト用クラス
/// 弾やエフェクトなど、プールから出し入れする対象に付ける
/// </summary>
public class PoolContent : MonoBehaviour
{
    // 自分を管理しているObjectPoolへ参照
    ObjectPool pool;

    // 初期化処理a
    void Start()
    {
        // 親オブジェクトにあるObjectPoolを取得
        // ObjectPoolは生成したPoolContentの親になっている
        pool = transform.parent.GetComponent<ObjectPool>();

        // 最初は非表示状態にして待機させる
        //gameObject.SetActive(false);
    }

    /// <summary>
    /// オブジェクトをステージ上に出現させる
    /// </summary>
    /// <param name="_position">出現位置</param>
    /// <param name="_angle">出現向き</param>
    public void ShowInStage(Vector3 _position, float _angle)
    {
        // 指定された位置へ移動
        transform.position = _position;

        // 指定させた角度へ回転
        transform.eulerAngles = new Vector3(0, _angle, 0);
    }

    /// <summary>
    /// オブジェクトをステージから戻す
    /// 使用終了したオブジェクトをObjectPoolへ返却する
    /// </summary>
    public void HideFromStage()
    {
        Debug.Assert(gameObject.activeInHierarchy);

        // ObjectPoolへ自分自身を返却する
        pool.Collect(this);
    }
}