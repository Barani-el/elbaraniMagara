using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Rigidbody2D rb;
    InputSystem_Actions characterInput;
    InputHandler inputHandler;

    [Header("Movement Staff")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float attackDamage;

    [Header("Jump Variables")]
    bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public bool isDoubleObtained;
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
        inputHandler = GetComponent<InputHandler>();
    }
  
    void Update()
    {
        HandleMovement();
        GroundCheck();
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
            
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        

        
        else
        {
            if (isDoubleObtained && isJumpable)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 1 / 4);
                rb.AddForce(Vector2.up * jumpPower *5/6, ForceMode2D.Impulse);
                isJumpable = false;
            }

        }
        

    }
    public void HandleRotation()
    {
        if (inputHandler.GetMovementVector().x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    void  GroundCheck()
    {
        
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            isGrounded = true;
            isJumpable = true;
        }
        else
        {
            isGrounded = false;
        }
        
    }

}
