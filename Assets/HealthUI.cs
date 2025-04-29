using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Damageable playerDamageable;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image heartImage;

    [Header("Heart Sprites (6 states)")]
    [SerializeField] private Sprite[] heartSprites;

    private void Awake()
    {
        if (playerDamageable == null)
            playerDamageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        // Подписываемся через AddListener
        playerDamageable.HealthChanged.AddListener(UpdateHealthUI);
    }

    private void OnDisable()
    {
        // Отписываемся через RemoveListener
        playerDamageable.HealthChanged.RemoveListener(UpdateHealthUI);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
        float healthPercent = (float)currentHealth / maxHealth;
        int spriteIndex = Mathf.FloorToInt(healthPercent * (heartSprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, heartSprites.Length - 1);
        heartImage.sprite = heartSprites[spriteIndex];
    }

    public void PlayHurtEffect()
    {
        StartCoroutine(HurtEffect());
    }

    private IEnumerator HurtEffect()
    {
        heartImage.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        heartImage.color = Color.white;
    }
}