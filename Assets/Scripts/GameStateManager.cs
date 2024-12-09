using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Image      gameOverImage;


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
        throw new NotImplementedException();
    }
}