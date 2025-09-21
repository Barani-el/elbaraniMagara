using Unity.VisualScripting;
using UnityEngine;

public class Boss1_Attack : MonoBehaviour, IDamageable
{
    enum AttackState { Idle, Prepare, Charge, Smash }
    AttackState currentState = AttackState.Idle;

    Animator animator;
    [SerializeField] GameObject electric;
    [SerializeField] Transform[] attackOrigins;
    [SerializeField] Transform Player;

    [SerializeField] int attackAmount;
    [SerializeField] float longChargeTime, shortChargeTime;
    [SerializeField] float idleTime = 2f; // saldýrýlar arasý bekleme

    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    [SerializeField] ParticleSystem hitBlood, deadBlood;

    float stateTimer;
    int attackIndex;
    bool isTakingDamage;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    
    void Update()
    {
       

        if (isTakingDamage) return; // damage animasyonu sýrasýnda bekle

        stateTimer -= Time.deltaTime;
        if (stateTimer > 0) return;

        switch (currentState)
        {
            case AttackState.Idle:
                StartPrepare();
                break;

            case AttackState.Prepare:
                StartCharge();
                break;

            case AttackState.Charge:
                StartSmash();
                break;

            case AttackState.Smash:
                if (attackIndex < attackAmount )
                    StartPrepare(shortChargeTime);
                else
                {
                    attackIndex++;
                    UpdateStats();
                    GoIdle();
                }
                break;
        }
    }

    void StartPrepare(float chargeTime = -1f)
    {
        currentState = AttackState.Prepare;
        animator.SetTrigger("Prepare");
        stateTimer = (chargeTime > 0) ? chargeTime : GetAnimLength("Prepare");
    }

    void StartCharge()
    {
        currentState = AttackState.Charge;
        animator.SetTrigger("Charge");
        if (attackIndex > 0) stateTimer = shortChargeTime;
        else if(attackIndex==0) stateTimer = longChargeTime;

    }

    void StartSmash()
    {
        currentState = AttackState.Smash;
        animator.SetTrigger("Smash");
        Shazam(attackIndex);
        attackIndex++;
        stateTimer = GetAnimLength("Smash");
    }

    void GoIdle()
    {
        currentState = AttackState.Idle;
        animator.SetTrigger("Idle");
        attackIndex = 0;
        stateTimer = idleTime; // tekrar döngü için idle bekleme süresi
    }

    float GetAnimLength(string stateName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
            if (clip.name == stateName) return clip.length;
        return 0.5f;
    }

    void Shazam(int i)
    {
        Vector2 dir = (Player.position - attackOrigins[i].position).normalized;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        Instantiate(electric, attackOrigins[i].position, rot);
    }
   
   
    public void TakeDamage(int damageAmount)
    {
        animator.SetTrigger("takeDamage");
        currentHealth -= damageAmount;
        isTakingDamage = true; // saldýrýyý durdur

        // damage animasyonu bitince Event ile EndDamage çaðýrman lazým
        if (currentHealth <= 0)
            Dead();
    }

    public void EndDamage() // Damage animasyonunun sonunda Animation Event ile çaðýr
    {
        isTakingDamage = false;
    }

 
    void UpdateStats()
    {
        attackAmount = Mathf.Clamp(attackAmount,3 ,attackOrigins.Length);
        if (attackIndex % 5 == 0)
            attackAmount++;
    }
    public void Dead()
    {
        animator.SetBool("isDead", true);
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
