using System.Collections.Generic;
using UnityEngine;

public class ShotStockController : MonoBehaviour
{
    public static ShotStockController Instance { get; private set; }

    [Header("最大ショット数")]
    [SerializeField] private int maxShotCount = 5;

    [Header("ショットプレハブ")]
    [SerializeField] private ShotController shotPrefab;

    [Header("ショットを並べる親")]
    [SerializeField] private Transform shotParent;

    private List<ShotController> shots = new List<ShotController>();

    // 現在チャージ中のShot
    private ShotController chargingShot;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        CreateShots();

        // 最初はチャージ不要
        // 全部満タンなので何もしない
    }


    private void Update()
    {
        // 現在チャージ中のShotが満タンになった
        if (chargingShot != null && chargingShot.IsFull)
        {
            chargingShot = null;
        }

        // チャージ中のShotがなければ
        // 空いているShotを1個だけチャージ
        if (chargingShot == null)
        {
            StartNextCharge();
        }
    }


    // =========================
    // Shot生成
    // =========================

    private void CreateShots()
    {
        for (int i = 0; i < maxShotCount; i++)
        {
            CreateShot();
        }
    }


    private void CreateShot()
    {
        ShotController shot = Instantiate(shotPrefab, shotParent);

        shots.Add(shot);

        // 最初は満タン
        shot.SetFull();
    }


    // =========================
    // 次にチャージするShotを探す
    // =========================

    private void StartNextCharge()
    {
        for (int i = 0; i < shots.Count; i++)
        {
            // 満タンなら無視
            if (shots[i].IsFull)
            {
                continue;
            }

            // このShotがすでにチャージ中なら無視
            if (shots[i].IsCharging)
            {
                continue;
            }

            // チャージ対象にする
            chargingShot = shots[i];

            chargingShot.StartCharge();

            return;
        }
    }


    // =========================
    // ショットを使用
    // =========================

public void Shot()
{
    // 右から満タンのShotを探す
    for (int i = shots.Count - 1; i >= 0; i--)
    {
        if (shots[i].IsFull)
        {
            // このShotを消費
            shots[i].Clear();

            // 状態を左詰めする
            MoveShotsLeft(i);

            return;
        }
    }
}
private void MoveShotsLeft(int emptyIndex)
{
    for (int i = emptyIndex; i < shots.Count - 1; i++)
    {
        // 次のShotが現在チャージ中なら、
        // 移動先を新しいチャージ対象にする
        if (chargingShot == shots[i + 1])
        {
            chargingShot = shots[i];
        }

        shots[i].CopyFrom(shots[i + 1]);
    }

    // 一番右を空にする
    shots[shots.Count - 1].Clear();
}
    // =========================
    // 最大ショット数を増やす
    // =========================

    public void AddMaxShot(int amount)
    {
        maxShotCount += amount;

        while (shots.Count < maxShotCount)
        {
            CreateShot();
        }
    }
}