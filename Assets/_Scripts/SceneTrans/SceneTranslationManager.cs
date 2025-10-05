using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTranslationManager : MonoBehaviour
{
    public static SceneTranslationManager instance;

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
        DontDestroyOnLoad(instance);
        
    }
    public void ChangeScene_(GameObject player, string targetScene, Vector3 spawnPoint, int scale)
    {
        StartCoroutine(ChangeScene(player,targetScene,spawnPoint,scale));
    }
    
    public IEnumerator ChangeScene(GameObject player,string targetScene,Vector3 spawnPoint,int scale)
    {
        UI.instance.CloseScreen();
        yield return new WaitForSeconds(1f);
        // Karakter kontrolcüsü kapat
        SceneManager.LoadScene(targetScene);
        player.transform.position = spawnPoint;
        player.transform.rotation = Quaternion.Euler(scale, player.transform.rotation.y, player.transform.rotation.z);
        //Karater kontrolcüsü aç 
        yield return new WaitForSeconds(0.5f);
        UI.instance.OpenScreen();
    }
}
