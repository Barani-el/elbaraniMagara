using UnityEngine;

public class CheckPoint : MonoBehaviour,IInteractable
{
   public void Interact()
    {
        Debug.Log("Saved!!");
        if (Vector2.Distance(transform.position, SaveManager.instance.GetSavedPoint()) < 1) return;
       

        SaveManager.instance.Save(transform.position);
    }
}
