using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _loseSound;
    [Space]
    [SerializeField] private AudioSource _victorySound;

    public void PlayWinSound() => _winSound.Play();
    public void PlayLoseSound() => _loseSound.Play();
    public void PlayVictorySound() => _victorySound.Play();
}
