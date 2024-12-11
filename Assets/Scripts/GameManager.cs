using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;

public class GameManager : MonoBehaviour
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

    private bool isInPreviewPhase = true;
    private int currentLives;
    private float currentTimer;
    private int changesFound = 0;
    private List<ObjectChange> activeChanges = new List<ObjectChange>(); 
    private bool isGameActive = false;

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
        Position,
        Rotation,
        Scale,
        //Color,
        Material,
        Visibility,
        //Amount
    }

    [System.Serializable]
    public class RoomObject
    {
        public GameObject gameObject;  
        public bool canMove;          
        public bool canRotate;        
        public bool canScale;         
        //public bool canChangeColor;   
        public bool canChangeMaterial;
        public bool canHide;          
        //public bool canDuplicate;
        
        public Vector3 maxMoveDistance = new Vector3(5,5,5);  
        public Vector3 maxRotation = new Vector3(90,90,90);   
        public Vector3 maxScale = new Vector3(3,3,3);            
        public Material[] possibleMaterials;                  
        //public Color[] possibleColors;                       
    }

    void Start()
    {
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
        if(!isGameActive) return;

        currentTimer -= Time.deltaTime;
        UpdateUI();

        if (currentTimer <= 0)
        {
            if (isInPreviewPhase)
            {
                StartGameplayPhase();
            }
            else
            {
                EndGame(false);
            }
        }
    }

    void UpdateUI()
    {
        if (timerText) timerText.text = $"Time: {Mathf.CeilToInt(currentTimer)}";
        if (livesText) livesText.text = $"Lives Left: {currentLives}";
        if (phaseText) phaseText.text = isInPreviewPhase ? "Memorize the Room!" : "Find the Changes!";
    }

    void StartGameplayPhase()
    {
        isInPreviewPhase = false;
        currentTimer = gameplayDuration;
        MakeRandomChanges();
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
        
        if (roomObject.canMove) possibleChanges.Add(ChangeType.Position);
        if (roomObject.canRotate) possibleChanges.Add(ChangeType.Rotation);
        if (roomObject.canScale) possibleChanges.Add(ChangeType.Scale);
        //if (roomObject.canChangeColor) possibleChanges.Add(ChangeType.Color);
        if (roomObject.canChangeMaterial) possibleChanges.Add(ChangeType.Material);
        if (roomObject.canHide) possibleChanges.Add(ChangeType.Visibility);

        ChangeType changeType = possibleChanges[Random.Range(0, possibleChanges.Count)];
        
        ObjectChange change = new ObjectChange();
        change.changedObject = roomObject.gameObject;
        change.changeType = changeType;

        //make change selected
        switch (changeType)
        {
            case ChangeType.Position:
                Vector3 newPos = roomObject.gameObject.transform.position + new Vector3(
                    Random.Range(-roomObject.maxMoveDistance.x, roomObject.maxMoveDistance.x),
                    Random.Range(-roomObject.maxMoveDistance.y, roomObject.maxMoveDistance.y),
                    Random.Range(-roomObject.maxMoveDistance.z, roomObject.maxMoveDistance.z)
                );
                change.originalValue = roomObject.gameObject.transform.position;
                change.newValue = newPos;
                roomObject.gameObject.transform.position = newPos;
                break;

            // case ChangeType.Color:
            //     if (roomObject.possibleColors.Length > 0)
            //     {
            //         Renderer renderer = roomObject.gameObject.GetComponent<Renderer>();
            //         if (renderer != null)
            //         {
            //             change.originalValue = renderer.material.color;
            //             Color newColor = roomObject.possibleColors[Random.Range(0, roomObject.possibleColors.Length)];
            //             change.newValue = newColor;
            //             renderer.material.color = newColor;
            //         }
            //     }
            //     break;

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
            
            case ChangeType.Visibility:
                change.originalValue = roomObject.gameObject.activeSelf;
                change.newValue = false;
                roomObject.gameObject.SetActive(false);
                break;

            case ChangeType.Material:
                if (roomObject.possibleMaterials.Length > 0)
                {
                    Renderer renderer = roomObject.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        change.originalValue = renderer.material;
                        Material newMaterial = roomObject.possibleMaterials[Random.Range(0, roomObject.possibleMaterials.Length)];
                        change.newValue = newMaterial;
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
            changesFound++;
            activeChanges.Remove(change);
            
            // switch (change.changeType)
            // {
            //     case ChangeType.Position:
            //         clickedObject.transform.position = (Vector3)change.originalValue;
            //         break;

            //     case ChangeType.Color:
            //         Renderer renderer = clickedObject.GetComponent<Renderer>();
            //         if (renderer != null)
            //             renderer.material.color = (Color)change.originalValue;
            //         break;

            //     case ChangeType.Visibility:
            //         clickedObject.SetActive((bool)change.originalValue);
            //         break;

            //     case ChangeType.Material:
            //         if (roomObject.possibleMaterials.Length > 0)
            //         {
            //             Renderer renderer = roomObject.gameObject.GetComponent<Renderer>();
            //             if (renderer != null)
            //             {
            //                 change.originalValue = renderer.material;
            //                 Material newMaterial = roomObject.possibleMaterials[Random.Range(0, roomObject.possibleMaterials.Length)];
            //                 change.newValue = newMaterial;
            //                 renderer.material = newMaterial;
            //             }
            //         }
            //         break;

            //     case ChangeType.Visibility:
            //         change.originalValue = roomObject.gameObject.activeSelf;
            //         change.newValue = false;
            //         roomObject.gameObject.SetActive(false);
            //         break;
            //     }

            if (changesFound >= numberOfChangesToMake)
            {
                EndGame(true);
            }
        }
        else //wrong guess
        {
            currentLives--;
            if (currentLives <= 0)
            {
                EndGame(false);
            }
        }
        
        UpdateUI();
    }

    void EndGame(bool won)
    {
        if (won)
            Debug.Log("You won!");
        else
            Debug.Log("Game Over!");
    }
}