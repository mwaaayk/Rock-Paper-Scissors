using UnityEngine;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    public void ShowHands() => _gameManager.ShowHands();
}
