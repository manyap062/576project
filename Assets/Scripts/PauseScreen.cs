using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [Header("Box Sprites References")]
    [SerializeField] private GameObject boxWithoutCatObject;  
    [SerializeField] private GameObject boxWithCatObject;     
    
    [Header("Hint Text")]
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

    private void Start()
    {
        
        ShowEmptyBox(); 
    }

    public void ShowCatHint()
    {
        ShowBoxWithCat();

        //random hint text
        string randomHint = catHints[Random.Range(0, catHints.Length)];
        hintText.text = randomHint;

        //switch back to empty box after 5 seconds
        Invoke("ShowEmptyBox", 5f);
    }

    private void ShowEmptyBox()
    {
        Debug.Log("Showing Empty Box");
        boxWithoutCatObject.SetActive(true);
        boxWithCatObject.SetActive(false);
    }

    private void ShowBoxWithCat()
    {
        Debug.Log("Showing Cat Box");
        boxWithoutCatObject.SetActive(false);
        boxWithCatObject.SetActive(true);
    }
}
