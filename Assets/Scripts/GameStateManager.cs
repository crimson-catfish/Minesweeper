using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Image      gameOverImage;
    [SerializeField] private Image      gameWinImage;


    private void OnEnable()
    {
        mapManager.OnGameOver += HandleGameOver;
        mapManager.OnGameWin += HandleGameWin;
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over");
        gameOverImage.enabled = true;
        Time.timeScale = 0;
    }

    private void HandleGameWin()
    {
        gameWinImage.enabled = true;
    }
}