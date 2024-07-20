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

    private int step; // Step number of the tile

    public int Step
    {
        get { return step; }
        set { step = value; }
    }

    private float clickTimeThreshold = 1f; // Time threshold to detect long press (1 second)
    private float clickStartTime = 0f; // Time when mouse button was pressed

    private Color defaultColor; // Default color of the tile sprite
    private Color highlightColor = Color.yellow; // Highlight color (adjust as needed)

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found.");
        }

        defaultSprite = spriteRenderer.sprite; // Store default sprite
        currentSprite = defaultSprite; // Initialize current sprite reference
        startRotation = transform.rotation; // Store initial rotation

        defaultColor = spriteRenderer.color; // Store default color of the sprite
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
            clickStartTime = Time.time; // Record the time when mouse button is pressed
            HandlePadInteraction(); // Handle interaction with Pad object
            HandleTileInteraction(); // Handle interaction with another Tile object
        }
    }

    void OnMouseUp()
    {
        float clickDuration = Time.time - clickStartTime;

        if (clickDuration >= clickTimeThreshold)
        {
            ResetSpriteToDefault(); // If mouse press duration is >= 1 second, reset sprite to default and clear data
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
        // Iterate through all tiles on the board
        foreach (var kvp in BoardManager.Instance.GetTiles())
        {
            Tile otherTile = kvp.Value; // Get the Tile object from the KeyValuePair

            if (otherTile != this)
            {
                Pad selectedPad = BoardManager.Instance.SelectedPad;

                if (selectedPad != null)
                {
                    Sprite padSprite = selectedPad.GetCurrentSprite();
                    Sprite otherTileSprite = otherTile.GetSprite();

                    // Check if the pad sprite is the same as the other tile's current sprite
                    if (padSprite == otherTileSprite)
                    {
                        // Replace the other tile sprite with the default sprite
                        otherTile.SetSprite(BoardManager.Instance.DefaultTileBoardSprite);
                        otherTile.StartRotation();

                        // Log that the other tile sprite was replaced with default
                        Debug.Log($"Replaced other tile with default sprite. Other tile step: {otherTile.Step}");
                    }
                }
            }
        }
    }

    public void SetSpriteToDefault()
    {
        SetSprite(defaultSprite); // Set sprite to default sprite
    }

    private void ResetSpriteToDefault()
    {
        // Reset sprite to default sprite
        SetSprite(defaultSprite);
        Debug.Log("Tile sprite reset to default sprite.");

        // Clear saved data associated with this tile step
        BoardManager.Instance.ClearTileDataForStep(Step);
    }

    public void Highlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor; // Change sprite color to highlight color
        }
    }

    public void Unhighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor; // Revert sprite color to default color
        }
    }

    // Trigger SaveReplacedTileData from external scripts or events
    public void TriggerSaveReplacedTileData(Sprite newSprite)
    {
        BoardManager.Instance.SaveReplacedTileData(this, newSprite, Step);
    }
}
