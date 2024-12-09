using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Tilemap      tilemap;

    [SerializeField] private Tile   backgroundTile;
    [SerializeField] private Tile   hiddenTile;
    [SerializeField] private Tile   mineTile;
    [SerializeField] private Tile   markTile;
    [SerializeField] private Tile[] numberTiles;

    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public int mineCount;


    private int[,]       map;
    private TileState[,] tileStates;

    private void OnEnable()
    {
        tileStates = new TileState[width, height];

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

                break;
            case TileState.Marked:
                tileStates[position.x, position.y] = TileState.Hidden;
                tilemap.SetTile(position + Vector3Int.forward, null);

                break;
        }
    }

    private void HandleTileReveal(Vector3Int position)
    {
        if (map == null)
            map = MapGenerator.Generate(width, height, mineCount, position.x, position.y);

        if (tileStates[position.x, position.y] != TileState.Hidden)
            return;

        if (map[position.x, position.y] == -1)
        {
            tilemap.SetTile(position + Vector3Int.back, backgroundTile);
            tilemap.SetTile(position, mineTile);

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
            tilemap.SetTile(position + Vector3Int.forward, null);

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
}