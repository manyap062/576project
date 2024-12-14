using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Manager : MonoBehaviour
{
    public Color highlightColor = Color.yellow; // Set the outline color
    public float outlineWidth = 5f; // Set the outline width
    public string highlightTag = "Guessable"; // Tag for highlightable objects

    private Outline lastHighlighted;

    void Update()
    {
        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the object is highlightable
            if (hitObject.CompareTag(highlightTag))
            {
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
