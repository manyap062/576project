using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string scene_to_load;
    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.name == "main_char") // Check object name
        {
            bool isLevelUnlocked = PlayerPrefs.GetInt(scene_to_load + "_unlocked", 0) == 1; 
            if (isLevelUnlocked)
            {
                Debug.Log($"Loading scene: {scene_to_load}");
                SceneManager.LoadScene(scene_to_load);
            }
            else
            {
                Debug.Log($"Level {scene_to_load} is locked.");
            }
        }
    }

}
