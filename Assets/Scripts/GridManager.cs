using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Grid         grid;

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
            Debug.Log(position);
        }
    }
}