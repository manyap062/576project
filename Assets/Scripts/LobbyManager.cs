using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelDoor
    {
        public string levelName; // Name of the level
        public Light doorLight; // Light component on the door
    }

    public LevelDoor[] levelDoors; // Array of all level doors
    public LevelDoor finalDoor; // Special door for the final level

    public Color unlockedColor = Color.green; // Color for unlocked levels
    public Color lockedColor = Color.red; // Color for locked levels
    public Color finalDoorUnlockedColor = Color.yellow; // Unique color for the final door
    public string firstLevelName; // Name of the first level to auto-complete
    private static bool hasGameStarted = false; // Tracks if the game has already started




    void Awake()
    {
        if (!hasGameStarted)
        {
            // Reset PlayerPrefs on the first play session
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("PlayerPrefs cleared. Starting fresh!");
            hasGameStarted = true; // Prevent further resets during the same play session
        }
    }

    void Start()
    {
        InitializeFirstLevel();

        bool unlockNextLevel = false;

        // Loop through each door and set the light color based on level status
        foreach (LevelDoor levelDoor in levelDoors)
        {

            string levelName = levelDoor.levelName;
            bool isUnlocked = PlayerPrefs.GetInt(levelName + "_unlocked", 0) == 1;
            bool isCompleted = PlayerPrefs.GetInt(levelName + "_completed", 0) == 1;

            if (isUnlocked)
            {
                // Update the light to green (unlocked)
                levelDoor.doorLight.color = unlockedColor;

                // If the level is completed, allow the next level to be unlocked
                unlockNextLevel = isCompleted;
            }
            else if (unlockNextLevel)
            {
                // Unlock this level and update the light to green
                PlayerPrefs.SetInt(levelName + "_unlocked", 1);
                PlayerPrefs.Save();

                levelDoor.doorLight.color = unlockedColor;

                // Stop unlocking further levels in this loop
                unlockNextLevel = false;
            }
            else
            {
                // Lock this level and update the light to red
                levelDoor.doorLight.color = lockedColor;
            }
        }

        bool allLevelsCompleted = AreAllLevelsCompleted();

        PlayerPrefs.SetInt("Treasure Room_unlocked", allLevelsCompleted ? 1 : 0);
        PlayerPrefs.Save();

        //Turn on light if all levels completed
        if (allLevelsCompleted)
        {
            // Unlock final door and set light color
            finalDoor.doorLight.color = finalDoorUnlockedColor;
            finalDoor.doorLight.enabled = true; // Enable the light for the final door
        }
        else
        {
            // Lock final door and disable its light
            finalDoor.doorLight.enabled = false; // Final door light is off when locked
        }
    }

    private void InitializeFirstLevel()
    {
        // Ensure the first level is always unlocked but not completed
        if (PlayerPrefs.GetInt(firstLevelName + "_unlocked", 0) == 0)
        {
            PlayerPrefs.SetInt(firstLevelName + "_unlocked", 1); // Unlock Level 1
            PlayerPrefs.SetInt(firstLevelName + "_completed", 0); // Ensure it's not marked as completed
            PlayerPrefs.Save();
        }
    }

    bool AreAllLevelsCompleted()
    {
        // Check if every level in levelDoors is marked as completed in PlayerPrefs
        foreach (LevelDoor levelDoor in levelDoors)
        {
            if (PlayerPrefs.GetInt(levelDoor.levelName + "_completed", 0) != 1)
            {
                return false; // If any level is not completed, return false
            }
        }
        Debug.Log("All Levels Completed");
        return true; // All levels are completed
    }
}
