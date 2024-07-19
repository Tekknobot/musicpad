using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Pad _padPrefab;
    [SerializeField] private Sprite[] _sprites; // Array of sprites to swap with

    private Dictionary<Vector2, Pad> _pads;
    private int _spriteIndex; // Index to track current sprite

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _pads = new Dictionary<Vector2, Pad>();
        _spriteIndex = 0; // Start with the first sprite in _sprites array

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedPad = Instantiate(_padPrefab, new Vector3(x, y - 2), Quaternion.identity);
                spawnedPad.name = $"DrumPad ({x},{y})";

                // Set the default sprite for the pad
                spawnedPad.SetSprite(_sprites[_spriteIndex]);

                _pads[new Vector2(x, y)] = spawnedPad;

                // Increment sprite index and wrap around if necessary
                _spriteIndex = (_spriteIndex + 1) % _sprites.Length;
            }
        }
    }

    public Pad GetPadAtPosition(Vector2 pos)
    {
        if (_pads.TryGetValue(pos, out var pad))
            return pad;
        return null;
    }

    public void SwapDefaultSprites(Sprite[] newSprites)
    {
        if (newSprites.Length != _sprites.Length)
        {
            Debug.LogWarning("New sprites array length does not match current sprites array length.");
            return;
        }

        _sprites = newSprites;
        _spriteIndex = 0; // Reset sprite index

        // Update all instantiated pads with the new sprites
        foreach (var pad in _pads.Values)
        {
            pad.SetSprite(_sprites[_spriteIndex]);

            // Increment sprite index and wrap around if necessary
            _spriteIndex = (_spriteIndex + 1) % _sprites.Length;
        }
    }
}
