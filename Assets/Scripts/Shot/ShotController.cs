using UnityEngine;
using UnityEngine.UI;

public class ShotController : MonoBehaviour
{
    [SerializeField] private Image onImage;

    [Header("チャージ速度")]
    [SerializeField] private float fillSpeed = 0.1f;

    private float fillAmount = 0f;
    private bool isFull = false;
    private bool isCharging = false;

    public bool IsFull => isFull;
    public bool IsCharging => isCharging;
    public float FillAmount => fillAmount;


    private void Update()
    {
        if (!isCharging)
        {
            return;
        }

        fillAmount += fillSpeed * Time.deltaTime;
        fillAmount = Mathf.Clamp01(fillAmount);

        onImage.fillAmount = fillAmount;

        if (fillAmount >= 1f)
        {
            fillAmount = 1f;
            isFull = true;
            isCharging = false;
        }
    }


    // チャージ開始
    public void StartCharge()
    {
        if (isFull)
        {
            return;
        }

        isCharging = true;
    }


    // 満タンにする
    public void SetFull()
    {
        fillAmount = 1f;
        isFull = true;
        isCharging = false;

        onImage.fillAmount = 1f;
    }


    // 状態をコピーする
    public void CopyFrom(ShotController source)
    {
        fillAmount = source.fillAmount;
        isFull = source.isFull;
        isCharging = source.isCharging;

        onImage.fillAmount = fillAmount;
    }


    // 空にする
    public void Clear()
    {
        fillAmount = 0f;
        isFull = false;
        isCharging = false;

        onImage.fillAmount = 0f;
    }
}