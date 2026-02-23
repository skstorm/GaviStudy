using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text linesText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    public int Score { get; private set; }
    public int Level { get; private set; } = 1;
    public int LinesCleared { get; private set; }
    public bool IsGameOver { get; private set; }

    private const int LinesPerLevel = 10;
    private static readonly int[] ScoreTable = { 0, 100, 300, 500, 800 };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public float GetFallInterval()
    {
        return Mathf.Max(0.05f, 1.0f - (Level - 1) * 0.1f);
    }

    public void AddScore(int lines)
    {
        if (lines <= 0 || lines > 4) return;

        Score += ScoreTable[lines] * Level;
        LinesCleared += lines;

        int newLevel = (LinesCleared / LinesPerLevel) + 1;
        Level = newLevel;

        UpdateUI();
    }

    public void GameOver()
    {
        IsGameOver = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + Score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = Score.ToString();
        if (levelText != null) levelText.text = Level.ToString();
        if (linesText != null) linesText.text = LinesCleared.ToString();
    }
}
