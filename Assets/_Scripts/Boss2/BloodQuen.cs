using System.Collections;
using UnityEngine;

public class BloodQuen : MonoBehaviour
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

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    void Start()
    {
        Attack();
    }
    void Update()
    {
        UpdateStats();
       
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);
        SpawnBlood();
    }

    void SpawnBlood()
    {
        for (int i = 0; i < bloodAmount; i++)
        {
            Vector2 targetPos = new Vector2(Random.Range(Player.position.x - 20, Player.position.x + 20), 0);
            Instantiate(blood, targetPos, Quaternion.identity);
        }

    }

  

    public void Dead()
    {
        animator.SetBool("isDead", true);
    }
    void UpdateStats()
    {
        bloodAmount = Mathf.Clamp(bloodAmount, 3, 6);
        if (attackIndex % 5 == 0)
            bloodAmount++;
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
        Destroy(gameObject);
    }
}
