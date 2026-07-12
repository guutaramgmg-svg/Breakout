using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{    
    // 移動アニメーション
    public EnemyData.Move EnemyMove;

    // エネミーの見た目
    public EnemyData.EnemyObj EnemyObj;
    
    // HP
    public int EnemyHp;
    
}
