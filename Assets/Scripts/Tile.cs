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

    public int step; // Step number of the tile

    public int Step
    {
        get { return step; }
        set { step = value; }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found.");
        }

        defaultSprite = spriteRenderer.sprite; // Store default sprite
        currentSprite = defaultSprite; // Initialize current sprite
        startRotation = transform.rotation; // Store initial rotation
    }

    void Update()
    {
        if (isRotating)
        {
            RotateTile(); // Rotate the tile when isRotating is true
        }
    }

    void OnMouseDown()
    {
        if (!isRotating)
        {
            HandlePadInteraction(); // Handle interaction with Pad object
            HandleTileInteraction(); // Handle interaction with another Tile object
        }
    }

    private void RotateTile()
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
            currentSprite = newSprite; // Update current sprite reference
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
        isRotating = true; // Start rotating the tile
    }

    public void StopRotation()
    {
        isRotating = false; // Stop rotating the tile
        currentRotation = 0f; // Reset current rotation angle

        // Snap rotation to original rotation
        transform.rotation = startRotation;
    }

    private void HandlePadInteraction()
    {
        Pad selectedPad = BoardManager.Instance.SelectedPad;

        if (selectedPad != null)
        {
            this.SetSprite(selectedPad.GetCurrentSprite());
            BoardManager.Instance.SaveReplacedTileData(this, selectedPad.GetCurrentSprite(), Step); // Pass 'Step' parameter
            StartRotation();
        }
    }

    private void HandleTileInteraction()
    {
        Tile selectedTile = this; // Assuming this logic is correct for your game

        if (selectedTile != null && selectedTile != this)
        {
            // Check if the selected tile's sprite is different from this tile's current sprite
            if (selectedTile.GetSprite() != this.GetSprite())
            {
                // Replace the tile with the selected tile's sprite
                BoardManager.Instance.SaveReplacedTileData(selectedTile, selectedTile.GetSprite(), selectedTile.Step); // Pass 'Step' parameter
                this.SetSprite(selectedTile.GetSprite());
                StartRotation();

                // Retrieve and log the step variable of the clicked tile
                Debug.Log($"Clicked tile step: {selectedTile.Step}");
            }
        }
    }

    public void SetSpriteToDefault()
    {
        SetSprite(defaultSprite); // Set sprite to default sprite
    }
}
