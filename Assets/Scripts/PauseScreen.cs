using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Sprite boxWithoutCat;  // first sprite
    [SerializeField] private Sprite boxWithCat;     // second sprite
    
    private Image boxImage;

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

    private void Start()
    {
        boxImage = GetComponent<Image>();
        boxImage.sprite = boxWithoutCat;
    }

    public void ShowCatHint()
    {
        boxImage.sprite = boxWithCat;

        // random hint
        hintText.text = catHints[Random.Range(0, catHints.Length)];

        // switches back and forth
        Invoke("HideCat", 5f); 
    }

    private void HideCat()
    {
        boxImage.sprite = boxWithoutCat;
    }
}
