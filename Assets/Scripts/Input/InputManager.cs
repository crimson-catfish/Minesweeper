using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private Tilemap       tilemap;

    private Controls Controls { set; get; }

    private new Camera      camera;
    private     EventSystem eventSystem;


    private void OnEnable()
    {
        Controls = new Controls();
        camera = Camera.main;

        SetGridActions();
    }

    public Action<Vector3Int> OnTileReveal;
    public Action<Vector3Int> OnTileMarked;


    private void SetGridActions()
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

        Vector3Int gridPosition = PixelsToGridPosition(positionPixels);
        OnTileReveal(gridPosition);
    }

    private void HandleTileMark(InputAction.CallbackContext _)
    {
        Vector2 positionPixels = Controls.Grid.Position.ReadValue<Vector2>();

        if (IsOverUI(positionPixels))
            return;

        Vector3Int gridPosition = PixelsToGridPosition(positionPixels);
        OnTileMarked(gridPosition);
    }


    private Vector3Int PixelsToGridPosition(Vector2 positionPixels)
    {
        Vector3 positionWorld = camera.ScreenToWorldPoint(positionPixels);

        return tilemap.WorldToCell(positionWorld);
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