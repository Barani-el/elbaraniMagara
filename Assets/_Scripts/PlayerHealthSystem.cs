using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    public static PlayerHealthSystem instance;
    [Min(1)] public int maxHealth = 3;
    public int currentHealth;

    [SerializeField] ParticleSystem damageParticle;
    [SerializeField] HeartsUI heartsUI;
    [SerializeField] Animator screenAnimator;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
        currentHealth = maxHealth;
        heartsUI.BuildHearts(maxHealth);
        heartsUI.RefreshHearts(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        damageParticle.Play();
        PlayerController.instance.animator.SetTrigger("takeDamage");

        currentHealth -= amount;
        SaveManager.instance.UpdateHealth();
        if (currentHealth < 0) currentHealth = 0;

        heartsUI.RefreshHearts(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
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

    IEnumerator Die()
    {
        PlayerController.instance.animator.SetBool("isDead", true);
        //Karakter kontroller kapanýcak burada
        screenAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(1);
        Born();
    }

    IEnumerator LittleDie()
    {
        screenAnimator.SetTrigger("Close");
        //Karakter kontroller off
        yield return new WaitForSeconds(1);
        Born();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hit"))
        {
            PlayerSoundManager.instance.TakeDamage();
            TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("spike"))
        {
            PlayerSoundManager.instance.TakeDamage();
            TakeDamage(1);
            if(currentHealth > 0) StartCoroutine(LittleDie());

        }
    }

    public void Born()
    {
        Vector3 spawnpoint = SaveManager.instance.GetSavedPoint();
        int spawnHealth = SaveManager.instance.GetCurrentHealth();

        PlayerController.instance.animator.SetBool("isDead", false);
        //Karaker kontrolcüsü açýlacak burada
        transform.position = spawnpoint;

        if (spawnHealth <= 0)
        {
            Debug.Log("can yenileniyor");
            currentHealth = maxHealth;
            heartsUI.RefreshHearts(currentHealth, maxHealth);
           
        }
        else currentHealth = spawnHealth;
     
        screenAnimator.SetTrigger("Open");
    }
}
