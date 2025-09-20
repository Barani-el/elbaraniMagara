using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    [SerializeField] LayerMask hitLayer;
    [SerializeField] int damage;
    [SerializeField] float pushForce;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == hitLayer)
        {
            var d = collision.GetComponent<IDamageable>();
            if (d !=null)
            {
                d.TakeDamage(damage);
            }
            var rb = collision.attachedRigidbody;
            if (rb)
            {
                rb.linearVelocity = new Vector2(transform.right.x * pushForce, rb.linearVelocity.y);
            }
        }
    }
}
