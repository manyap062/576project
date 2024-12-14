using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlight_system : MonoBehaviour
{
    private Outline outline;

    void Start()
    {
        // Get the Outline component
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Disable outline by default
        }
    }

    void OnMouseEnter()
    {
        if (outline != null)
        {
            outline.enabled = true; // Enable outline on hover
        }
    }

    void OnMouseExit()
    {
        if (outline != null)
        {
            outline.enabled = false; // Disable outline when not hovering
        }
    }
}
