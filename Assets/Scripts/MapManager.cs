using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]                               private InputManager inputManager;
    [FormerlySerializedAs("grid"), SerializeField] private Tilemap      tilemap;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int mineCount;


    private int[,] map;

    private void OnEnable()
    {
        inputManager.OnTileReveal += HandleTileReveal;
        inputManager.OnTileMark += HandleTileMark;
    }

    private void HandleTileMark(Vector3Int position)
    {
        Debug.Log(position);
    }

    private void HandleTileReveal(Vector3Int position)
    {
        if (map == null)
            map = MapGenerator.Generate(width, height, mineCount, position.x, position.y);

        Debug.Log(map[position.x, position.y]);
    }
}