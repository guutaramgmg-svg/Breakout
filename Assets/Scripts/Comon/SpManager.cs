using UnityEngine;
using UnityEngine.UI;

public class SpManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float increaseSpeed = 0.5f;
    [SerializeField] PaddleController paddleController;

    void Update()
    {
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
　　　　　//Debug.Log("アクション発動！");
        paddleController.BoolShot();
        Reset();
    }
    void Reset()
    {
        slider.value = 0f;
    }



}
