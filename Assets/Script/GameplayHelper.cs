using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayHelper : MonoBehaviour
{
    public static GameplayHelper Instance { get; private set; }

    private Label scoreText;
    private Label TimerText;

    public int trapScore;
    public int towerScore;
    public int tankScore;
    public int mortarScore;

    public int score;
    private float elapsedTime;
    private float lastScoreUpdateTime;

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

        var root = GetComponent<UIDocument>().rootVisualElement;
        TimerText = root.Q<Label>("TimerText");
        scoreText = root.Q<Label>("ScoreText");

        TimerText.text = "TIME: 00:00";
        scoreText.text = "SCORE: 0";
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        TimerText.text = $"TIME: {minutes:00}:{seconds:00}";
        
        if (elapsedTime >= lastScoreUpdateTime + 5f)
        {
            score += 10;
            lastScoreUpdateTime = elapsedTime;
        }

        scoreText.text = $"SCORE: {score}";
    }

    public void KilledEnemy(string tag)
    {
        switch (tag)
        {
            case "TankTrap": score += trapScore; break;
            case "Tower": score += towerScore; break;
            case "EnemyTank": score += tankScore; break;
            case "Mortar": score += mortarScore; break;
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }
}
