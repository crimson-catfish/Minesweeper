using System;
using System.Collections.Generic;
using RDG;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private Tilemap       tilemap;
    [SerializeField] private MapManager    mapManager;


    private Controls Controls { set; get; }

    private new Camera      camera;
    private     EventSystem eventSystem;


    private void OnEnable()
    {
        Controls = new Controls();
        camera = Camera.main;

        SetGridActions();

        mapManager.OnMinePressed += Controls.Grid.Disable;
        mapManager.OnGameWin += Controls.Grid.Disable;
    }

    public Action<Vector3Int> OnTileReveal;
    public Action<Vector3Int> OnTileMark;

    public void SetGridActions()
    {
        Controls.Grid.Enable();

        Controls.Grid.Reveal.performed += HandleTileReveal;
        Controls.Grid.Mark.performed += HandleTileMark;
    }

    private void HandleTileReveal(InputAction.CallbackContext _)
    {
        Vector2 positionPixels = Controls.Grid.Position.ReadValue<Vector2>();

        if (IsOverUI(positionPixels))
            return;

        Vector3 positionWorld = camera.ScreenToWorldPoint(positionPixels);
        Vector3Int positionGrid = new((int)positionWorld.x, (int)positionWorld.y, 0);

        if (tilemap.HasTile(positionGrid))
            OnTileReveal?.Invoke(positionGrid);
    }

    private void HandleTileMark(InputAction.CallbackContext _)
    {
        Vector2 positionPixels = Controls.Grid.Position.ReadValue<Vector2>();

        if (IsOverUI(positionPixels))
            return;

        Vector3 positionWorld = camera.ScreenToWorldPoint(positionPixels);
        Vector3Int positionGrid = new((int)positionWorld.x, (int)positionWorld.y, 0);

        if (tilemap.HasTile(positionGrid))
            OnTileMark?.Invoke(positionGrid);
    }

    private bool IsOverUI(Vector2 point)
    {
        foreach (Canvas canvas in canvasManager.uiCanvases)
        {
            List<RaycastResult> results = new();

            PointerEventData pointerData = new(eventSystem) { position = point };

            if (canvas.TryGetComponent<GraphicRaycaster>(out var raycaster) == false)
                continue;

            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
                return true;
        }

        return false;
    }
}