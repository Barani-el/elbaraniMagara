using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Idle seçimi (0 veya 1)")]
    [Range(0, 1)] public int idleVariant = 0;
    public Transform player;
    Animator animator;
    Rigidbody2D rb;

    [Header("Hareket & Algı")]
    public float moveSpeed = 2.2f;         
    public float detectRange = 8f;              
    public float attackRange = 1.4f;            
    public float attackCooldown = 0.9f;         
    public float attackLockDuration = 0.5f;
    float attackTimer;

    bool facingRight = true;
    bool isAttacking = false;
    //[SerializeField] float attackCooldown;
    
    bool isChasing;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectRange && distance >attackRange)
        {
            isChasing = true;
            isAttacking = false;
        }
        else if(distance < attackRange)
        {
            isChasing = false;
            isAttacking = true;
        }
        else
        {
            isChasing = false;
            isAttacking = false;
        }

        if (!isAttacking) FacePlayer();

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            if (attackTimer >= attackCooldown)
            {
                int attackState = Random.Range(0, 2);
                animator.SetInteger("attackState", attackState);

            }
        }

        else if (isChasing)
        {
            animator.SetInteger("state", 1);
            float dir = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        }

        else
        {
            animator.SetInteger("state", 0);
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {

        attackTimer += Time.deltaTime;
        attackTimer = Mathf.Clamp(attackTimer,0,attackCooldown);

       
    }

    void FacePlayer()
    {
        bool shouldFaceRight = player.position.x > transform.position.x;
        if (shouldFaceRight != facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }
  
}
