using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Text bestScoreText; // Для рекорду
    public Text lastScoreText; // Для останнього результату

    void Start()
    {
        // 1. Отримуємо дані з пам'яті
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        Debug.Log("Рекорд: " + highScore + " | Останній: " + lastScore);

        // 2. Автоматичний пошук (якщо не перетягнули в інспекторі)
        if (bestScoreText == null)
            bestScoreText = GameObject.Find("BestScoreText")?.GetComponent<Text>();

        if (lastScoreText == null)
            lastScoreText = GameObject.Find("LastScoreText")?.GetComponent<Text>();

        // 3. Виведення тексту
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score: " + highScore.ToString();
        }

        if (lastScoreText != null)
        {
            lastScoreText.text = "Your Score: " + lastScore.ToString();
        }
        else
        {
            Debug.LogError("Не вдалося знайти об'єкт lastScoreText! Перетягни його в інспекторі.");
        }
    }
}