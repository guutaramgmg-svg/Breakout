using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyStatus : MonoBehaviour
{
    // このブロックの種類
    public EnemyType blockType;

    // エネミー耐久値（ヒットポイント）
    public int Hp
    {
        get => enemyController.Hp;
        set => enemyController.Hp = value;
    }

    // 死亡時最終サイズ
    private float deathEndScale = 1.5f;
    public EnemyController enemyController;


    #region アニメーション
    private Animator animator;    
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    #endregion

    // 見た目変更用の SpriteRenderer
    protected SpriteRenderer sr;

    // 点滅処理
    private bool isBlinking = false;

    // 死亡フラグ
    private bool isDead = false;

    /// <summary>
    /// オブジェクト生成時に最初に呼ばれる
    /// 参照の取得は必ずここで行う
    /// </summary>
    protected virtual void Awake()
    {
        // SpriteRendererを取得
        sr = GetComponent<SpriteRenderer>();
 
        // Animatorを取得
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
        // 初期HPに応じた色を設定
        UpdateColor();
    }


    float invincibleTime = 0.5f;
    float timer = 0f;
    bool isInvincible = false;

    void Update()
    {
        // 一定時間無敵
        if (isInvincible)
        {
            timer += Time.deltaTime;
            if (timer >= invincibleTime)
            {
                isInvincible = false;
                timer = 0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Buster"))
        {
//            TakeDamage();
        }
    }

    /// <summary>
    /// 他のオブジェクトと衝突した時に呼ばれる
    /// </summary>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("衝突した: " + collision.gameObject.name);
        // ボール以外との衝突は無視
        if (collision.gameObject.CompareTag("Ball"))
        {
            // ダメージ処理
            TakeDamageBall();            
        }
    }


    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    public virtual void TakeDamageBall()
    {
        if (isDead) return;

        // HPを減らす
        TakeDamage(1);

        // HPが0以下なら破壊
        if (Hp <= 0)
        {
            OnBreak();
        }
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    public virtual void TakeDamageBuster()
    {
        if (isDead) return;

        if (isInvincible) return;
        isInvincible = true;
        // HPを減らす
        TakeDamage(1);

        // HPが0以下なら破壊
        if (Hp <= 0)
        {
            OnBreak();
        }
    }

    // ダメージ処理
    protected virtual void TakeDamage(int damage)
    {   
        Hp = Mathf.Max(0, Hp - damage);
        animator.SetTrigger(HitHash);
        // 点滅の演出
        StartCoroutine(Blink());
        
        if(Hp > 0)
        {
            // HPに応じて色を更新
            UpdateColor();            
        }
    }

    // 点滅処理
    private IEnumerator Blink()
    {
        if (isBlinking) yield break;
 
        isBlinking = true;
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.05f);
        }

        sr.enabled = true;
        isBlinking = false;
    }



    /// <summary>
    /// ブロックが壊れた時の処理
    /// （派生クラスで拡張可能）
    /// </summary>
    protected virtual void OnBreak()
    {
        if (isDead) return;

        isDead = true;
        // ブレイクアニメーション
        //animator.SetTrigger(DeathHash);
        StartCoroutine(BreakEffect());
    }

    /// <summary>
    ///  ブレイク演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator BreakEffect()
    {
        // 現在の大きさ保持
        Vector3 start = transform.localScale;
        // 最終拡大サイズ
        Vector3 end = start * deathEndScale;

        // 演出時間
        float t = 0f;        
        const float duration = 0.1f;
        
        // 徐々に拡大
        while (t < duration)
        {
            t += Time.deltaTime;
            // 拡大率を補間
            transform.localScale = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        // 最後の大きさを取得
        transform.localScale = end;
 
        // 死亡アニメーション再生
        animator.SetTrigger(DeathHash);
    }

    /// <summary>
    /// 自分自身の削除処理
    /// </summary>
    public void OnDestroyEnd()
    {
        // 自分自身を削除
        Destroy(transform.parent.gameObject);
        // GameManager にブロック破壊を通知
        GameManager.Instance.OnBlockDestroyed();
        
    }

    /// <summary>
    /// HPや種類に応じてブロックの色を変更する
    /// 派生クラスでオーバーライドする前提
    /// </summary>
    protected virtual void UpdateColor()
    {
        // デフォルト色（通常ブロック用）
        sr.color = Color.white;
        switch (Hp)
        {
            case 3:
                sr.color = Color.white;
                break;

            case 2:
                sr.color = new Color(1f, 0.8f, 0.8f);
                break;

            case 1:
                sr.color = new Color(1f, 0.5f, 0.5f);
                break;
            default:
                sr.color = Color.white;
                break;
        }
    }

}
