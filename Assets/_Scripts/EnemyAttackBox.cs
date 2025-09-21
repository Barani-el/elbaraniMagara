using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    [SerializeField] int hitLayer;
    [SerializeField] int damage;
    [SerializeField] float pushForce;
    BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider =GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") )
        {
            Debug.Log("PLAYER!");
            var d = collision.GetComponent<IDamageable>();
            if (d != null)
            {
                boxCollider.enabled = false;
                d.TakeDamage(damage);
            }
            var rb = collision.attachedRigidbody;
            if (rb)
            {
                rb.AddForce(new Vector2(transform.right.x * pushForce, rb.linearVelocity.y), ForceMode2D.Impulse);
            }
        }
    }
}
