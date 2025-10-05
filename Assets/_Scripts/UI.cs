using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    Animator animator;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        animator = GetComponent<Animator>();
    }
    public void CloseScreen()
    {
        animator.SetBool("isBlack", true);
    }

    public void OpenScreen()
    {
        animator.SetBool("isBlack", false);
    }
}
