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
    [HideInInspector]public Animator animator;
    [Header("Movement Stuff")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float attackDamage;
    bool isFacingRight = true;
    [Header("Attack Stuff")]
    bool isAttacking;
    [SerializeField] int attackIndex = 0;
    [SerializeField] float mintimer,maxtimer;
    [SerializeField] float time;
    [SerializeField] GameObject[] attackList;

    [Header("Jump Variables")]
    public bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public bool isDoubleObtained;
    bool isDoubleJumping;
    [SerializeField] bool isJumpable;
    [Header("Dash Stuff")]
    bool isDashing;
    bool canDash = true;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashPower;
    [SerializeField] float dashAmount;
    [SerializeField] ParticleSystem jumpParticle, dashParticle;

    public bool canInteract ;
    GameObject interactableObject;
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
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMovement();
        }
 
    }
    void Update()
    {
       
        GroundCheck();
        AnimationHandler();

        time += Time.deltaTime;
        time = Mathf.Clamp(time, 0, maxtimer+0.01f);
        if (time > maxtimer)
        {
            
            attackIndex = 0;
            EndAttack();
        }
    }

    public void GiveDamage(int attackIndex)
    {
        attackList[attackIndex].SetActive(true);
    }
    public void CloseDamageBox(int attackIndex)
    {
        attackList[attackIndex].SetActive(false);
    }
    public void Attack()
    {
     
        if (isGrounded) 
        {
            
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
                attackIndex = 0;
                time = 0;
            }
        }
        else if (!isGrounded) // Havadaki attack
        {
            if (time >= mintimer)
            {
                isAttacking = true;
                animator.SetInteger("airState", 4);
            }
          
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
    public void Dash()
    {
        if (!canDash) return;

        Debug.Log("DASH ALTERNATIVE!");

        float dir = transform.localScale.x;

        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        isDashing = true;
        rb.linearVelocity = Vector2.zero;
        dashParticle.Play(rb);
        rb.AddForce(new Vector2(dir* dashPower,0), ForceMode2D.Impulse);
        Debug.Log(rb.linearVelocityY);
        
        rb.gravityScale = gravity;
        canDash = false;

        Invoke(nameof(StopDashing), dashAmount);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    void StopDashing()
    {
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }
    void ResetDash()
    {
        canDash = true;  
       
    }


    public void HandleJump()
    {
        if (isGrounded)
        {
            jumpParticle.Play();
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isDoubleJumping = false;
        }
        else if (isDoubleObtained && isJumpable )
        {
            jumpParticle.Play();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
            rb.AddForce(Vector2.up * jumpPower * 4/5f, ForceMode2D.Impulse);
            isJumpable = false;
            isDoubleJumping = true;
        }
    }

    public void StopJumping()
    {
        if (rb.linearVelocity.y >0)
        {
            Debug.Log("JUMP REALESE");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y*0.5f);
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
 

    public void Interact()
    {
        Debug.LogWarning("Interact");
        if (!canInteract) return;
        Debug.LogWarning("Interact DONE");
        interactableObject.GetComponent<IInteractable>().Interact();
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.LogWarning("Can Save");
            canInteract = true;
            interactableObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            canInteract = false;
            interactableObject = null;
        }
    }

}
