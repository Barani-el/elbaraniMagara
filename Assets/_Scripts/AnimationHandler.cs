using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public Animator animator;

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
        animator.SetInteger("groundState", 1);
    }
}
