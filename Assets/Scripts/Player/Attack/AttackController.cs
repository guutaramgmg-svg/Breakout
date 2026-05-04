using TMPro;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    PaddleController paddleController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paddleController = FindAnyObjectByType<PaddleController>();
    }

    // Update is called once per frame
    void Update()
    {
     this.transform.position = paddleController.transform.position;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("----------------");

        // ダメージボールをキャッチしたら
        if (collision.gameObject.CompareTag("Damage"))
        {
            Debug.Log("キャッチ成功");
            paddleController.isCatch = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("近距離アタック成功");
            collision.GetComponent<EnemyStatus>().TakeDamage();
        }
    }


}
