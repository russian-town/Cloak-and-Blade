using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour, IDataReader
{
    private readonly float _defaultMultiplier = 20f;

    [SerializeField] private AudioMixerGroup _masterGroup;
    [SerializeField] private AudioMixerGroup _soundGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

    public event Action AudioValueChanged;

    public void Read(PlayerData playerData)
    {
        _masterGroup.audioMixer.SetFloat(Constants.MasterVolume, Mathf.Log10(playerData.MasterSliderValue) * _defaultMultiplier);
        _masterGroup.audioMixer.SetFloat(Constants.SoundVolume, Mathf.Log10(playerData.SoundSliderValue) * _defaultMultiplier);
        _masterGroup.audioMixer.SetFloat(Constants.MusicVolume, Mathf.Log10(playerData.MusicSliderValue) * _defaultMultiplier);
    }

    public void Mute()
        => AudioListener.pause = true;

    public void UnMute()
        => AudioListener.pause = false;

    public void ChangeMasterVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(Constants.MasterVolume, Mathf.Log10(value) * _defaultMultiplier);
        AudioValueChanged?.Invoke();
    }

    public void ChangeSoundVolume(float value)
    {
        _soundGroup.audioMixer.SetFloat(Constants.SoundVolume, Mathf.Log10(value) * _defaultMultiplier);
        AudioValueChanged?.Invoke();
    }

    public void ChangeMusicVolume(float value)
    {
        _musicGroup.audioMixer.SetFloat(Constants.MusicVolume, Mathf.Log10(value) * _defaultMultiplier);
        AudioValueChanged?.Invoke();
    }
}
