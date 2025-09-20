using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    EnemyAI enemyAI;
    Animator animator;
    Rigidbody2D rb;
    [Min(1)][SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    bool isDead;
    [SerializeField] BoxCollider2D[] colliders;

 
    private void Awake()
    {
        animator = GetComponent<Animator>();  
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        enemyAI.animator.SetTrigger("takeDamage");
        currentHealth -=damage;
        Debug.Log("Damage Taken - Enemy");
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        enemyAI.enabled = false;
        isDead = true;
        
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        animator.SetBool("isDead", isDead);

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        
    }


    private void Update()
    {
       currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);    
    }
}
