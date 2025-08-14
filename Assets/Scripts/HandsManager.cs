using UnityEngine;
using UnityEngine.UI;

// Handles displaying and updating the player and computer hands.
public class HandsManager : MonoBehaviour
{
    [Header("Hand Images")]
    [SerializeField] private Image _playerHand;
    [SerializeField] private Image _computerHand;

    [Header("Hand Sprites")]
    [SerializeField] private Sprite _rockSprite;
    [SerializeField] private Sprite _paperSprite;
    [SerializeField] private Sprite _scissorsSprite;

    [Header("Buttons")]
    [SerializeField] private Button _rockButton;
    [SerializeField] private Button _paperButton;
    [SerializeField] private Button _scissorsButton;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioSource _whooshSound;

    private Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start()
    {
        ResetHands();

        // Reset hands when next round starts
        _gameManager.OnPickAnotherMove += ResetHands;

        _rockButton.onClick.AddListener(PickRock);
        _paperButton.onClick.AddListener(PickPaper);
        _scissorsButton.onClick.AddListener(PickScissors);
    }

    private void ResetHands()
    {
        _playerHand.sprite = _rockSprite;
        _computerHand.sprite = _rockSprite;
    }

    // Updates hand images based on chosen moves
    public void ShowHands()
    {
        _playerHand.sprite = GetSpriteForMove(_gameManager.PlayerMove);

        _gameManager.SetComputerMove((Move)Random.Range(0, 3));
        _computerHand.sprite = GetSpriteForMove(_gameManager.ComputerMove);

        _gameManager.TriggerShotResult();
    }

    // Plays the shuffle sound
    public void PlayWhooshSound() => _whooshSound.Play();

    private void PickRock() => PickMove(Move.Rock);
    private void PickPaper() => PickMove(Move.Paper);
    private void PickScissors() => PickMove(Move.Scissors);

    // Sets player move and triggers shuffle animation
    private void PickMove(Move move)
    {
        _gameManager.SetPlayerMove(move);
        _animator.SetTrigger("Shuffle");
    }

    private Sprite GetSpriteForMove(Move move)
    {
        switch (move)
        {
            case Move.Rock:
            {
                return _rockSprite;
            }
            case Move.Paper:
            {
                return _paperSprite;
            }
            case Move.Scissors:
            {
                return _scissorsSprite;
            }
            default:
            {
                return null;
            }
        }
    }
}
