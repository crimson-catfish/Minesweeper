using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

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

        mapManager.SetMap(width, height, mineCount);

        gameOverImage.enabled = false;
        gameWinImage.enabled = false;
        difficultyPanel.SetActive(false);

        mapManager.OnGameOver += HandleGameOver;
        mapManager.OnGameWin += HandleGameWin;
    }

    private void HandleGameOver()
    {
        gameOverImage.enabled = true;
        difficultyPanel.SetActive(true);
    }

    private void HandleGameWin()
    {
        gameWinImage.enabled = true;
        difficultyPanel.SetActive(true);
    }
}