using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade_GameM : MonoBehaviour
{
    [Header("Game Settings")]
    public float previewDuration = 60f;
    public float gameplayDuration = 120f;
    public int maxLives = 3;
    public int numberOfChangesToMake = 6;

    [Header("Objects That Can Change")]
    public List<RoomObject> roomObjects; //can do in inspector

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI livesText;
    public TMPro.TextMeshProUGUI phaseText;

    [Header("Audio Clips")]
    public AudioClip gameWinClip;
    public AudioClip guessCorrectClip;
    public AudioClip guessIncorrectClip;
    public AudioClip gameLoseClip;

    private AudioSource audioSource;

    public bool isInPreviewPhase = true;
    private int currentLives;
    private float currentTimer;
    private int changesFound = 0;
    private List<ObjectChange> activeChanges = new List<ObjectChange>();
    public bool isGameActive = false;
    public ScreenFader screenFader;

    private class ObjectChange
    {
        public GameObject changedObject;
        public string changeName;
        public object originalValue;
        public object newValue;
        public ChangeType changeType;
    }

    private enum ChangeType
    {
        Rotation,
        Scale,
        Material,
    }

    [System.Serializable]
    public class RoomObject
    {
        public GameObject gameObject;
        public bool canRotate;
        public bool canScale;
        public bool canChangeMaterial;

        public Vector3 maxRotation = new Vector3(90, 90, 90);
        public Vector3 maxScale = new Vector3(3, 3, 3);
        public Material[] possibleMaterials;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        InitializeGame();
    }

    void InitializeGame()
    {
        currentLives = maxLives;
        isInPreviewPhase = true;
        currentTimer = previewDuration;
        changesFound = 0;
        isGameActive = true;
        activeChanges.Clear();

        UpdateUI();
    }

    void Update()
    {
        if (!isGameActive) return;

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateUI();

            if (currentTimer <= 0)
            {
                currentTimer = 0;
                if (isInPreviewPhase)
                {
                    StartCoroutine(StartGameplayPhaseCoroutine());
                    isGameActive = false;
                }
                else
                {
                    EndGame(false);
                }
            }
        }
    }

    void UpdateUI()
    {
        if (timerText) timerText.text = $"Time: {Mathf.CeilToInt(currentTimer)}";
        if (livesText) livesText.text = $"Lives Left: {currentLives}";
        if (phaseText) phaseText.text = isInPreviewPhase ? "Memorize the Room!" : "Find the Changes!";
    }

    IEnumerator StartGameplayPhaseCoroutine()
    {
        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeToBlack());

        isInPreviewPhase = false;
        currentTimer = gameplayDuration;
        MakeRandomChanges();
        UpdateUI();
        isGameActive = true;

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeFromBlack());
    }


    void MakeRandomChanges()
    {
        List<RoomObject> availableObjects = new List<RoomObject>(roomObjects); //list of possible objects to change
        for (int i = 0; i < numberOfChangesToMake; i++)
        {

            if (availableObjects.Count == 0) break;

            int objectIndex = Random.Range(0, availableObjects.Count);
            RoomObject selectedObject = availableObjects[objectIndex];
            MakeRandomChange(selectedObject);

            availableObjects.RemoveAt(objectIndex); //remove changed object so doesn't change again
        }
    }

    void MakeRandomChange(RoomObject roomObject)
    {
        List<ChangeType> possibleChanges = new List<ChangeType>();

        if (roomObject.canRotate) possibleChanges.Add(ChangeType.Rotation);
        if (roomObject.canScale) possibleChanges.Add(ChangeType.Scale);
        if (roomObject.canChangeMaterial) possibleChanges.Add(ChangeType.Material);

        ChangeType changeType = possibleChanges[Random.Range(0, possibleChanges.Count)];
        Debug.Log(changeType);
        ObjectChange change = new ObjectChange();
        change.changedObject = roomObject.gameObject;
        change.changeType = changeType;

        //make change selected
        switch (changeType)
        {
            case ChangeType.Rotation:
                Vector3 newRotation = new Vector3(
                    Random.Range(-roomObject.maxRotation.x, roomObject.maxRotation.x),
                    Random.Range(-roomObject.maxRotation.y, roomObject.maxRotation.y),
                    Random.Range(-roomObject.maxRotation.z, roomObject.maxRotation.z)
                );
                change.originalValue = roomObject.gameObject.transform.rotation;
                change.newValue = Quaternion.Euler(newRotation);
                roomObject.gameObject.transform.rotation = Quaternion.Euler(newRotation);
                break;

            case ChangeType.Scale:
                Vector3 newScale = roomObject.gameObject.transform.localScale + new Vector3(
                    Random.Range(-roomObject.maxScale.x, roomObject.maxScale.x),
                    Random.Range(-roomObject.maxScale.y, roomObject.maxScale.y),
                    Random.Range(-roomObject.maxScale.z, roomObject.maxScale.z)
                );
                change.originalValue = roomObject.gameObject.transform.localScale;
                change.newValue = newScale;
                roomObject.gameObject.transform.localScale = newScale;
                break;

            case ChangeType.Material:
                if (roomObject.possibleMaterials.Length == 2) // Ensure exactly two variations are defined
                {
                    Renderer renderer = roomObject.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Use sharedMaterial to avoid instance-related issues
                        Material currentMaterial = renderer.sharedMaterial;
                        Debug.Log($"Current Material (Shared): {currentMaterial.name}"); // Log the current material name

                        change.originalValue = currentMaterial;

                        // Compare using references, not instances
                        Material newMaterial = roomObject.possibleMaterials[0] == currentMaterial
                            ? roomObject.possibleMaterials[1]
                            : roomObject.possibleMaterials[0];

                        Debug.Log($"Changed to New Material: {newMaterial.name}"); // Log the new material name

                        change.newValue = newMaterial;

                        // Apply the new material
                        renderer.material = newMaterial;
                    }
                }
                break;


        }

        activeChanges.Add(change);
    }

    public void OnObjectClicked(GameObject clickedObject)
    {
        if (isInPreviewPhase) return;
        ObjectChange change = activeChanges.Find(x => x.changedObject == clickedObject);

        if (change != null) //correct guess
        {

            switch (change.changeType)
            {
                case ChangeType.Rotation:
                    clickedObject.transform.rotation = (Quaternion)change.originalValue;
                    break;

                case ChangeType.Scale:
                    clickedObject.transform.localScale = (Vector3)change.originalValue;
                    break;

                case ChangeType.Material:
                    Renderer renderer = clickedObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = (Material)change.originalValue;
                    }
                    break;
            }
            changesFound++;
            activeChanges.Remove(change);

            if (changesFound >= numberOfChangesToMake)
            {
                EndGame(true);
            }
            else
            {
                PlaySound(guessCorrectClip); // Play correct guess sound only if game doesn't end
            }
        }
        else //wrong guess
        {
            currentLives--;

            if (currentLives <= 0)
            {
                EndGame(false);
            }
            else
            {
                PlaySound(guessIncorrectClip);
            }
        }

        UpdateUI();
    }

    void EndGame(bool won)
    {
        PlaySound(won ? gameWinClip : gameLoseClip);
        if (won)
            Debug.Log("You won!");
        else
            Debug.Log("Game Over!");
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool IsInteractable(GameObject obj)
    {
        foreach (var roomObj in roomObjects)
        {
            if (roomObj.gameObject == obj)
                return true;
        }
        return false;
    }
}