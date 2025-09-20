using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    [Min(1)] public int maxHealth = 3;
    public int currentHealth;

    [SerializeField] ParticleSystem damageParticle;
    [SerializeField] HeartsUI heartsUI;

    void Awake()
    {
        currentHealth = maxHealth;
        heartsUI.BuildHearts(maxHealth);
        heartsUI.RefreshHearts(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        damageParticle.Play();
        PlayerController.instance.animator.SetTrigger("takeDamage");

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        heartsUI.RefreshHearts(currentHealth, maxHealth);

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
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            heartsUI.RefreshHearts(currentHealth, maxHealth);
        }
    }

    public void IncreaseMaxHealth(int amount, bool fillNewHearts = true)
    {
        maxHealth += amount;
        if (fillNewHearts) currentHealth = maxHealth;

        heartsUI.BuildHearts(maxHealth);
        heartsUI.RefreshHearts(currentHealth, maxHealth);
    }

    public void Die()
    {
        PlayerController.instance.animator.SetBool("isDead", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hit"))
        {
            Debug.Log(currentHealth);
            TakeDamage(1);
        }
    }
}
