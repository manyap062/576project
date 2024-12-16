using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    
    private string[] catHints = new string[]
    {
        "Meow! Try creating a story with the objects you see!",
        "Focus on one section of the room at a time!",
        "Purr...Did you notice the colors and materials?",
        "Make a quick mental snapshot, don't overthink it!",
        "Practice makes purrfect- play again to sharpen up!",
        "Use your mental map! Where's the cool stuff?",
        "Stay sharp! No distractions, just focus on my loot!"
    };

    void Start()
    {
        ShowCatHint();
    }

    public void ShowCatHint()
    {
        string randomHint = catHints[Random.Range(0, catHints.Length)];
        hintText.text = randomHint;
    }
}