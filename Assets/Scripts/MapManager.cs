using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Tilemap      tilemap;
    [SerializeField] private MainCamera   camera;


    [SerializeField] private Tile   backgroundTile;
    [SerializeField] private Tile   hiddenTile;
    [SerializeField] private Tile   mineTile;
    [SerializeField] private Tile   markTile;
    [SerializeField] private Tile[] numberTiles;

    [SerializeField] private float explotionTime;

    public int Width     { private set; get; }
    public int Height    { private set; get; }
    public int MineCount { private set; get; }

    public Action OnMinePressed;
    public Action OnGameOver;
    public Action OnGameWin;

    private int[,]       map;
    private TileState[,] tileStates;
    private int          hiddenCount;

    private void OnEnable()
    {
        inputManager.OnTileReveal += HandleTileReveal;
        inputManager.OnTileMark += HandleTileMark;
    }

    public void SetMap(int width, int height, int mineCount)
    {
        Width = width;
        Height = height;
        MineCount = mineCount;

        map = null;

        tileStates = new TileState[Width, Height];
        hiddenCount = Width * Height;
        tilemap.ClearAllTiles();
        tilemap.BoxFill(new Vector3Int(Width - 1, Height - 1, 0), hiddenTile, 0, 0, Width - 1, Height - 1);

        camera.SetUpCamera();
    }

    private void HandleTileMark(Vector3Int position)
    {
        switch (tileStates[position.x, position.y])
        {
            case TileState.Hidden:
                tileStates[position.x, position.y] = TileState.Marked;
                tilemap.SetTile(position + Vector3Int.forward, markTile);

                break;
            case TileState.Marked:
                tileStates[position.x, position.y] = TileState.Hidden;
                tilemap.SetTile(position + Vector3Int.forward, null);

                break;
        }

        CheckForWin();
    }

    private void HandleTileReveal(Vector3Int position)
    {
        if (map == null)
            map = MapGenerator.Generate(Width, Height, MineCount, position.x, position.y);

        if (tileStates[position.x, position.y] != TileState.Hidden)
            return;


        if (map[position.x, position.y] == -1)
        {
            OnMinePressed?.Invoke();
            RevealMines(position);

            return;
        }

        Reveal(position);
        CheckForWin();
    }

    private void Reveal(Vector3Int position)
    {
        if (position.x < 0 ||
            position.y < 0 ||
            position.x >= Width ||
            position.y >= Height ||
            tileStates[position.x, position.y] == TileState.Revealed)
            return;

        if (tileStates[position.x, position.y] == TileState.Marked)
        {
            tilemap.SetTile(position + Vector3Int.forward, null);
        }
        else
            hiddenCount--;

        tileStates[position.x, position.y] = TileState.Revealed;
        tilemap.SetTile(position + Vector3Int.back, backgroundTile);

        if (map[position.x, position.y] > 0)
        {
            tilemap.SetTile(position, numberTiles[map[position.x, position.y] - 1]);

            return;
        }

        tilemap.SetTile(position, null);

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                Reveal(new Vector3Int(position.x + dx, position.y + dy, 0));
            }
        }
    }

    private void RevealMines(Vector3Int position)
    {
        tileStates[position.x, position.y] = TileState.Revealed;
        tilemap.SetTile(position + Vector3Int.back, backgroundTile);
        tilemap.SetTile(position, mineTile);


        List<Vector2Int> mines = new();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (map[x, y] == -1 && tileStates[x, y] != TileState.Revealed)
                    mines.Add(new Vector2Int(x, y));
            }
        }

        StartCoroutine(RevealMinesCoroutine(mines));
    }

    private IEnumerator RevealMinesCoroutine(List<Vector2Int> mines)
    {
        while (mines.Count > 0)
        {
            Vector2Int mine = mines[Random.Range(0, mines.Count)];

            if (tileStates[mine.x, mine.y] == TileState.Marked)
            {
                tilemap.SetTile(new Vector3Int(mine.x, mine.y, 1), null);
            }

            tilemap.SetTile(new Vector3Int(mine.x, mine.y, -1), backgroundTile);
            tilemap.SetTile(new Vector3Int(mine.x, mine.y, 0), mineTile);
            tileStates[mine.x, mine.y] = TileState.Revealed;

            mines.Remove(mine);

            yield return new WaitForSeconds(explotionTime * mines.Count * MineCount);
        }

        OnGameOver?.Invoke();
    }

    private void CheckForWin()
    {
        if (hiddenCount == MineCount)
            OnGameWin?.Invoke();
    }
}