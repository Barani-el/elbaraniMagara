using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Health target;
    public Transform container;      // Horizontal/Vertical Layout Group önerilir
    public GameObject heartPrefab;   // Ýçinde Image olan küçük bir prefab
    public Sprite fullHeart;
    public Sprite emptyHeart;

    readonly List<Image> hearts = new();

    void OnEnable()
    {
        if (!target) return;
        target.OnMaxHealthChanged.AddListener(Rebuild);
        target.OnHealthChanged.AddListener(Refresh);
        Rebuild(target.MaxHealth);
        Refresh(target.CurrentHealth, target.MaxHealth);
    }

    void OnDisable()
    {
        if (!target) return;
        target.OnMaxHealthChanged.RemoveListener(Rebuild);
        target.OnHealthChanged.RemoveListener(Refresh);
    }

    void Rebuild(int max)
    {
        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            if (hearts[i]) Destroy(hearts[i].gameObject);
        }
        hearts.Clear();

        for (int i = 0; i < max; i++)
        {
            var go = Instantiate(heartPrefab, container);
            var img = go.GetComponent<Image>();
            hearts.Add(img);
        }
    }

    void Refresh(int current, int max)
    {
        if (hearts.Count != max) Rebuild(max);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = i < current ? fullHeart : emptyHeart;
            hearts[i].SetNativeSize();
        }
    }
}
