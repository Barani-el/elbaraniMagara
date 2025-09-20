using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] HeartsUI heartUI;
    [Min(1)] public int maxHealth = 3;
    public int currentHealth;

    
    [SerializeField] ParticleSystem damageParticle;
  

    void Awake()
    {
       currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        damageParticle.Play();
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
        
    }

    public void IncreaseMaxHealth(int amount, bool fillNewHearts = true)
    {
        maxHealth +=amount;
        heartUI.Rebuild(maxHealth);
        heartUI.Refresh(currentHealth,maxHealth);
    }

    public void Die()
    {
        PlayerController.instance.animator.SetBool("isDead", true);
    }

    
}
