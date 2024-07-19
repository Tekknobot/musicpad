using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Sprite defaultSprite; // Default sprite of the Tile
    private Sprite currentSprite; // Current sprite of the Tile

    private bool isRotating = false; // Flag to control rotation
    private float rotationSpeed = 180f; // Rotation speed in degrees per second
    private float currentRotation = 0f; // Current rotation angle
    private Quaternion startRotation; // Initial rotation of the tile

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        defaultSprite = spriteRenderer.sprite; // Store default sprite
        currentSprite = defaultSprite; // Initialize current sprite
        startRotation = transform.rotation; // Store initial rotation
    }

    void Update()
    {
        if (isRotating)
        {
            RotateSprite(); // Rotate the sprite when isRotating is true
        }
    }

    void OnMouseDown()
    {
        // Toggle between default sprite and selected Pad's sprite
        if (currentSprite == defaultSprite)
        {
            // Copy sprite from selected Pad to this Tile
            if (BoardManager.Instance != null && BoardManager.Instance.SelectedPad != null)
            {
                Sprite padSprite = BoardManager.Instance.SelectedPad.GetCurrentSprite();
                SetSprite(padSprite);

                // Ensure tile is on top layer
                SetSortingOrder(1);

                // Start rotating the tile sprite
                StartRotation();
            }
        }
        else
        {
            // Revert to default sprite
            SetSprite(defaultSprite);
            StopRotation();
        }
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

    public Sprite GetSprite()
    {
        return currentSprite; // Return current sprite
    }

    public void ResetSprite()
    {
        SetSprite(defaultSprite); // Reset sprite to default
    }

    public void StartRotation()
    {
        isRotating = true; // Start rotating the sprite
        SetSortingOrder(2); // Ensure tile is on top layer when rotating
    }

    public void StopRotation()
    {
        isRotating = false; // Stop rotating the sprite
        currentRotation = 0f; // Reset current rotation angle
        SetSortingOrder(0); // Reset sorting order to default when rotation stops

        // Snap rotation to original rotation
        transform.rotation = startRotation;
    }

    private void RotateSprite()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime; // Calculate rotation amount
        currentRotation += rotationAmount; // Update current rotation angle

        if (currentRotation >= 180f)
        {
            currentRotation = 180f; // Clamp rotation angle to 180 degrees
            StopRotation(); // Stop rotation when 180 degrees is reached
        }

        float normalizedRotation = currentRotation / 180f; // Normalize rotation amount
        transform.rotation = Quaternion.Lerp(startRotation, startRotation * Quaternion.Euler(0f, 0f, 180f), normalizedRotation);
    }

    private void SetSortingOrder(int order)
    {
        spriteRenderer.sortingOrder = order; // Set the sorting order of the spriteRenderer
    }
}
