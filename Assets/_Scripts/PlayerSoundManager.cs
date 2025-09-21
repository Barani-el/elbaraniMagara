using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager instance;
    float basePitch;
    float baseVol;
    [SerializeField] AudioSource audioSource,walkAudioSource;
    [Header("Walk")]
    [SerializeField] AudioClip walk;
    [Header("Attack")]
    [SerializeField] AudioClip attack;
    [SerializeField] float attackMinPitch = 0.92f;
    [SerializeField] float attackMaxPitch = 1.07f;
    [Header("Jump")]
    [SerializeField] AudioClip jump;
    [SerializeField] float firstJumpPitch;
    [SerializeField] float doubleJumpPitch;
    [Header("Dash")]
    [SerializeField] AudioClip dash;
    [Header("TakeDamage")]
    [SerializeField] AudioClip takeDamage;
    [SerializeField] float takeDamageMinPitch = 0.95f;
    [SerializeField] float takeDamageMaxPitch = 1.1f;
    [Header("Die")]
    [SerializeField] AudioClip die;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
       
        basePitch = audioSource.pitch;

    }
    public void Attack()
    {
        
        audioSource.pitch = Random.Range(attackMinPitch,attackMaxPitch);
        audioSource.PlayOneShot(attack);
        
    }

    public void Jump(int jumpCount)
    {
        if (jumpCount == 0)
        {
            audioSource.pitch = firstJumpPitch;

        }
        else { audioSource.pitch = doubleJumpPitch;}

        audioSource.PlayOneShot(jump);
    }
    public void Dash()
    {
        audioSource.PlayOneShot(dash);
    }

    public void TakeDamage()
    {
        audioSource.pitch = Random.Range(takeDamageMinPitch,takeDamageMaxPitch);
        audioSource.PlayOneShot(takeDamage);
    }
    public void Die()
    {
        audioSource.pitch = basePitch;
        audioSource.PlayOneShot(die);
    }

    public void StartWalking()
    {
       if (!walkAudioSource.isPlaying)
        {
            walkAudioSource.clip = walk;
            walkAudioSource.loop = true;
            walkAudioSource.pitch = basePitch;
            walkAudioSource.Play();
        }
    }

    public void StopWalking()
    {
        walkAudioSource.Stop();
    }
}
