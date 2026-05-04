using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BusterStatus : MonoBehaviour
{
    [SerializeField] GameObject view;
    [SerializeField] GameObject line;

    [SerializeField] List<Collider2D> EnemyList;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        ResetView();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        EnemyList.RemoveAll(item => item == null);

        if (EnemyList.Count == 0)
        {
            ResetView();
            return;  
        } 

        if (!collision.CompareTag("Enemy")) return;

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

            float busterY = transform.position.y - 5;
            float enemyY = bottomEnemy.transform.position.y;

            // ===== view（敵に合わせる）=====
            Vector3 pos = view.transform.position;
            pos.y = enemyY;
            view.transform.position = pos;

            // ===== 中間位置 =====
            float centerY = (busterY + enemyY) / 2f;

            Vector3 pos2 = line.transform.position;
            pos2.y = centerY;
            line.transform.position = pos2;

            // ===== 距離（長さ）=====
            float distance = Mathf.Abs(enemyY - busterY);

            Vector3 scale = line.transform.localScale;
            scale.x = 0.2f; // 太さ固定
            scale.y = distance;
            line.transform.localScale = scale;

            // ダメージ
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

            if (EnemyList.Count == 0)
            {
                ResetView();
            }
        }
    }

    private void ResetView()
    {        
        Vector3 viewPos = view.transform.position;
        Vector3 viewScale = view.transform.localScale;

        Vector3 linePos = line.transform.position;
        Vector3 lineScale = line.transform.localScale;

        view.transform.position = new Vector3(viewPos.x,6,viewPos.z);
        view.transform.localScale = new Vector3(viewScale.x,viewScale.y,viewScale.z);
        
        line.transform.position = new Vector3(linePos.x,1 ,linePos.z);
        line.transform.localScale = new Vector3(lineScale.x,10,lineScale.z);    

    }

}
