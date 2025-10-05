using UnityEngine;

public class SceneTranslationPoint : MonoBehaviour
{
    public string targetSceneName;
    public Vector3 targetPos;
    public int face;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneTranslationManager.instance.ChangeScene_(collision.gameObject,targetSceneName,targetPos,face);
        }
    }
}
