using System.Collections;
using UnityEngine;

public class DisappearPlatform : MonoBehaviour
{
    [SerializeField] float disappearDelay = 1.5f;
    [SerializeField] float respawnDelay = 3f; // tekrar görünme süresi

    Collider2D col;
    SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);

        // Objeyi gizle
        col.enabled = false;
        sr.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Objeyi geri aç
        col.enabled = true;
        sr.enabled = true;
    }
}
