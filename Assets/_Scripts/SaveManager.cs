using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] Vector3 lastSavedPoint;
    [SerializeField] int lastCurrentHealth;

    [SerializeField] Transform firstSpawnPoint;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else Destroy(this.gameObject);
        LoadSavePoint();
    }
    public void Save(Vector3 savePoint)
    {
        lastSavedPoint = savePoint;
        PlayerPrefs.SetFloat("SpawnX", savePoint.x);
        PlayerPrefs.SetFloat("SpawnY", savePoint.y);
        PlayerPrefs.SetFloat("SpawnZ", savePoint.z);
        PlayerPrefs.SetInt("CurrentHealth", PlayerHealthSystem.instance.currentHealth);
        PlayerPrefs.Save();

    }

    public Vector3 GetSavedPoint()
    {
        return lastSavedPoint;
    }

    public void UpdateHealth()
    {
        lastCurrentHealth = PlayerHealthSystem.instance.currentHealth;
    }
    public int GetCurrentHealth()
    {
        return lastCurrentHealth; 
    }
    public void LoadSavePoint()
    {
        if (PlayerPrefs.HasKey("SpawnX"))
        {
            lastSavedPoint = new Vector3(
                PlayerPrefs.GetFloat("SpawnX"),PlayerPrefs.GetFloat("SpawnY"),PlayerPrefs.GetFloat("SpawnZ")
                );
            lastCurrentHealth = PlayerPrefs.GetInt("CurrentHealth");
        }
        else
        {
            lastSavedPoint = firstSpawnPoint.position;
            lastCurrentHealth = PlayerHealthSystem.instance.maxHealth;
        }
    }
}
