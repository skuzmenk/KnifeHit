using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Settings")]
    public Text inGameScoreText;
    public Text knivesText;

    [Header("Knife Settings")]
    // Замінили один ніж на масив для вибору з магазину
    public GameObject[] knifePrefabs;
    public Transform spawnPoint;

    [Header("Asset Store Sprites")]
    public Sprite[] normalTargetSprites;
    public Sprite[] bossTargetSprites;

    [Header("Level Settings")]
    public int knivesLeft = 5;
    private int score = 0;
    private int level = 1;

    private GameObject currentKnife;
    private TargetRotation target;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = FindObjectOfType<TargetRotation>();

        // Налаштовуємо рівень та оновлюємо UI
        UpdateTargetVisuals();
        UpdateUI();

        SpawnKnife();
    }

    void UpdateTargetVisuals()
    {
        if (target == null) return;

        bool isBossLevel = (level % 5 == 0);

        if (isBossLevel)
        {
            if (bossTargetSprites.Length > 0)
            {
                Sprite bossSprite = bossTargetSprites[Random.Range(0, bossTargetSprites.Length)];
                // Передаємо sprite, статус боса та номер рівня для швидкості
                target.ChangeTargetAppearance(bossSprite, true, level);
            }
            knivesLeft = Random.Range(10, 14);
        }
        else
        {
            if (normalTargetSprites.Length > 0)
            {
                Sprite normalSprite = normalTargetSprites[Random.Range(0, normalTargetSprites.Length)];
                target.ChangeTargetAppearance(normalSprite, false, level);
            }
            knivesLeft = Random.Range(4, 8);
        }

        UpdateUI();
    }

    public void OnHit()
    {
        // Нарахування балів: 2 за боса (кожен 5-й рівень), 1 за звичайний
        if (level % 5 == 0)
        {
            score += 2;
        }
        else
        {
            score += 1;
        }

        knivesLeft--;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        UpdateUI();
        currentKnife = null;

        if (knivesLeft > 0)
        {
            Invoke("SpawnKnife", 0.1f);
        }
        else
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        level++;
        UpdateTargetVisuals();
        ClearKnives();
        Invoke("SpawnKnife", 0.2f);
    }

    void SpawnKnife()
    {
        if (currentKnife != null) return;

        // Отримуємо індекс ножа, який гравець обрав у магазині
        int selectedIndex = PlayerPrefs.GetInt("SelectedKnife", 0);

        // Перевірка, щоб індекс не виходив за межі масиву префабів
        if (selectedIndex >= knifePrefabs.Length)
        {
            selectedIndex = 0;
        }

        // Створюємо саме той ніж, який обрано
        currentKnife = Instantiate(knifePrefabs[selectedIndex], spawnPoint.position, Quaternion.identity);
    }

    void ClearKnives()
    {
        GameObject[] knives = GameObject.FindGameObjectsWithTag("Knife");
        foreach (GameObject k in knives)
        {
            // Видаляємо лише ті ножі, що встромилися в колоду (мають parent)
            if (k.transform.parent != null)
            {
                Destroy(k);
            }
        }
    }

    void UpdateUI()
    {
        if (inGameScoreText != null)
            inGameScoreText.text = score.ToString();

        if (knivesText != null)
            knivesText.text = "Knives: " + knivesLeft;
    }

    public void GameOver()
    {
        // 1. Зберігаємо результат останньої гри
        PlayerPrefs.SetInt("LastScore", score);

        // 2. Оновлюємо рекорд
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        // 3. Накопичуємо загальну валюту (сума всіх ігор)
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += score;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);

        PlayerPrefs.Save();

        // Завантаження сцени програшу
        SceneManager.LoadScene("GameOver");
    }
}