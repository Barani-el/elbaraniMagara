using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Image heartPrefab; // UI > Image prefab
    [SerializeField] private Sprite fullHeart;  // dolu kalp sprite
    [SerializeField] private Sprite emptyHeart; // boþ kalp sprite

    private List<Image> hearts = new List<Image>();
    [SerializeField] Transform heartPos;
    public void BuildHearts(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart=Instantiate(heartPrefab, transform);
            hearts.Add(newHeart);
        }
    }

    public void RefreshHearts(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
