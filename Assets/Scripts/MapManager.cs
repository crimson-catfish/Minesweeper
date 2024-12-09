using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Tilemap      tilemap;

    [SerializeField] private Tile   backgroundTile;
    [SerializeField] private Tile   hiddenTile;
    [SerializeField] private Tile   mineTile;
    [SerializeField] private Tile   markTile;
    [SerializeField] private Tile[] numberTiles;

    [SerializeField] private float explotionTime;

    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public int mineCount;

    public Action OnMinePressed;
    public Action OnGameOver;
    public Action OnGameWin;

    private int[,]       map;
    private TileState[,] tileStates;
    private int          hiddenCount;
    private int          markCount = 0;

    private void OnEnable()
    {
        tileStates = new TileState[width, height];
        hiddenCount = width * height;

        inputManager.OnTileReveal += HandleTileReveal;
        inputManager.OnTileMark += HandleTileMark;
    }

    private void Start()
    {
        tilemap.ClearAllTiles();
        tilemap.BoxFill(new Vector3Int(width - 1, height - 1, 0), hiddenTile, 0, 0, width - 1, height - 1);
    }

    private void HandleTileMark(Vector3Int position)
    {
        switch (tileStates[position.x, position.y])
        {
            case TileState.Hidden:
                tileStates[position.x, position.y] = TileState.Marked;
                tilemap.SetTile(position + Vector3Int.forward, markTile);
                markCount++;
                hiddenCount--;

                break;
            case TileState.Marked:
                tileStates[position.x, position.y] = TileState.Hidden;
                tilemap.SetTile(position + Vector3Int.forward, null);
                markCount--;
                hiddenCount++;

                break;
        }

        CheckForWin();
    }

    private void HandleTileReveal(Vector3Int position)
    {
        if (map == null)
            map = MapGenerator.Generate(width, height, mineCount, position.x, position.y);

        if (tileStates[position.x, position.y] != TileState.Hidden)
            return;


        if (map[position.x, position.y] == -1)
        {
            OnMinePressed?.Invoke();
            RevealMines(position);

            return;
        }

        Reveal(position);
    }

    private void Reveal(Vector3Int position)
    {
        if (position.x < 0 ||
            position.y < 0 ||
            position.x >= width ||
            position.y >= height ||
            tileStates[position.x, position.y] == TileState.Revealed)
            return;

        if (tileStates[position.x, position.y] == TileState.Marked)
        {
            tilemap.SetTile(position + Vector3Int.forward, null);
            markCount--;
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

        CheckForWin();
    }

    private void RevealMines(Vector3Int position)
    {
        tileStates[position.x, position.y] = TileState.Revealed;
        tilemap.SetTile(position + Vector3Int.back, backgroundTile);
        tilemap.SetTile(position, mineTile);


        List<Vector2Int> mines = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
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
                markCount--;
            }

            tilemap.SetTile(new Vector3Int(mine.x, mine.y, -1), backgroundTile);
            tilemap.SetTile(new Vector3Int(mine.x, mine.y, 0), mineTile);
            tileStates[mine.x, mine.y] = TileState.Revealed;

            mines.Remove(mine);

            yield return new WaitForSeconds(explotionTime * mines.Count * mineCount);
        }

        OnGameOver?.Invoke();
    }

    private void CheckForWin()
    {
        if (markCount == mineCount && hiddenCount == 0)
            OnGameWin?.Invoke();
    }
}