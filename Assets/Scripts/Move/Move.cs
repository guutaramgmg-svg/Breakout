using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector3 previousPosition;

    private static readonly int XHash = Animator.StringToHash("X");
    private static readonly int YHash = Animator.StringToHash("Y");

    private Vector2 lastDirection = Vector2.down; // 初期は下向き

//    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        Vector3 movement = transform.position - previousPosition;
        if(movement.sqrMagnitude > 0.0001f)
        {
            lastDirection = movement.normalized;
            animator.SetFloat(XHash, movement.x);
            animator.SetFloat(YHash, movement.y);           
        }
        previousPosition = transform.position;
    }

}
