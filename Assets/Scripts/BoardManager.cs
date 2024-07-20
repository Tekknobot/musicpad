using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton instance

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Sprite _defaultSprite; // Default sprite for tiles

    private Dictionary<int, Tile> _tiles; // Dictionary to store Tiles
    private List<ReplacedTileData> _replacedTilesData; // List to store replaced tile data

    private Pad selectedPad; // Store the selected Pad

    public Pad SelectedPad
    {
        get { return selectedPad; }
        set { selectedPad = value; }
    }

    public int _stepCount; // Step counter

    void Awake()
    {
        Instance = this; // Initialize singleton instance
        _tiles = new Dictionary<int, Tile>(); // Initialize _tiles dictionary
        _replacedTilesData = new List<ReplacedTileData>(); // Initialize replaced tiles data list
    }

    void Start()
    {
        GenerateGrid();

        // Example: Simulate saved tiles
        // In your actual implementation, this logic should come from your save/load system
        if (HasSavedTiles())
        {
            DisplaySavedOrDefaultBoard();
        }
        else
        {
            DisplayDefaultBoard();
        }
    }

    void GenerateGrid()
    {
        int step = 0; // Reset step counter

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Instantiate tiles
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(y, x), Quaternion.identity);
                spawnedTile.name = $"Tile ({x},{y})";
                spawnedTile.step = _stepCount++;
                Debug.Log(_stepCount);
                _tiles[step++] = spawnedTile;

                // Example: Save initial tile data
                SaveTileData(spawnedTile, spawnedTile.GetSprite(), spawnedTile.Step);
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

    public void DisplaySavedOrDefaultBoard()
    {
        if (HasSavedTiles())
        {
            DisplaySavedTilesForPad(selectedPad.GetCurrentSprite());
        }
        else
        {
            DisplayDefaultBoard();
        }
    }

    public void DisplaySavedTilesForPad(Sprite padSprite)
    {
        ClearTiles(); // Clear current tile sprites

        // Iterate through each tile on the board
        foreach (var boardTile in _tiles.Values)
        {
            bool tileFound = false;

            // Check each saved tile data to find a match with the current board tile's step
            foreach (var replacedTileData in _replacedTilesData)
            {
                if (replacedTileData.NewSprite == padSprite && boardTile.Step == replacedTileData.NewStep)
                {
                    boardTile.SetSprite(replacedTileData.NewSprite);
                    tileFound = true;
                    break; // Exit the loop once a match is found
                }
            }

            // If no matching saved tile was found, reset the board tile to the default sprite
            if (!tileFound)
            {
                boardTile.SetSprite(_defaultSprite);
            }
        }
    }

    private void ClearTiles()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.SetSprite(_defaultSprite); // Reset all tiles to default sprite
        }
    }

    public bool HasSavedTiles()
    {
        return _replacedTilesData.Count > 0; // Check if there are saved tiles in replacedTilesData list
    }

    public void SaveReplacedTileData(Tile tile, Sprite newSprite, int newStep)
    {
        // Save replaced tile data into the list
        ReplacedTileData replacedTileData = new ReplacedTileData(tile, newSprite, newStep);
        _replacedTilesData.Add(replacedTileData);
    }

    private void SaveTileData(Tile tile, Sprite sprite, int step)
    {
        // Save tile data into the list or array
        // Here you can choose to save it in _replacedTilesData or another appropriate structure
        // For simplicity, I'll demonstrate saving in _replacedTilesData
        ReplacedTileData tileData = new ReplacedTileData(tile, sprite, step);
        _replacedTilesData.Add(tileData);
    }

    // Class to hold replaced tile data
    private class ReplacedTileData
    {
        public Tile Tile;
        public Sprite NewSprite;
        public int NewStep;

        public ReplacedTileData(Tile tile, Sprite newSprite, int newStep)
        {
            Tile = tile;
            NewSprite = newSprite;
            NewStep = newStep;
        }
    }
}
