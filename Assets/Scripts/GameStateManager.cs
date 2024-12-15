using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private MapManager   mapManager;
    [SerializeField] private InputManager inputManager;


    [SerializeField] private Image          background;
    [SerializeField] private GameObject     difficultyPanel;
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    [SerializeField] private TMP_InputField minesInputField;

    [SerializeField] private Image gameOverImage;
    [SerializeField] private Image gameWinImage;

    public void StartGame()
    {
        int width = int.Parse(widthInputField.text);
        int height = int.Parse(heightInputField.text);
        int mineCount = int.Parse(minesInputField.text);

        if (mineCount > width * height / 2 - 1)
            return;

        mapManager.SetMap(width, height, mineCount);

        background.enabled = false;
        gameOverImage.enabled = false;
        gameWinImage.enabled = false;
        difficultyPanel.SetActive(false);

        inputManager.SetGridActions();
        mapManager.OnGameOver += HandleGameOver;
        mapManager.OnGameWin += HandleGameWin;
    }

    private void HandleGameOver()
    {
        background.enabled = true;
        gameOverImage.enabled = true;
        difficultyPanel.SetActive(true);
    }

    private void HandleGameWin()
    {
        background.enabled = true;
        gameWinImage.enabled = true;
        difficultyPanel.SetActive(true);
    }
}