using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSystem_Arcade : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button restartButton;

    [Header("Pause References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    [Header("Instructions References")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button exitInstructionsButton;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [Header("Game References")]
    [SerializeField] private Arcade_GameM gameManager;

    private float previousTimeScale;
    private bool isPaused = false;

    void Start()
    {
        // Initialize panels
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        instructionsPanel.SetActive(false);

        menuButton.onClick.AddListener(ToggleMenu);
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeGame);
        instructionsButton.onClick.AddListener(ShowInstructions);
        restartButton.onClick.AddListener(RestartGame);
        exitInstructionsButton.onClick.AddListener(BackToMenu);

        SetupInstructionsText();
        gameManager.InitializeGame();
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        instructionsPanel.SetActive(false);

        // Pause game when opening menu
        if (menuPanel.activeSelf && !isPaused)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            isPaused = true;
            gameManager.isGameActive = false;
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        gameManager.isGameActive = false;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = previousTimeScale;
        gameManager.isGameActive = true;
        isPaused = false;
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        instructionsPanel.SetActive(false);
        gameManager.RestartLevel();  // use the RestartLevel method from Arcade_GameM
    }

    private void ShowInstructions()
    {
        menuPanel.SetActive(false);
        instructionsPanel.SetActive(true);

        // pause game when opening instructions
        if (instructionsPanel.activeSelf && !isPaused)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            isPaused = true;
            gameManager.isGameActive = false;
        }
    }

    private void BackToMenu()
    {
        instructionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void SetupInstructionsText()
    {
        instructionsText.text = "Welcome to the Arcade Game!" + 
            "\n\nHow to Play:\n" +
            "1. Preview Phase:\n" +
            $"   - You have {gameManager.previewDuration} seconds to memorize the room\n" +
            "2. Gameplay Phase:\n" +
            $"   - Spot {gameManager.numberOfChangesToMake} changes within {gameManager.gameplayDuration} seconds\n" +
            $"   - You have {gameManager.maxLives} lives. Good luck!";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
                TogglePause();
            else if (menuPanel.activeSelf)
                ToggleMenu();
            else
                TogglePause();
        }
    }
}

