using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public Button continueButton;
    public Button menuButton;

    [Header("Win/Lose Menu")]
    public GameObject winPanel;
    public GameObject losePanel;
    public Button winMenuButton;
    public Button loseMenuButton;
    public TMP_Text winLoseText;

    private bool isPaused = false;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();

        // Настройка кнопок паузы
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);

        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMainMenu);

        // Настройка кнопок победы/поражения
        if (winMenuButton != null)
            winMenuButton.onClick.AddListener(GoToMainMenu);

        if (loseMenuButton != null)
            loseMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    // Обработчик паузы (вызывается через Input System)
    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started) // Только при нажатии, а не при отпускании
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Останавливаем время
        pauseMenuPanel.SetActive(true); // Показываем меню паузы

        // Дополнительно: отключаем управление игроком
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Возобновляем время
        pauseMenuPanel.SetActive(false); // Скрываем меню паузы

        // Включаем управление обратно
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = true;
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // На всякий случай восстанавливаем время
        SceneManager.LoadScene("MainMenu"); // Замените на имя вашей сцены меню
    }

    // Оригинальные методы для UI здоровья
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }

    public void ShowWinPanel()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        winLoseText.text = "Победа!";
        DisablePlayerInput();
    }

    public void ShowLosePanel()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        winLoseText.text = "Поражение";
        DisablePlayerInput();
    }

    private void DisablePlayerInput()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }
}