using UnityEngine;

public class SpriteHighlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public Color highlightColor = Color.yellow; // Customize this in the Inspector for your outline color
    public float outlineThickness = 1f; // Customize this in the Inspector for the outline thickness

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
            return;
        }

        // Find and apply the custom outline shader
        Shader outlineShader = Shader.Find("Custom/SpriteOutline");
        if (outlineShader != null)
        {
            Material outlineMat = new Material(outlineShader);
            spriteRenderer.material = outlineMat;

            // Initialize with no outline
            spriteRenderer.material.SetFloat("_OutlineThickness", 0f);
        }
        else
        {
            Debug.LogError("Custom/SpriteOutline shader not found!");
        }
    }

    // Called when the mouse enters the collider
    void OnMouseEnter()
    {
        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            spriteRenderer.material.SetFloat("_OutlineThickness", outlineThickness);
            spriteRenderer.material.SetColor("_OutlineColor", highlightColor);
        }
    }

    // Called when the mouse exits the collider
    void OnMouseExit()
    {
        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            spriteRenderer.material.SetFloat("_OutlineThickness", 0f);
        }
    }
}