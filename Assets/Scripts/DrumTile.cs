using UnityEngine;

public class DrumTile : MonoBehaviour
{
    private bool isRotating = false; // Flag to control rotation
    private float rotationSpeed = 180f; // Rotation speed in degrees per second
    private float currentRotation = 0f; // Current rotation angle
    private Quaternion startRotation; // Initial rotation of the drum tile

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Sprite defaultSprite; // Default sprite of the drum tile

    private Color defaultColor; // Default color of the drum tile sprite
    private Color highlightColor = Color.yellow; // Highlight color (adjust as needed)

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        defaultSprite = spriteRenderer.sprite; // Store default sprite
        startRotation = transform.rotation; // Store initial rotation

        defaultColor = spriteRenderer.color; // Store default color of the sprite
    }

    void Update()
    {
        if (isRotating)
        {
            RotateDrumTile(); // Rotate the drum tile when isRotating is true
        }
    }

    void OnMouseDown()
    {
        if (!isRotating)
        {
            HandlePadInteraction(); // Handle interaction with DrumPad object
            HandleTileInteraction(); // Handle interaction with another DrumTile object
        }
    }

    private void RotateDrumTile()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime; // Calculate rotation amount
        currentRotation += rotationAmount; // Update current rotation angle

        if (currentRotation >= 180f)
        {
            currentRotation = 180f; // Clamp rotation angle to 180 degrees
            StopRotation(); // Stop rotating when 180 degrees is reached
        }

        float normalizedRotation = currentRotation / 180f; // Normalize rotation amount
        transform.rotation = Quaternion.Lerp(startRotation, startRotation * Quaternion.Euler(180f, 180f, 0f), normalizedRotation);

        if (!isRotating)
        {
            // Snap rotation to original rotation
            transform.rotation = startRotation;
        }
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite; // Update the sprite of the SpriteRenderer
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found.");
        }
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite; // Return current sprite
    }

    public void ResetSprite()
    {
        spriteRenderer.sprite = defaultSprite; // Reset sprite to default
    }

    public void StartRotation()
    {
        isRotating = true; // Start rotating the drum tile
    }

    public void StopRotation()
    {
        isRotating = false; // Stop rotating the drum tile
        currentRotation = 0f; // Reset current rotation angle

        // Snap rotation to original rotation
        transform.rotation = startRotation;
    }

    private void HandlePadInteraction()
    {
        DrumPad selectedPad = DrumBoardManager.Instance.SelectedPad;

        if (selectedPad != null)
        {
            this.SetSprite(selectedPad.GetComponent<SpriteRenderer>().sprite);
            StartRotation();

            // Log interaction with drum pad
            Debug.Log($"Drum Tile {gameObject.name} interacted with Drum Pad {selectedPad.gameObject.name}");
        }
    }

    private void HandleTileInteraction()
    {
        // Example interaction logic between drum tiles
        // This can be expanded based on specific requirements
        Debug.Log($"Drum Tile {gameObject.name} interacted with another Drum Tile.");

        // Example: Reset sprite and stop rotation of the interacting drum tile
        ResetSprite();
        StopRotation();
    }
}
