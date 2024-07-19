using UnityEngine;

public class Pad : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Sprite defaultSprite; // Default sprite of the Pad
    private Sprite currentSprite; // Current sprite of the Pad

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        defaultSprite = spriteRenderer.sprite; // Store default sprite
        currentSprite = defaultSprite; // Initialize current sprite
    }

    void OnMouseDown()
    {
        // Store current sprite in BoardManager
        BoardManager.Instance.SetSelectedPad(this);
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite; // Set the sprite of the SpriteRenderer
            currentSprite = sprite; // Update current sprite
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found.");
        }
    }

    public Sprite GetCurrentSprite()
    {
        return currentSprite;
    }

    public void ResetSprite()
    {
        SetSprite(defaultSprite); // Reset sprite to default
    }
}
