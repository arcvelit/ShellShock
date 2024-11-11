using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public Canvas gameOverCanvas; 

    public UIDocument uiDocument;
    public Text scoreText;

    public float maxHealth;
    public float health;
    public float percentHealth => health / maxHealth;
    public float dangerLifeThreshold;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        HealFully();
    }

    public void InflictDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
        else if (percentHealth < dangerLifeThreshold && !UIManager.Instance.flickering)
        {
            StartCoroutine(UIManager.Instance.FlickerHealth());
        }
        else
        {
            UIManager.Instance.UpdateHealthBar(percentHealth);
        }
    }

    public void HealFully()
    {
        health = maxHealth;
        UIManager.Instance.UpdateHealthBar(percentHealth);
    }

    public void Kill()
    {
        Die();
    }

    private void Die()
    {
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        gameOverCanvas.gameObject.SetActive(true);
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        scoreText.text = "SCORE\n"+GameplayHelper.Instance.score.ToString();


    }
}