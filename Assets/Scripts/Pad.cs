using UnityEngine;
using System.Collections;

public class Pad : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Sprite defaultSprite; // Default sprite of the Pad
    private Sprite currentSprite; // Current sprite of the Pad

    private Vector3 originalScale; // Store the original scale of the pad
    private float scaleFactor = 1.2f; // Scale factor when clicked

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        defaultSprite = spriteRenderer.sprite; // Store default sprite
        currentSprite = defaultSprite; // Initialize current sprite

        originalScale = transform.localScale; // Store original scale
    }

    void OnMouseDown()
    {
        // Scale up the pad
        StartCoroutine(ScalePad());

        // Set this pad as the selected pad in BoardManager
        BoardManager.Instance.SelectedPad = this;

        // Display the board based on saved tiles or default
        BoardManager.Instance.DisplaySavedOrDefaultBoard();
    }

    IEnumerator ScalePad()
    {
        Vector3 targetScale = originalScale * scaleFactor;
        float duration = 0.1f;
        float elapsed = 0f;

        // Scale up
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Ensure scale is exactly the target scale

        // Scale down
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // Ensure scale is exactly the original scale
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
