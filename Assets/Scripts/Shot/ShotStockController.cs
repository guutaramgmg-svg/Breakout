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

        // 最初のチャージ対象を探す
        StartNextCharge();
    }


    private void Update()
    {
        // 現在チャージ中のShotがあるか確認
        if (chargingShot != null)
        {
            // 満タンになったら
            if (chargingShot.IsFull)
            {
                chargingShot = null;
            }
        }

        // チャージ中のShotがなければ次を探す
        if (chargingShot == null)
        {
            StartNextCharge();
        }
    }


    // ショットを生成
    private void CreateShots()
    {
        for (int i = 0; i < maxShotCount; i++)
        {
            CreateShot();
        }
    }


    // ショットを1個生成
    private void CreateShot()
    {
        ShotController shot = Instantiate(shotPrefab, shotParent);

        shots.Add(shot);

        // 最初は満タン
        shot.SetFull();
    }


    // 次にチャージするShotを探す
    private void StartNextCharge()
    {
        for (int i = 0; i < shots.Count; i++)
        {
            // すでに満タンなら次を見る
            if (shots[i].IsFull)
            {
                continue;
            }

            // このShotをチャージ対象にする
            chargingShot = shots[i];

            // チャージ開始
            chargingShot.Reset();

            return;
        }
    }


    // 最大ショット数を増やす
    public void AddMaxShot(int amount)
    {
        maxShotCount += amount;

        // 増えた分だけ生成
        while (shots.Count < maxShotCount)
        {
            CreateShot();
        }
    }


    // ショットを使用
    public void Shot()
    {
        for (int i = 0; i < shots.Count; i++)
        {
            if (shots[i].IsFull)
            {
                shots[i].ShotLost();
                return;
            }
        }
    }
}