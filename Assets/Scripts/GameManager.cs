using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Handles the main game flow,
// including score tracking, UI management, and result processing.

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int _targetScore = 3;         // Score needed to win
    [SerializeField] private float _resetTime = 2f;        // Delay before resetting for the next round

    [Header("Score Handlers")]
    [SerializeField] private ScoreHandler _playerScoreHandler;
    [SerializeField] private ScoreHandler _computerScoreHandler;

    [Header("UI References")]
    [SerializeField] private GameObject _selectionPanel;   // Panel for selecting moves
    [SerializeField] private ResultHandler _resultHandler; // Displays round result
    [SerializeField] private CanvasGroup _combatPanel;     // Displays chosen moves
    [SerializeField] private GameObject _gameOverPanel;    // End-of-game screen
    [SerializeField] private TextMeshProUGUI _gameOverPanelTitle;

    [Header("Audio")]
    [SerializeField] private SoundManager _soundManager;

    private int _playerScore;
    private int _computerScore;
    private Move _playerMove;
    private Move _computerMove;

    private WaitForSeconds _oneSecondWait;
    private WaitForSeconds _resetWait;

    // Events
    public Action OnPickAnotherMove;
    public Move PlayerMove => _playerMove;
    public Move ComputerMove => _computerMove;

    private void Awake()
    {
        _oneSecondWait = new WaitForSeconds(1f);
        _resetWait = new WaitForSeconds(_resetTime);
    }

    public void PlayGame()
    {
        ResetScores();

        // Reset UI
        _combatPanel.alpha = 0;
        _gameOverPanel.SetActive(false);
        _selectionPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Called to evaluate the round after both moves are set.
    public void TriggerShotResult() => StartCoroutine(ShowResult());

    // Sets the player's move and updates the UI to the combat panel.
    public void SetPlayerMove(Move move)
    {
        _playerMove = move;
        _selectionPanel.SetActive(false);
        _combatPanel.alpha = 1f;
    }

    // Sets the computer's move.
    public void SetComputerMove(Move move) => _computerMove = move;

    // Determines the result of the round, updates scores, and plays sounds.
    private string GetResult()
    {
        if (_playerMove == _computerMove)
            return "Draw!";

        bool playerWins = (_playerMove == Move.Rock && _computerMove == Move.Scissors) ||
                          (_playerMove == Move.Paper && _computerMove == Move.Rock) ||
                          (_playerMove == Move.Scissors && _computerMove == Move.Paper);

        if (playerWins)
        {
            _playerScoreHandler.AddScore();
            _playerScore++;
            _soundManager.PlayWinSound();
            return "You Win!";
        }
        else
        {
            _computerScoreHandler.AddScore();
            _computerScore++;
            _soundManager.PlayLoseSound();
            return "You Lose!";
        }
    }

    // Shows the round result, then either ends the game or prepares for the next round.
    private IEnumerator ShowResult()
    {
        yield return _oneSecondWait;

        string result = GetResult();

        if (IsGameOver())
        {
            ShowGameOver();
        }
        else
        {
            _resultHandler.gameObject.SetActive(true);
            _resultHandler.ShowResult(result);
            StartCoroutine(PickAnotherMove());
        }
    }

    // Waits, then resets the UI for the next move.
    private IEnumerator PickAnotherMove()
    {
        yield return _resetWait;

        _resultHandler.gameObject.SetActive(false);
        OnPickAnotherMove?.Invoke();

        _combatPanel.alpha = 0;
        _selectionPanel.SetActive(true);
    }

    // Resets both player and computer scores.
    private void ResetScores()
    {
        _playerScore = 0;
        _computerScore = 0;
        _playerScoreHandler.ResetScores();
        _computerScoreHandler.ResetScores();
    }

    // Checks if either player has reached the target score.
    private bool IsGameOver() =>
        _playerScore == _targetScore || _computerScore == _targetScore;

    // Displays the game over panel with the result.
    private void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);

        if (_playerScore > _computerScore)
        {
            _soundManager.PlayVictorySound();
            _gameOverPanelTitle.text = "VICTORY";
        }
        else
        {
            _gameOverPanelTitle.text = "DEFEAT";
        }
    }
}
