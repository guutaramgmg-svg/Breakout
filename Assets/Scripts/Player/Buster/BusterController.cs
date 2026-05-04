using UnityEngine;

public class BusterController : MonoBehaviour
{
    PaddleController paddleController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paddleController = FindAnyObjectByType<PaddleController>();
        

    }

    // Update is called once per frame
    void Update()
    {
     this.transform.position = paddleController.transform.position;
    }

}
