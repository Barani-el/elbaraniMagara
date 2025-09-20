using UnityEngine;

public class Electric : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OpenHitBox()
    {
        boxCollider.enabled = true;
    }
    public void Execute()
    {
        Destroy(gameObject);
    }

}
