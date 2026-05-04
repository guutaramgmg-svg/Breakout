using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BusterStatus : MonoBehaviour
{
    [SerializeField] GameObject view;
    [SerializeField] GameObject line;

    [SerializeField] List<Collider2D> EnemyList;
    //

    void OnTriggerStay2D(Collider2D collision)
    {

        if (EnemyList.Count == 0) return;

        Collider2D bottomEnemy = null;
        float lowestY = float.MaxValue;

        // 一番下の敵を探す
        foreach (var item in EnemyList)
        {
            if (item == null) continue;

            float y = item.transform.position.y;

            if (y < lowestY)
            {
                lowestY = y;
                bottomEnemy = item;
            }
        }

        // 見つかったらダメージ
        if (bottomEnemy != null)
        {
            Debug.Log("ダメージを与える");
            
            // エネミーの位置
            Vector3 pos = view.transform.position;
            pos.y = bottomEnemy.transform.position.y;
            view.transform.position = pos;

            bottomEnemy.GetComponent<EnemyStatus>().TakeDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {        
        Debug.Log("バスター");
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("バスターEnemy");
            EnemyList.Add(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyList.Remove(collision);
        }
        
    }

}
