using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Min(1)] public int maxHealth = 3;
    public int Current { get; private set; }

    public UnityEvent OnDeath;

    bool dead;

    void Awake() => Current = maxHealth;

    public void TakeDamage(int damageAmount)
    {
        if (dead || damageAmount <= 0) return;
        Current = Mathf.Max(0, Current - damageAmount);
        if (Current == 0) Die();
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        OnDeath?.Invoke();
    }

    public bool IsDead() => dead;
}
