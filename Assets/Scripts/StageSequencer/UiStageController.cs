using Unity.VisualScripting;
using UnityEngine;

public class UiStageController : MonoBehaviour
{
    public void UiDestroy()
    {
        this.gameObject.SetActive(false);
    }
}
