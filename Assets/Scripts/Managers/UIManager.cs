using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    int scoreValue;
    int coinsValue;

    GameDataSO gameDataSO;
    PlayerManager playerManager;

    [Header("Start Game Panel")]
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private TextMeshProUGUI startPanelBestScoreText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverBestScoreText;
    [SerializeField] private TextMeshProUGUI gameOverCoinsText;

    [Header("InGame Panel")]
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI inGameCoinsText;

    void Start()
    {
        gameDataSO = EventManager.getGameDataSO?.Invoke();
        playerManager = EventManager.getPlayer?.Invoke();

        gameStartPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanelBestScoreText.text = "Best: " + PlayerPrefs.GetInt(nameof(gameDataSO.bestScore), 0).ToString();

    }

    void OnEnable()
    {
        EventManager.showGameOverPanel += ShowGameOverPanel;
        EventManager.updateCoinScore += UpdateCoinScore;
    }

    void OnDisable()
    {
        EventManager.showGameOverPanel -= ShowGameOverPanel;
        EventManager.updateCoinScore -= UpdateCoinScore;
    }

    private void ShowGameOverPanel()
    {
        scoreValue = (int)EventManager.getPlayer?.Invoke().transform.position.z;
        if ((scoreValue + coinsValue) > gameDataSO.bestScore)
        {
            gameDataSO.bestScore = (scoreValue + coinsValue);
            gameDataSO.SaveGameData();
            gameOverBestScoreText.color = Color.blue;
            DOTween.To(() => gameOverBestScoreText.fontSize, x => gameOverBestScoreText.fontSize = x, 150, .6f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
            gameOverBestScoreText.text = "New Best: " + PlayerPrefs.GetInt(nameof(gameDataSO.bestScore), 0).ToString();
        }
        else
        {
            gameOverBestScoreText.text = "Best: " + PlayerPrefs.GetInt(nameof(gameDataSO.bestScore), 0).ToString();
        }
        gameDataSO.totalCoins += coinsValue;
        gameDataSO.SaveGameData();

        StartCoroutine(nameof(ShowGameOverPanelDelayed));
    }

    IEnumerator ShowGameOverPanelDelayed()
    {
        yield return new WaitForSeconds(2f);

        gameStartPanel.SetActive(false);
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + scoreValue.ToString() + "m";
        gameOverCoinsText.text = "Coins: " + coinsValue.ToString();
    }

    private void UpdateCoinScore()
    {
        coinsValue++;
        inGameCoinsText.text = coinsValue.ToString();
    }

    private void Update()
    {
        scoreValue = (int)EventManager.getPlayer?.Invoke().transform.position.z;
        inGameScoreText.text = scoreValue + "m";
    }

    public void PlayGameButton()
    {
        gameOverPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        inGamePanel.SetActive(true);
        EventManager.startGame?.Invoke();
    }

    public void RePlayGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
