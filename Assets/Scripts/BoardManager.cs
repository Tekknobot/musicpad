using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton instance

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Sprite _defaultSprite; // Default sprite for tiles

    private Dictionary<Vector2, Tile> _tiles; // Dictionary to store Tiles
    private Pad selectedPad; // Store the selected Pad

    public Pad SelectedPad
    {
        get { return selectedPad; }
        set { selectedPad = value; }
    }

    void Awake()
    {
        Instance = this; // Initialize singleton instance
        _tiles = new Dictionary<Vector2, Tile>(); // Initialize _tiles dictionary
    }

    void Start()
    {
        GenerateGrid();

        // Example: Simulate saved tiles
        // In your actual implementation, this logic should come from your save/load system
        if (HasSavedTiles())
        {
            DisplaySavedTiles();
        }
        else
        {
            DisplayDefaultBoard();
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Instantiate tiles
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x},{y})";
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public void DisplayDefaultBoard()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.SetSprite(_defaultSprite);
        }
    }

    public void DisplaySavedTiles()
    {
        // Logic to display saved tiles goes here
        foreach (var tile in _tiles.Values)
        {
            // Example: Set all tiles to a specific sprite for demonstration
            Sprite savedSprite = Resources.Load<Sprite>("SavedSprite"); // Replace with your saved sprite logic
            tile.SetSprite(savedSprite);
        }
    }

    public bool HasSavedTiles()
    {
        // Example: Check if there are saved tiles (replace with actual save/load logic)
        return false; // Modify this condition based on your actual game's logic
    }

    public void DisplaySavedOrDefaultBoard()
    {
        if (HasSavedTiles())
        {
            DisplaySavedTiles();
        }
        else
        {
            DisplayDefaultBoard();
        }
    }
}
