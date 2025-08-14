using UnityEngine;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioSource _whooshSound;

    public void ShowHands() => _gameManager.ShowHands();

    public void PlayWhooshSound() => _whooshSound.Play();
}
