using UnityEngine;
using UnityEngine.UI;

public class ShotController : MonoBehaviour
{
    [SerializeField] private Image onImage;

    [SerializeField] private float fillAmount = 0f;
    [SerializeField] private float fillSpeed = 0.1f;

    [SerializeField] private bool isFull = false;

    // 満タンかどうか
    public bool IsFull => isFull;


    private void Update()
    {
        if (isFull)
        {
            return;
        }

        // ゲージを増やす
        fillAmount += fillSpeed * Time.deltaTime;

        fillAmount = Mathf.Clamp01(fillAmount);

        onImage.fillAmount = fillAmount;

        if (fillAmount >= 1f)
        {
            fillAmount = 1f;
            isFull = true;
        }
    }


    // ショットを使用
    public void ShotLost()
    {
        fillAmount = 0f;
        isFull = false;

        onImage.fillAmount = fillAmount;
    }


    // チャージ開始
    public void Reset()
    {
        isFull = false;
    }


    // 最初から満タン
    public void SetFull()
    {
        fillAmount = 1f;
        isFull = true;

        onImage.fillAmount = fillAmount;
    }
}