using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Min(1)] public int MaxHealth = 3;
    public int CurrentHealth { get; private set; }

    public UnityEvent<int, int> OnHealthChanged;   // (current, max)
    public UnityEvent<int> OnMaxHealthChanged;    // (max)
    public UnityEvent OnDeath;

    bool IsDead => CurrentHealth <= 0;

    void Awake()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth == 0 ? MaxHealth : CurrentHealth, 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnMaxHealthChanged?.Invoke(MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth == 0) Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead) return;
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void IncreaseMaxHealth(int amount, bool fillNewHearts = true)
    {
        if (amount <= 0) return;
        MaxHealth += amount;
        if (fillNewHearts) CurrentHealth = MaxHealth;
        else CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void Die()
    {
        if (IsDead) OnDeath?.Invoke();
    }
}
