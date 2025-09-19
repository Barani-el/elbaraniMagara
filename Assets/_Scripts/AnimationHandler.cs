using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        HandleAnim();
    }

    public void HandleAnim()
    {
        animator.SetBool("isGrounded", PlayerController.instance.isGrounded);
    }
}
