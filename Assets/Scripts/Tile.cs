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
            RotateTile(); // Rotate the tile when isRotating is true
        }
    }

    void OnMouseDown()
    {
        if (!isRotating)
        {
            // Get the selected pad from BoardManager
            Pad selectedPad = BoardManager.Instance.SelectedPad;

            if (selectedPad != null)
            {
                // Replace the tile sprite with the selected pad sprite
                SetSprite(selectedPad.GetCurrentSprite());

                // Start rotating the tile
                StartRotation();
            }

            Tile selectedTile = this; // Get the selected tile from BoardManager

            if (selectedTile != null)
            {
                // Save replaced tile data in BoardManager
                BoardManager.Instance.SaveReplacedTileData(this, selectedTile.GetSprite(), selectedTile.Step);

                // Replace the tile sprite with the selected tile sprite
                SetSprite(selectedTile.GetSprite());

                // Start rotating the tile
                StartRotation();

                // Retrieve and log the step variable of the clicked tile
                Debug.Log($"Clicked tile step: {Step}");
            }            
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
        isRotating = true; // Start rotating the tile
    }

    public void StopRotation()
    {
        isRotating = false; // Stop rotating the tile
        currentRotation = 0f; // Reset current rotation angle

        // Snap rotation to original rotation
        transform.rotation = startRotation;
    }

    private void SetSortingOrder(int order)
    {
        spriteRenderer.sortingOrder = order; // Set the sorting order of the spriteRenderer
    }
}
