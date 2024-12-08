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


    private int[,] map;

    private void OnEnable()
    {
        inputManager.OnTileReveal += HandleTileReveal;
        inputManager.OnTileMark += HandleTileMark;
    }

    private void Start()
    {
        tilemap.ClearAllTiles();
        tilemap.origin = Vector3Int.zero;
        tilemap.size = new Vector3Int(width, height);
        tilemap.BoxFill(new Vector3Int(width - 1, height - 1, 0), hiddenTile, 0, 0, width - 1, height - 1);
    }

    private void HandleTileMark(Vector3Int position)
    {
        if (tilemap.GetTile(position + Vector3Int.forward) == markTile)
            tilemap.SetTile(position + Vector3Int.forward, null);
        else
            tilemap.SetTile(position + Vector3Int.forward, markTile);
    }

    private void HandleTileReveal(Vector3Int position)
    {
        if (map == null)
            map = MapGenerator.Generate(width, height, mineCount, position.x, position.y);

        Debug.Log(map[position.x, position.y]);
    }
}