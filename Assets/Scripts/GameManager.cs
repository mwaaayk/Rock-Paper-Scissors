using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _targetScore = 3;
    [SerializeField] private float _resetTime = 2f;
    [Space]
    [SerializeField] private Image _playerHand;
    [SerializeField] private Image _computerHand;
    [Space]
    [SerializeField] private ScoreHandler _playerScoreHandler;
    [SerializeField] private ScoreHandler _computerScoreHandler;
    [Space]
    [SerializeField] private Sprite _rockSprite;
    [SerializeField] private Sprite _paperSprite;
    [SerializeField] private Sprite _scissorsSprite;
    [Space]
    [SerializeField] private Button _rockButton;
    [SerializeField] private Button _paperButton;
    [SerializeField] private Button _scissorsButton;
    [Space]
    [SerializeField] private Animator _animator;
    [Space]
    [SerializeField] private GameObject _selectionPanel;
    [SerializeField] private ResultHandler _resultHandler;
    [SerializeField] private CanvasGroup _combatPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _gameOverPanelTitle;
    [SerializeField] private SoundManager _soundManager;

    private int _playerScore = 0;
    private int _computerScore = 0;
    private Move _playerMove;
    private Move _computerMove;

    private enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    public void PlayGame()
    {
        _playerScore = 0;
        _computerScore = 0;
        _playerScoreHandler.ResetScores();
        _computerScoreHandler.ResetScores();

        _playerHand.sprite = _rockSprite;
        _computerHand.sprite = _rockSprite;

        _combatPanel.alpha = 0;
        _gameOverPanel.SetActive(false);
        _selectionPanel.SetActive(true);

        _rockButton.onClick.AddListener(PickRock);
        _paperButton.onClick.AddListener(PickPaper);
        _scissorsButton.onClick.AddListener(PickScissors);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShuffleHands()
    {
        _selectionPanel.SetActive(false);
        _combatPanel.alpha = 1f;
        _animator.SetTrigger("Shuffle");
    }

    public void ShowHands()
    {
        _playerHand.sprite = GetSpriteForMove(_playerMove);

        _computerMove = (Move)Random.Range(0, 3);
        _computerHand.sprite = GetSpriteForMove(_computerMove);

        StartCoroutine(ShowResult());
    }

    public void PickRock()
    {
        _playerMove = Move.Rock;
        ShuffleHands();
    }

    public void PickPaper()
    {
        _playerMove = Move.Paper;
        ShuffleHands();
    }

    public void PickScissors()
    {
        _playerMove = Move.Scissors;
        ShuffleHands();
    }

    private Sprite GetSpriteForMove(Move move)
    {
        switch (move)
        {
            case Move.Rock:
                return _rockSprite;
            case Move.Paper:
                return _paperSprite;
            case Move.Scissors:
                return _scissorsSprite;
            default:
                return null;
        }
    }

    private string GetResult()
    {
        if (_playerMove == _computerMove)
            return "Draw!";

        if ((_playerMove == Move.Rock && _computerMove == Move.Scissors) ||
            (_playerMove == Move.Paper && _computerMove == Move.Rock) ||
            (_playerMove == Move.Scissors && _computerMove == Move.Paper))
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

    private IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);
        string result = GetResult();

        if (_playerScore == _targetScore || _computerScore == _targetScore)
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
        else
        {
            _resultHandler.gameObject.SetActive(true);
            _resultHandler.ShowResult(result);
            StartCoroutine(PickAnotherMove());
        }
    }

    private IEnumerator PickAnotherMove()
    {
        yield return new WaitForSeconds(_resetTime);
        _resultHandler.gameObject.SetActive(false);
        _playerHand.sprite = _rockSprite;
        _computerHand.sprite = _rockSprite;
        _combatPanel.alpha = 0;
        _selectionPanel.SetActive(true);
    }
}
