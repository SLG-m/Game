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

        // ��������� ������ �����
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);

        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMainMenu);

        // ��������� ������ ������/���������
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

    // ���������� ����� (���������� ����� Input System)
    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started) // ������ ��� �������, � �� ��� ����������
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
        Time.timeScale = 0f; // ������������� �����
        pauseMenuPanel.SetActive(true); // ���������� ���� �����

        // �������������: ��������� ���������� �������
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // ������������ �����
        pauseMenuPanel.SetActive(false); // �������� ���� �����

        // �������� ���������� �������
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = true;
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // �� ������ ������ ��������������� �����
        SceneManager.LoadScene("MainMenu"); // �������� �� ��� ����� ����� ����
    }

    // ������������ ������ ��� UI ��������
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
        winLoseText.text = "������!";
        DisablePlayerInput();
    }

    public void ShowLosePanel()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        winLoseText.text = "���������";
        DisablePlayerInput();
    }

    private void DisablePlayerInput()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }
}