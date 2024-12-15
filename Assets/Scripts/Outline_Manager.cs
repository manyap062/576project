using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Manager : MonoBehaviour
{
    public GameManager gameManager;
    public Camera mainCamera;
    public int lol;
    public Color highlightColor = Color.yellow; // Set the outline color
    public float outlineWidth = 5f; // Set the outline width
    public string highlightTag = "Guessable"; // Tag for highlightable objects

    private Outline lastHighlighted;

    void Update()
    {
        if (gameManager.isInPreviewPhase)
        {
            Debug.Log("Still in preview phase");
            return;
        }
        // Cast a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the object is highlightable
            if (gameManager.IsInteractable(hit.collider.gameObject)) // Tagging was used previously but not neccessary
            //hitObject.CompareTag(highlightTag) ||
            {
                bool isInteractable = gameManager.IsInteractable(hit.collider.gameObject);
                Outline outline = hitObject.GetComponent<Outline>();
                if (outline == null)
                {
                    // Add the Outline component dynamically if not already present
                    outline = hitObject.AddComponent<Outline>();
                    outline.OutlineColor = highlightColor;
                    outline.OutlineWidth = outlineWidth;
                }

                // Highlight the object
                if (lastHighlighted != outline)
                {
                    ClearLastHighlight();
                    outline.enabled = true;
                    lastHighlighted = outline;
                }
                if (Input.GetMouseButtonDown(0) && isInteractable)
                {
                    // Calls on the game manager for the clicking logic
                    gameManager.OnObjectClicked(hit.collider.gameObject);
                }
                return;
            }
        }

        // Clear highlight if no object is under the mouse
        ClearLastHighlight();
    }

    void ClearLastHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.enabled = false;
            lastHighlighted = null;
        }
    }
}
