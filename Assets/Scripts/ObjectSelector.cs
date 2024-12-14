using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameManager gameManager;
    public Material highlightMaterial; 
    public Camera mainCamera;

    private GameObject currentHighlightedObject;
    private Material originalMaterial;
    private Renderer currentRenderer;

    void Start()
    {
        //mainCamera = Camera.main;
    }

    void Update()
    {
        // Prevents highlights during preview phase
        if (gameManager.isInPreviewPhase)
        {
            Debug.Log("Still in preview phase");
            RemoveHighlight();
            return;
        }

        // Raycast from current mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // The cast hit something
        if (Physics.Raycast(ray, out hit))
        {
            // If hit doesn't involve the current highlighted object, remove the highlight
            if (currentHighlightedObject != hit.collider.gameObject) { RemoveHighlight(); }


            // Check if what was hit was an object that can be interacted with
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            bool isInteractable = gameManager.IsInteractable(hit.collider.gameObject);
            Debug.Log("Is interactable: " + isInteractable);

            // Check if the object can be interacted with & isn't the previously highlighted object
            if (isInteractable && (currentHighlightedObject != hit.collider.gameObject))
            {
                // Reassign the highlighted object to the currently hit one & update the renderer
                currentHighlightedObject = hit.collider.gameObject;
                currentRenderer = currentHighlightedObject.GetComponent<Renderer>();

                // Change the material of the object to be the highlight material
                if (currentRenderer != null)
                {
                    originalMaterial = currentRenderer.material;
                    currentRenderer.material = highlightMaterial;
                }
            }

            // Check if the user clicks on the object
            if (Input.GetMouseButtonDown(0) && isInteractable) 
            {
                // Calls on the game manager for the clicking logic
                gameManager.OnObjectClicked(hit.collider.gameObject);
            }
        }
        else
        {
            RemoveHighlight();
        }
    }

    void RemoveHighlight()
    {
        if (currentHighlightedObject != null && currentRenderer != null)
        {
            currentRenderer.material = originalMaterial;
        }
        currentHighlightedObject = null;
        currentRenderer = null;
    }

    void OnDisable()
    {
        RemoveHighlight(); 
    }
}