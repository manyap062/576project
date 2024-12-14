using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject instructions;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button exitInstructionsButton;
    
    [Header("Instructions Text")]
    [SerializeField] private TextMeshProUGUI instructionsText;
    
    [Header("Game References")]
    [SerializeField] private GameManager gameManager;
    
    private float previousTimeScale;
    private bool wasInPreviewPhase;

    void Start()
    {
        menu.SetActive(false);
        instructions.SetActive(false);
        
        menuButton.onClick.AddListener(ToggleMenu);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        instructionsButton.onClick.AddListener(ShowInstructions);
        exitInstructionsButton.onClick.AddListener(BackToMenu);
        
        SetupInstructionsText();
    }

    private void SetupInstructionsText()
    {
        instructionsText.text = "How to Play:\n\n" +
            "!!!!insert movement controller info !!!!1. Preview Phase:\n" +
            $"   - You have {gameManager.previewDuration} seconds to memorize the room\n" +
            "   - Study all objects carefully\n\n" +
            "2. Find the Changes:\n" +
            $"   - You have {gameManager.gameplayDuration} seconds to spot {gameManager.numberOfChangesToMake} changes\n" +
            $"   - You have {gameManager.maxLives} lives\n" +
            "   - Click on objects you think have changed\n" +
            "   - Objects might change position, rotation, scale, material, or visibility\n" +
            "   - Pet Marty for hints, but be careful not to scare him! And be sure to avoid being caught by his owner!\n";
    }

    private void ToggleMenu()
    {
        if (menu.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        wasInPreviewPhase = gameManager.isInPreviewPhase;
        gameManager.isGameActive = false;
        menu.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = previousTimeScale;
        gameManager.isGameActive = true;
        menu.SetActive(false);
        instructions.SetActive(false);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        instructions.SetActive(false);
        //gameManager.InitializeGame();
    }

    private void ShowInstructions()
    {
        menu.SetActive(false);
        instructions.SetActive(true);
    }

    private void BackToMenu()
    {
        instructions.SetActive(false);
        menu.SetActive(true);
    }

    void Update()
    {
        // can exit using esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
}