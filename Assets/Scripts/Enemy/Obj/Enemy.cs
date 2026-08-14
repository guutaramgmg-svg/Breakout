using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{    
    // 移動アニメーション
    public EnemyObjectData.Move EnemyMove;

    // エネミーの見た目
    public EnemyObjectData.EnemyObj EnemyObj;
    
    // HP
    public int EnemyHp;
    
}
