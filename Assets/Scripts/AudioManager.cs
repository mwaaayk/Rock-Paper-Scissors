using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [Space]
    [SerializeField] private Image _soundButtonIcon;
    [SerializeField] private Image _musicButtonIcon;
    [Space]
    [SerializeField] private Sprite _soundOnIcon;
    [SerializeField] private Sprite _soundOffIcon;
    [SerializeField] private Sprite _musicOnIcon;
    [SerializeField] private Sprite _musicOffIcon;

    private bool _musicEnabled = true;
    private bool _soundEnabled = true;

    public void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
        _musicButtonIcon.sprite = _musicEnabled ? _musicOnIcon : _musicOffIcon;
        ApplySettings();
    }

    public void ToggleSFX()
    {
        _soundEnabled = !_soundEnabled;
        _soundButtonIcon.sprite = _soundEnabled ? _soundOnIcon : _soundOffIcon;
        ApplySettings();
    }

    private void ApplySettings()
    {
        SetMusicVolume(_musicEnabled ? 0f : -80f);
        SetSFXVolume(_soundEnabled ? 0f : -80f);
    }

    private void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    private void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }
}
