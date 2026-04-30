using UnityEngine;

public class BusterStatus : MonoBehaviour
{
float damageInterval = 0.2f;
float timer = 0f;

// void OnTriggerStay2D(Collider2D collision)
// {
//     Debug.Log("バスター");
//     if (collision.CompareTag("Enemy"))
//     {
//     Debug.Log("バスターEnemy");
//         timer += Time.deltaTime;

//         if (timer >= damageInterval)
//         {
//             timer = 0f;

//             collision.GetComponent<EnemyController>()?.TakeDamage();
//             Debug.Log("ダメージ！");
//         }
//     }
// }

}
