using UnityEngine;

public class Heart : MonoBehaviour,IDamageable
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] int maxHP;
    [SerializeField] int currentHp;

    [SerializeField] ParticleSystem hitBlood, deadBlood;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentHp = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (currentHp<=0) return;
        hitBlood.Play();
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        animator.SetBool("isDead", true);
        deadBlood.Play();
    }
   
    public void HeartBeat(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
