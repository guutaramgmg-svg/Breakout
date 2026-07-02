using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections;


public class EnemyStatus : MonoBehaviour
{
    // このブロックの種類
    public EnemyType blockType;

    // ブロックの耐久値（ヒットポイント）
    [SerializeField] protected int hp = 1;

    private Animator animator;
    
    // 見た目変更用の SpriteRenderer
    protected SpriteRenderer sr;

    // 点滅処理
    private bool isBlinking = false;

    /// <summary>
    /// オブジェクト生成時に最初に呼ばれる
    /// 参照の取得は必ずここで行う
    /// </summary>
    protected virtual void Awake()
    {
        // SpriteRenderer を取得
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// ゲーム開始時に一度だけ呼ばれる
    /// </summary>
    protected virtual void Start()
    {
        animator = this.GetComponent<Animator>();
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
        // HPを減らす
        TakeDamage(1);

        // HPに応じて色を更新
        UpdateColor();

        // HPが0以下なら破壊
        if (hp <= 0)
        {
            OnBreak();
        }
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    public virtual void TakeDamageBuster()
    {
        if (isInvincible) return;
        isInvincible = true;
        // HPを減らす
        TakeDamage(1);

        // HPが0以下なら破壊
        if (hp <= 0)
        {
            OnBreak();
        }
    }

    protected virtual void TakeDamage(int dameage)
    {   
        animator.SetTrigger("Hit");
        hp = hp - dameage;
        StartCoroutine(Blink());
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
        animator.SetTrigger("Death");        
    }

    public void OnDestroyEnd()
    {
        // 自分自身を削除
        Destroy(transform.parent.gameObject);
        // GameManager にブロック破壊を通知
        //FindFirstObjectByType<GameManager>()?.OnBlockDestroyed();
        GameManager.Instance.OnBlockDestroyed();
        
    }

    /// <summary>
    /// HPや種類に応じてブロックの色を変更する
    /// 派生クラスでオーバーライドする前提
    /// </summary>
    protected virtual void UpdateColor()
    {
        // デフォルト色（通常ブロック用）
//        sr.color = Color.white;
        switch (hp)
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
                sr.color = Color.red;
                break;
        }
    }

}
