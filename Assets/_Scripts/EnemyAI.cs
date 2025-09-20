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
    public float attackCooldown;        
    public float attackTimer;
    [SerializeField] GameObject attackBox;
    bool facingRight = true;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isChasing;
    public bool onAttack;


    [SerializeField] float distance;


    [SerializeField] GameObject helmet;
    [SerializeField] Transform headLevel;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
    }
    private void Start()
    {
        animator.SetInteger("idleVariant", idleVariant);
    }

    private void FixedUpdate()
    {
        animator.SetBool("onAttack", isAttacking);

        distance = Vector2.Distance(transform.position, player.position);
        
        if (distance < detectRange && distance >attackRange && !onAttack)
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

            if (attackTimer >= attackCooldown && !onAttack)
            {
                rb.linearVelocity = Vector2.zero;
                onAttack = true;
                animator.SetBool("onAttack", onAttack);
                int attackState = Random.Range(2, 4);
                Debug.Log(attackState);
                animator.SetInteger("state", attackState);
                
            }
            else
            {
                
                
                
            }
        }

        else if (isChasing )
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

    public void OpenDamageBox()
    {
        attackBox.SetActive(true);
    }
    public void EndAttack()
    {
        
        attackBox.SetActive(false);
        onAttack = false;

        attackTimer = 0;
        animator.SetInteger("state", idleVariant);
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
    public void HelmetOff()
    {
        GameObject helmet_ = Instantiate(helmet, headLevel.position, Quaternion.identity);
        float backDir = facingRight ? -1f : 1f;
        Vector2 launcDir = new Vector2(backDir,Random.Range(0.8f,1.2f)).normalized;
        helmet_.GetComponent<Rigidbody2D>().AddForce(launcDir * Random.Range(5,8),ForceMode2D.Impulse);
        helmet_.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-30f,30f),ForceMode2D.Impulse);
    }

}
