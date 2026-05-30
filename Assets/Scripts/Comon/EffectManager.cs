using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
public class EffectManager : MonoBehaviour
{
    // MouseUpエフェクトプレハブ
    [SerializeField] GameObject mouseUpEffectPrefab;
    // MouseDownエフェクトプレハブ
    [SerializeField] GameObject MouseDownEffectPrefab;

    // MouseDragエフェクトプレハブ
    [SerializeField] GameObject MouseDragEffectPrefab;

    [SerializeField] float dragEffectInterval = 0.05f;


    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

            Vector2 screenPos =
                Pointer.current.position.ReadValue();

            Vector3 worldPos =
                Camera.main.ScreenToWorldPoint(
                    new Vector3(screenPos.x, screenPos.y, 10f)
                );

            worldPos.z = 0;

        // なし
        if (Pointer.current == null) return;

        // タップした瞬間
        if (Pointer.current.press.wasPressedThisFrame)
        {
            GameObject effect = Instantiate(MouseDownEffectPrefab, worldPos,Quaternion.identity,transform);
            Destroy(effect, 0.5f);            
        }

        // タップ中
        if (Pointer.current.press.isPressed)
        {
            timer += Time.deltaTime;
            if(timer >= dragEffectInterval)
            {
                timer = 0f;
                GameObject effect = Instantiate(MouseDragEffectPrefab, worldPos, Quaternion.identity,transform);
                Destroy(effect, 0.6f);            
            }
        }

        // タップ離した瞬間
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            GameObject effect = Instantiate(mouseUpEffectPrefab, worldPos,Quaternion.identity);
            Destroy(effect, 0.5f);                        
        }
    }
}
