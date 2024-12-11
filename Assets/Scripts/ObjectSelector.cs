using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameManager gameManager;
    public Material highlightMaterial; 
    
    private Camera mainCamera;
    private GameObject currentHighlightedObject;
    private Material originalMaterial;
    private Renderer currentRenderer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (gameManager.isInPreviewPhase)
        {
            Debug.Log("Still in preview phase");
            RemoveHighlight();
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            bool isInteractable = gameManager.IsInteractable(hit.collider.gameObject);
            Debug.Log("Is interactable: " + isInteractable);

            if (isInteractable && currentHighlightedObject != hit.collider.gameObject)
            {
                RemoveHighlight();

                currentHighlightedObject = hit.collider.gameObject;
                currentRenderer = currentHighlightedObject.GetComponent<Renderer>();
                if (currentRenderer != null)
                {
                    originalMaterial = currentRenderer.material;
                    currentRenderer.material = highlightMaterial;
                }
            }

            if (Input.GetMouseButtonDown(0) && isInteractable) 
            {
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