using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton instance

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Sprite _defaultSprite; // Default sprite for tiles
    [SerializeField] private Sprite _defaultTileBoardSprite; // Default tile board sprite

    private Dictionary<int, Tile> _tiles; // Dictionary to store Tiles
    private List<ReplacedTileData> _replacedTilesData; // List to store replaced tile data

    private Pad selectedPad; // Store the selected Pad

    public Pad SelectedPad
    {
        get { return selectedPad; }
        set { selectedPad = value; }
    }

    public int _stepCount = -1; // Step counter

    public Sprite DefaultTileBoardSprite
    {
        get { return _defaultTileBoardSprite; }
    }

    private int _midiNoteStart = 48; // MIDI note number to start with

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
                spawnedTile.Step = _stepCount++;
                _tiles[step++] = spawnedTile;

                // Save initial tile data
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
                    break; // Exit loop once a match is found
                }
            }

            if (!tileFound)
            {
                boardTile.SetSpriteToDefault(); // Set the board tile sprite to default if no match is found
            }
        }
    }

    public void ClearTiles()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.ResetSprite(); // Reset each tile's sprite to default
        }
    }

    public bool HasSavedTiles()
    {
        return _replacedTilesData.Count > 0; // Return true if there are saved tiles data
    }

    public void SaveReplacedTileData(Tile tile, Sprite newSprite, int newStep)
    {
        // Create a new ReplacedTileData object and add it to the replaced tiles data list
        _replacedTilesData.Add(new ReplacedTileData(tile.GetSprite(), newSprite, tile.Step, newStep));

        // Example: Trigger MIDI note when a tile's sprite is replaced
        if (newSprite != _defaultSprite)
        {
            int midiNote = GetMIDINoteForTileStep(tile.Step);
            GameObject.Find("DrumSequencer").GetComponent<SampleSequencer>().AddNote(midiNote, tile.Step, tile.Step + 1);
        }
    }

    public int GetMIDINoteForTileStep(int step)
    {
        return _midiNoteStart + step;
    }

    public void ClearTileDataForStep(int step)
    {
        // Remove replaced tile data associated with the specified step
        _replacedTilesData.RemoveAll(data => data.OldStep == step);
    }

    public Dictionary<int, Tile> GetTiles()
    {
        return _tiles; // Return the _tiles dictionary
    }

    public Sprite GetDefaultTileBoardSprite()
    {
        return _defaultTileBoardSprite; // Return the default tile board sprite
    }

    private void SaveTileData(Tile tile, Sprite sprite, int step)
    {
        // Example: You can implement your own logic to save tile data here
        // For demonstration, I'm just printing the data to console
        Debug.Log($"Saving Tile Data: Tile ({tile.transform.position.x},{tile.transform.position.y}), Sprite: {sprite.name}, Step: {step}");
    }
}
