using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton instance

    [SerializeField] private int _width, _height;
    [SerializeField] private Pad _padPrefab;
    [SerializeField] private Tile _tilePrefab;

    private Dictionary<Vector2, Pad> _pads;
    private Dictionary<Vector2, Tile> _tiles;
    private Pad selectedPad;

    public Pad SelectedPad => selectedPad;

    void Awake()
    {
        Instance = this; // Initialize singleton instance
        _pads = new Dictionary<Vector2, Pad>();
        _tiles = new Dictionary<Vector2, Tile>();
    }

    void Start()
    {
        GenerateGrid();
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

                // Set default sprite for tiles (if needed)
                // spawnedTile.SetSprite(defaultSprite);
            }
        }
    }

    public void SetSelectedPad(Pad pad)
    {
        selectedPad = pad;
    }

    public void CopySpriteToTile(Tile tile)
    {
        if (selectedPad != null && tile != null)
        {
            Sprite padSprite = selectedPad.GetCurrentSprite();
            tile.SetSprite(padSprite);
            tile.StartRotation(); // Start rotating the tile
        }
    }

    public void StopTileRotation(Tile tile)
    {
        if (tile != null)
        {
            tile.StopRotation(); // Stop rotating the tile
        }
    }

    public Pad GetPadAtPosition(Vector2 pos)
    {
        if (_pads.TryGetValue(pos, out var pad))
            return pad;
        return null;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            return tile;
        return null;
    }
}
