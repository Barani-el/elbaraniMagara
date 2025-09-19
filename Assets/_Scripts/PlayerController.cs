using System.IO.IsolatedStorage;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Rigidbody2D rb;
    InputSystem_Actions characterInput;
    InputHandler inputHandler;
    Animator animator;
    [Header("Movement Staff")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float attackDamage;
    bool isFacingRight = true;
    [Header("Attack Staff")]
    bool isAttacking;
    [SerializeField] int attackIndex = 0;
    [SerializeField] float mintimer,maxtimer;
    [SerializeField] float time;

    [Header("Jump Variables")]
    public bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public bool isDoubleObtained;
    bool isDoubleJumping;
    [SerializeField] bool isJumpable;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("asdas");
            Destroy(gameObject);
        }


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<InputHandler>();
    }
  
    void Update()
    {
        HandleMovement();
        GroundCheck();
        AnimationHandler();

        time += Time.deltaTime;
        time = Mathf.Clamp(time, 0, maxtimer+0.01f);
        if (time > maxtimer)
        {
            Debug.Log("timer is out of range");
            attackIndex = 0;
            EndAttack();
        }
    }
    public void Attack()
    {
        Debug.Log("Attack!");
        if (isGrounded) // Sadece yerdeyken combo sistemi çalýþsýn
        {
            //Baran
            if (time >= mintimer  && attackIndex == 0)
            {
                
                isAttacking = true;
                animator.SetBool("isAttackEnd", !isAttacking);
                animator.SetInteger("groundState", 2);
                attackIndex = 1;
                time = 0;
            }
            else if (time >= mintimer && time <= maxtimer && attackIndex == 1) 
            {
                isAttacking = true;
                animator.SetBool("isAttackEnd", !isAttacking);
                animator.SetInteger("groundState", 3);
                attackIndex = 2;
                time = 0;
            }
        }
        else if (!isGrounded) // Havadaki attack
        {
            isAttacking = true;
            animator.SetInteger("airState", 4);
        }
    }
  
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttackEnd", !isAttacking);
    }
    public void HandleMovement()
    {
        HandleRotation();
        Vector2 input = inputHandler.GetMovementVector();
        rb.linearVelocity = new Vector2(input.x * speed, rb.linearVelocity.y);
    }


    public void HandleJump()
    {
        if (isGrounded)
        {
            //animator.SetInteger("airState", 1);
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isDoubleJumping = false;
        }
        else if (isDoubleObtained && isJumpable )
        {
           // animator.SetInteger("airState", 2);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // daha kontrollü
            rb.AddForce(Vector2.up * jumpPower * 4/5f, ForceMode2D.Impulse);
            isJumpable = false;
            isDoubleJumping = true;
        }
    }

    public void HandleRotation()
    {
       
        if (isFacingRight && inputHandler.GetMovementVector().x < 0f || !isFacingRight && inputHandler.GetMovementVector().x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    
    void  GroundCheck()
    {
        
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            isGrounded = true;
            animator.SetInteger("airState", 0);
            isJumpable = true;
        }
        else
        {
            isGrounded = false;
        }
        
    }
 
   
    void AnimationHandler()
    {
        animator.SetBool("isGrounded", isGrounded);

        if (isAttacking) return;
        // Ground State (Idle / Run)
        if (isGrounded)
        {
            if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                animator.SetInteger("groundState", 1); // Run
            else
                animator.SetInteger("groundState", 0); // Idle
        }
        else
        {
            // Air State
            if (rb.linearVelocity.y > 0.1f)
            {
                if (!isDoubleJumping)
                {
                    animator.SetInteger("airState", 1);
                }
                else
                {
                    animator.SetInteger("airState", 2);
                }
                 // Jump
            }
            else if (rb.linearVelocity.y < -0.1f)
                animator.SetInteger("airState", 3); // Fall
        }
    }


}
