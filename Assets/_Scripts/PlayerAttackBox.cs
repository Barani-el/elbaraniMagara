using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    [SerializeField] int hitLayer;
    [SerializeField] int damage;
    [SerializeField] float pushForce;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Damagable"))
        {
            Debug.Log("ENEMY!");
            var d = collision.GetComponent<IDamageable>();
            if (d !=null)
            {
                d.TakeDamage(damage);
            }
            var rb = collision.attachedRigidbody;
            if (rb)
            {
               rb.AddForce(new Vector2(transform.right.x * pushForce, rb.linearVelocity.y),ForceMode2D.Impulse) ;
            }
        }
    }
}
