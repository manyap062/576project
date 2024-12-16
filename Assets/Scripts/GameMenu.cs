using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSystem : MonoBehaviour
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
    [SerializeField] private PauseScreen pauseScreenCat;  // Reference to cat script

    [Header("Instructions References")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button exitInstructionsButton;
    [SerializeField] private TextMeshProUGUI instructionsText;
    
    [Header("Game References")]
    [SerializeField] private GameManager gameManager;
    
    private float previousTimeScale;
    private bool isPaused = false;

    void Start()
    {
        // initialize panels
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
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        
        // if opening menu, pause game
        if (menuPanel.activeSelf && !isPaused)
            PauseGame();
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
        
        // show cat hint when pausing
        if (pauseScreenCat != null)
            pauseScreenCat.ShowCatHint();
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
        gameManager.InitializeGame();  // Make sure this exists in GameManager
    }

    private void ShowInstructions()
    {
        menuPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    private void BackToMenu()
    {
        instructionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void SetupInstructionsText()
    {
        instructionsText.text = "add movement controls!!!!" + 
            "How to Play:\n\n" +
            "1. Preview Phase:\n" +
            $"   - You have {gameManager.previewDuration} seconds to memorize the room\n" +
            "   - Study all objects carefully\n\n" +
            "2. Find the Changes:\n" +
            $"   - You have {gameManager.gameplayDuration} seconds to spot {gameManager.numberOfChangesToMake} changes\n" +
            $"   - You have {gameManager.maxLives} lives\n" +
            "   - Click on objects you think have changed\n" +
            "   - Objects might change position, rotation, scale, material, or visibility\n" +
            "   - Pet Marty for hints, but be careful not to scare him! And be sure to avoid being caught by his owner!\n";
    }
}