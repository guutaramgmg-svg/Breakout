using UnityEngine;
using UnityEngine.UI;

public class SpManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float increaseSpeed = 0.5f; /// TODO ローグライク化するときにこの値を変動させる
    [SerializeField] PaddleController paddleController;

    void Update()
    {
        // ショット数が最大ならゲージを増やさない
        if(GameManager.Instance.IsShotMax())
        {
            return;
        }

        // ゲージ増加
        slider.value += increaseSpeed * Time.deltaTime;

        // MAX到達
        if (slider.value >= slider.maxValue)
        {
            OnMax();
        }
    }
    
    void OnMax()
    {
        // ショット数を１増やす
        GameManager.Instance.AddShot();
        // ゲージのリセット
        Reset();
    }
    void Reset()
    {
        slider.value = 0f;
    }



}
