using System.Collections;
using UnityEngine;

public class BloodQuen : MonoBehaviour,IDamageable
{
    Animator animator;
    Transform Player;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] GameObject blood;

    [SerializeField] ParticleSystem hitBlood, deadBlood;
    [SerializeField] float attackDelay;
    [SerializeField] int bloodAmount;
    int attackIndex;
    [SerializeField] Transform floorLevel;

    [SerializeField] BoxCollider2D[] colliders;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    void Start()
    {
        StartCoroutine(Attack());
    }
    void Update()
    {
      
       
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);
        SpawnBlood();
        attackIndex++;
        UpdateStats();
        yield return new WaitForSeconds(attackDelay);
        StartCoroutine(Attack());
      
    }

    void SpawnBlood()
    {
        for (int i = 0; i < bloodAmount; i++)
        {
            Vector2 targetPos = new Vector2(Random.Range(Player.position.x - 10, Player.position.x + 10), floorLevel.position.y+4);
            Instantiate(blood, targetPos, Quaternion.identity);
        }
        
    }
   
    void UpdateStats()
    {
        bloodAmount = Mathf.Clamp(bloodAmount, 3, 6);
        if (attackIndex % 5 == 0)
            bloodAmount++;
    }
    public void TakeDamage(int damageCount)
    {
        if (currentHealth <= 0) return;
        animator.SetTrigger("TakeDamage");
        currentHealth -= bloodAmount;
      
        if (currentHealth <= 0)
            Dead();
    }
    public void Dead()
    {
        CameraShake.Instance.Shake(1.5f, 0.2f, 2);
        animator.SetTrigger("Die");
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        StopAllCoroutines();
    }
    public void DeadBloodEffect()
    {
        deadBlood.Play();
    }

    public void HitBloodEffect()
    {
        hitBlood.Play();
    }

    public void Destroy()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
