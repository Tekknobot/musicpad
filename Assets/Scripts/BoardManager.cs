using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BoardManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;
 
    private Dictionary<Vector2, Tile> _tiles;
 
    float step;
    float noteDuration = 1.0f;

    void Start() {
        GenerateGrid();
    }
 
    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(y, x), Quaternion.identity);
                spawnedTile.step = step;
                spawnedTile.duration = noteDuration;
                spawnedTile.name = $"Tile ({y},{x})";

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }
 
    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}